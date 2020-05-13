using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Webkit;
using App.Core.Shared;
using App.Platform.Android.Server.Plugins.Browser.Enumerators;
using App.Platform.Android.Server.Plugins.Browser.Extensions;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserViewClient : WebViewClient
    {
        private readonly ConcurrentDictionary<string, TimeoutTaskCompletionSource<byte[]>> _responseTcs;
        private readonly ConcurrentBag<TimeoutTaskCompletionSource<bool>> _visibleTcs;

        #region Abstracts

        private WebResourceResponse Cache(string method, string url, IDictionary<string, string> headers)
        {
            try
            {
                // Initialize the request.
                var request = WebRequest.CreateHttp(url);
                request.CopyCookies(CookieManager.Instance);
                request.CopyHeaders(headers);
                request.Method = method;

                // Initialize the response.
                using var memoryStream = new MemoryStream();
                using var response = (HttpWebResponse) request.GetResponse();
                using var responseStream = response.GetResponseStream();
                responseStream?.CopyTo(memoryStream);

                // Initialize the response buffer.
                var responseBuffer = memoryStream.ToArray();
                var responseTcs = _responseTcs.GetOrAdd(url, x => new TimeoutTaskCompletionSource<byte[]>());
                responseTcs.TrySetResult(responseBuffer);

                // Return the response.
                var contentEncoding = response.ContentEncoding;
                var contentType = Regex.Replace(response.ContentType, @"\s*;(.*)$", string.Empty);
                var responseHeaders = response.Headers.AllKeys.ToDictionary(x => x, x => response.Headers[x]);
                var statusCode = (int) response.StatusCode;
                var statusDescription = response.StatusDescription;
                var stream = new MemoryStream(responseBuffer);
                return new WebResourceResponse(contentType, contentEncoding, statusCode, statusDescription, responseHeaders, stream);
            }
            catch (WebException)
            {
                return null;
            }
        }

        #endregion

        #region Constructor

        public BrowserViewClient()
        {
            _responseTcs = new ConcurrentDictionary<string, TimeoutTaskCompletionSource<byte[]>>();
            _visibleTcs = new ConcurrentBag<TimeoutTaskCompletionSource<bool>>();
        }

        #endregion

        #region Methods

        public async Task<byte[]> ResponseAsync(string url)
        {
            var responseTcs = _responseTcs.GetOrAdd(url, x => new TimeoutTaskCompletionSource<byte[]>());
            var response = await responseTcs.Task;
            return response;
        }

        public async Task WaitForVisibleAsync()
        {
            var visibleTcs = new TimeoutTaskCompletionSource<bool>();
            _visibleTcs.Add(visibleTcs);
            await visibleTcs.Task;
        }

        #endregion

        #region Overrides of WebViewClient

        public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
        {
            if (view.Url != request.Url.ToString()) return;
            while (_visibleTcs.TryTake(out var visibleTcs)) visibleTcs.TrySetCanceled();
        }

        public override void OnPageCommitVisible(WebView view, string url)
        {
            while (_visibleTcs.TryTake(out var visibleTcs)) visibleTcs.TrySetResult(true);
        }

        public override WebResourceResponse ShouldInterceptRequest(WebView view, IWebResourceRequest request)
        {
            switch (BrowserViewFilter.GetState(request.Url.Host))
            {
                case FilterState.Block:
                    return new WebResourceResponse("text/plain", "UTF-8", null);
                case FilterState.Cache:
                    return Cache(request.Method, request.Url.ToString(), request.RequestHeaders);
                default:
                    return null;
            }
        }

        #endregion
    }
}