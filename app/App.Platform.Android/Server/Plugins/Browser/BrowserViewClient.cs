using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using Android.Webkit;
using App.Platform.Android.Shared;
using App.Platform.Android.Utilities;
using App.Platform.Android.Utilities.Extensions;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserViewClient : WebViewClient
    {
        private readonly Controller _controller;
        private readonly ConcurrentBag<TaskCompletionSource<bool>> _navigations;
        private readonly ConcurrentDictionary<string, BrowserResponse> _results;
        private readonly string _viewId;

        #region Abstracts

        private WebResourceResponse Persist(HttpWebResponse response, string url)
        {
            var result = BrowserResponse.Create(response);
            if (_results.TryAdd(url, result)) throw new Exception("Use ServerReceiver"); // _core.DispatchEvent($"browser.{_viewId}", url);
            return result.ToWebViewResponse();
        }

        #endregion

        #region Constructor

        public BrowserViewClient(Controller controller, string viewId)
        {
            _controller = controller;
            _navigations = new ConcurrentBag<TaskCompletionSource<bool>>();
            _results = new ConcurrentDictionary<string, BrowserResponse>();
            _viewId = viewId;
        }
        
        #endregion

        #region Methods

        public Task<byte[]> ResponseAsync(string url)
        {
            return Task.FromResult(_results.TryGetValue(url, out var response) ? response.Buffer : null);
        }

        public async Task WaitForNavigateAsync()
        {
            var tcs = new TimeoutTaskCompletionSource<bool>(30);
            _navigations.Add(tcs);
            await tcs.Task;
        }

        #endregion

        #region Overrides of WebViewClient

        public override void OnPageCommitVisible(WebView view, string url)
        {
            while (_navigations.TryTake(out var navigation))
            {
                navigation.TrySetResult(true);
            }
        }
        
        public override WebResourceResponse ShouldInterceptRequest(WebView view, IWebResourceRequest request)
        {
            try
            {
                if (request.Method != "GET") return base.ShouldInterceptRequest(view, request);
                if (_results.TryGetValue(request.Url.ToString(), out var result)) return result.ToWebViewResponse();
                var http = WebRequest.CreateHttp(request.Url.ToString());
                http.Method = request.Method;
                http.CopyCookies(CookieManager.Instance);
                http.CopyHeaders(request.RequestHeaders);
                return Persist((HttpWebResponse) http.GetResponse(), request.Url.ToString());
            }
            catch (WebException ex) when (ex.Response is HttpWebResponse response)
            {
                var statusCode = (int) response.StatusCode;
                if (statusCode >= 300 && statusCode < 400) return base.ShouldInterceptRequest(view, request);
                return Persist(response, request.Url.ToString());
            }
            catch (WebException)
            {
                return base.ShouldInterceptRequest(view, request);
            }
        }

        #endregion
    }
}