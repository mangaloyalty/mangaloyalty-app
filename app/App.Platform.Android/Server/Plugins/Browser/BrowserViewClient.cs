﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Webkit;
using App.Core.Shared;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserViewClient : WebViewClient
    {
        private static readonly BrowserHttpCache Http;
        private readonly ConcurrentDictionary<string, TimeoutTaskCompletionSource<BrowserHttpResponse>> _responseTcs;
        private readonly ConcurrentBag<TimeoutTaskCompletionSource<bool>> _visibleTcs;

        #region Abstracts

        private async Task<WebResourceResponse> RequestAsync(string method, string url, IDictionary<string, string> headers)
        {
            try
            {
                if (!string.Equals(method, "GET", StringComparison.InvariantCultureIgnoreCase)) return null;
                var responseTcs = _responseTcs.GetOrAdd(url, x => new TimeoutTaskCompletionSource<BrowserHttpResponse>());
                var response = await Http.GetAsync(url, headers);
                responseTcs.TrySetResult(response);
                return response.ToResourceResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new WebResourceResponse("text/plain", null, null);
            }
        }

        #endregion

        #region Constructor

        public BrowserViewClient()
        {
            _responseTcs = new ConcurrentDictionary<string, TimeoutTaskCompletionSource<BrowserHttpResponse>>();
            _visibleTcs = new ConcurrentBag<TimeoutTaskCompletionSource<bool>>();
        }

        static BrowserViewClient()
        {
            Http = BrowserHttpCache.Create(new HttpClient(new Xamarin.Android.Net.AndroidClientHandler
            {
                ConnectTimeout = TimeSpan.FromSeconds(5),
                ReadTimeout = TimeSpan.FromSeconds(5),
                UseCookies = false
            }));
        }

        #endregion

        #region Methods

        public async Task<BrowserHttpResponse> ResponseAsync(string url)
        {
            var responseTcs = _responseTcs.GetOrAdd(url, x => new TimeoutTaskCompletionSource<BrowserHttpResponse>());
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
            return RequestAsync(request.Method, request.Url.ToString(), request.RequestHeaders).Result;
        }

        #endregion
    }
}