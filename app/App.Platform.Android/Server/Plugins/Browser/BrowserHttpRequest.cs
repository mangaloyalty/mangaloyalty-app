using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.Webkit;
using App.Platform.Android.Server.Plugins.Browser.Extensions;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserHttpRequest
    {
        private readonly HttpClient _client;
        private readonly string _url;

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
                        var message = new HttpRequestMessage(HttpMethod.Get, _url);
                        message.CopyHeaders(headers, "If-Modified-Since", "If-None-Match");
                        message.CopyCookies(CookieManager.Instance);

                        // Initialize the response.
                        using var response = await _client.SendAsync(message, cancellationSource.Token);
                        using var responseContent = response.Content;
                        var responseBuffer = await responseContent.ReadAsByteArrayAsync();
                        return new BrowserHttpResponse(responseBuffer, responseContent, response);
                    }
                    catch (HttpRequestException)
                    {
                        if (cancellationSource.IsCancellationRequested) throw;
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationSource.Token);
                    }
                    catch (OperationCanceledException)
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