using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Webkit;
using App.Core.Shared;
using App.Platform.Android.Server.Plugins.Browser.Enumerators;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserViewClient : WebViewClient
    {
        private static readonly HttpClient HttpClient;
        private readonly BrowserHttpCache _cache;
        private readonly ConcurrentDictionary<string, TimeoutTaskCompletionSource<byte[]>> _responseTcs;
        private readonly ConcurrentBag<TimeoutTaskCompletionSource<bool>> _visibleTcs;

        #region Abstracts

        private async Task<WebResourceResponse> CacheAsync(string method, string url, IDictionary<string, string> headers)
        {
            try
            {
                if (method != "GET") return null;
                var responseTcs = _responseTcs.GetOrAdd(url, x => new TimeoutTaskCompletionSource<byte[]>());
                var response = await _cache.GetAsync(url, headers);
                responseTcs.TrySetResult(response.Buffer);
                return response.ToResourceResponse();
            }
            catch (Exception)
            {
                return new WebResourceResponse("text/plain", "UTF-8", null);
            }
        }

        #endregion

        #region Constructor

        public BrowserViewClient()
        {
            _cache = new BrowserHttpCache(HttpClient);
            _responseTcs = new ConcurrentDictionary<string, TimeoutTaskCompletionSource<byte[]>>();
            _visibleTcs = new ConcurrentBag<TimeoutTaskCompletionSource<bool>>();
        }

        static BrowserViewClient()
        {
            HttpClient = new HttpClient(new Xamarin.Android.Net.AndroidClientHandler
            {
                ConnectTimeout = TimeSpan.FromSeconds(5),
                ReadTimeout = TimeSpan.FromSeconds(5)
            });
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
                    return CacheAsync(request.Method, request.Url.ToString(), request.RequestHeaders).Result;
                default:
                    return null;
            }
        }

        #endregion
    }
}