using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using Android.Webkit;
using App.Platform.Android.Utilities;
using App.Platform.Android.Utilities.Extensions;

namespace App.Platform.Android.Plugins.Browser
{
    public class BrowserViewClient : WebViewClient
    {
        private readonly MainActivity _activity;
        private readonly ConcurrentBag<TaskCompletionSource<bool>> _navigations;
        private readonly ConcurrentDictionary<string, BrowserResponse> _responses;
        private readonly string _viewId;

        #region Abstracts

        private WebResourceResponse PersistResponse(HttpWebResponse response, string url)
        {
            var result = BrowserResponse.CreateFrom(response);
            if (_responses.TryAdd(url, result)) _activity.DispatchEvent($"browser.{_viewId}", url);
            return result.ToWebViewResponse();
        }

        #endregion

        #region Constructor

        public BrowserViewClient(MainActivity activity, string viewId)
        {
            _activity = activity;
            _navigations = new ConcurrentBag<TaskCompletionSource<bool>>();
            _responses = new ConcurrentDictionary<string, BrowserResponse>();
            _viewId = viewId;
        }

        #endregion

        #region Methods

        public Task<byte[]> ResponseAsync(string url)
        {
            return _responses.TryGetValue(url, out var response) ? Task.FromResult(response.Buffer) : null;
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
                if (_responses.TryGetValue(request.Url.ToString(), out var cache)) return cache.ToWebViewResponse();
                var http = WebRequest.CreateHttp(request.Url.ToString());
                http.Method = request.Method;
                http.CopyHeaders(request.RequestHeaders);
                return PersistResponse((HttpWebResponse) http.GetResponse(), request.Url.ToString());
            }
            catch (WebException ex) when (ex.Response is HttpWebResponse response)
            {
                var statusCode = (int) response.StatusCode;
                if (statusCode >= 300 && statusCode < 400) return base.ShouldInterceptRequest(view, request);
                return PersistResponse(response, request.Url.ToString());
            }
            catch (WebException)
            {
                return base.ShouldInterceptRequest(view, request);
            }
        }
        
        #endregion
    }
}