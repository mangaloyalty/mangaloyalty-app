using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserHttpCache
    {
        private readonly HttpClient _client;
        private readonly ConcurrentDictionary<string, BrowserHttpRequest> _requests;

        #region Constructor

        public BrowserHttpCache(HttpClient client)
        {
            _client = client;
            _requests = new ConcurrentDictionary<string, BrowserHttpRequest>(StringComparer.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Methods

        public async Task<BrowserHttpResponse> GetAsync(string url, IDictionary<string, string> headers)
        {
            return await _requests.GetOrAdd(url, x => BrowserHttpRequest.Create(_client, url, headers)).GetAsync();
        }

        #endregion
    }
}