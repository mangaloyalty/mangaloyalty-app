using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Webkit;
using App.Core.Shared;
using App.Platform.Android.Server.Plugins.Browser.Extensions;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserHttpRequest
    {
        private readonly HttpClient _client;
        private readonly IDictionary<string, string> _headers;
        private readonly TimeoutTaskCompletionSource<BrowserHttpResponse> _responseTcs;
        private readonly string _url;

        #region Abstracts

        private async Task RunAsync()
        {
            while (!_responseTcs.Task.IsCompleted)
            {
                try
                {
                    // Initialize the request.
                    var message = new HttpRequestMessage(HttpMethod.Get, _url);
                    message.CopyCookies(CookieManager.Instance);
                    message.CopyHeaders(_headers, "If-Modified-Since", "If-None-Match");

                    // Initialize the response.
                    using var response = await _client.SendAsync(message, _responseTcs.CancellationToken);
                    using var responseContent = response.Content;
                    var responseBuffer = await responseContent.ReadAsByteArrayAsync();
                    _responseTcs.TrySetResult(BrowserHttpResponse.Create(responseBuffer, responseContent, response));
                }
                catch (Exception)
                {
                    if (_responseTcs.Task.IsCompleted) return;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        #endregion

        #region Constructor

        private BrowserHttpRequest(HttpClient client, string url, IDictionary<string, string> headers)
        {
            _client = client;
            _headers = headers;
            _responseTcs = new TimeoutTaskCompletionSource<BrowserHttpResponse>();
            _url = url;
        }

        public static BrowserHttpRequest Create(HttpClient client, string url, IDictionary<string, string> headers)
        {
            var request = new BrowserHttpRequest(client, url, headers);
            _ = request.RunAsync();
            return request;
        }

        #endregion

        #region Methods

        public async Task<BrowserHttpResponse> GetAsync()
        {
            return await _responseTcs.Task;
        }

        #endregion
    }
}