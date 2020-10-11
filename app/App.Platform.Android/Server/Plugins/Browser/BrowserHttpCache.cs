using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserHttpCache : IDisposable
    {
        private readonly HttpClient _client;
        private readonly Dictionary<string, DateTime> _expires;
        private readonly Dictionary<string, BrowserHttpRequest> _requests;
        private readonly object _syncRoot;
        private bool _isDisposed;

        #region Abstracts

        private async Task ExpireAsync()
        {
            while (!_isDisposed)
            {
                lock (_syncRoot)
                {
                    foreach (var (key, _) in _expires.Where(x => x.Value < DateTime.Now).ToList())
                    {
                        _requests.Remove(key);
                        _expires.Remove(key);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        #endregion

        #region Constructor

        private BrowserHttpCache(HttpClient client)
        {
            _client = client;
            _expires = new Dictionary<string, DateTime>();
            _requests = new Dictionary<string, BrowserHttpRequest>();
            _syncRoot = new object();
        }

        public static BrowserHttpCache Create(HttpClient client)
        {
            var cache = new BrowserHttpCache(client);
            _ = Task.Run(cache.ExpireAsync);
            return cache;
        }

        #endregion

        #region Methods

        public async Task<BrowserHttpResponse> GetAsync(string url, IDictionary<string, string> headers)
        {
            BrowserHttpRequest request;

            lock (_syncRoot)
            {
                if (_isDisposed) throw new ObjectDisposedException(nameof(GetAsync));
                if (!_requests.TryGetValue(url, out request)) request = new BrowserHttpRequest(_client, url);
                _expires[url] = DateTime.Now.AddMinutes(1);
                _requests[url] = request;
            }

            return await request.GetAsync(headers);
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            lock (_syncRoot)
            {
                _isDisposed = true;
            }
        }

        #endregion
    }
}