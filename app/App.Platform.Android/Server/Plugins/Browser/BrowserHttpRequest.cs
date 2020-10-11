using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.Webkit;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserHttpRequest
    {
        private readonly HttpClient _client;
        private readonly string _url;
        private BrowserHttpResponse _previousResponse;

        #region Constructor

        public BrowserHttpRequest(HttpClient client, string url)
        {
            _client = client;
            _url = url;
        }
        
        #endregion

        #region Methods

        public async Task<BrowserHttpResponse> GetAsync(IDictionary<string, string> headers)
        {
            using (var cancellationSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            {
                while (true)
                {
                    try
                    {
                        // Initialize the request.
                        var cookie = CookieManager.Instance.GetCookie(_url);
                        var message = new HttpRequestMessage(HttpMethod.Get, _url);
                        var response = _previousResponse;

                        // Initialize the request headers.
                        foreach (var (key, value) in headers)
                            message.Headers.Add(key, value);
                        if (cookie != null)
                            message.Headers.Add("Cookie", cookie);
                        if (response?.Headers.ContainsKey("ETag") == true)
                            message.Headers.Add("If-None-Match", response.Headers["ETag"]);
                        if (response?.Headers.ContainsKey("Last-Modified") == true)
                            message.Headers.Add("If-Modified-Since", response.Headers["Last-Modified"]);

                        // Initialize the response.
                        using var messageResponse = await _client.SendAsync(message, cancellationSource.Token);
                        using var messageContent = messageResponse.Content;
                        var messageBuffer = await messageContent.ReadAsByteArrayAsync();

                        // Return the response.
                        if (response != null && messageResponse.StatusCode == HttpStatusCode.NotModified) return response;
                        response = new BrowserHttpResponse(messageBuffer, messageContent, messageResponse);
                        return (_previousResponse = response);
                    }
                    catch (Exception)
                    {
                        if (cancellationSource.IsCancellationRequested) throw;
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationSource.Token);
                    }
                }
            }
        }

        #endregion
    }
}