using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using App.Core.Server;
using App.Core.Server.Models;
using App.Platform.Android.Server.Plugins.Browser;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Plugins
{
    public class BrowserPlugin : IBrowserPlugin
    {
        private readonly Context _context;
        private readonly TaskCompletionSource<bool> _initTcs;
        private readonly ConcurrentDictionary<string, BrowserView> _views;
        private int _previousId;

        #region Constructor

        public BrowserPlugin(Context context, TaskCompletionSource<bool> initTcs)
        {
            _context = context;
            _initTcs = initTcs;
            _views = new ConcurrentDictionary<string, BrowserView>();
        }

        #endregion

        #region Implementation of IBrowserPlugin
        
        public Task BootAsync()
        {
            _initTcs.TrySetResult(true);
            return Task.CompletedTask;
        }

        public async Task<string> CreateAsync()
        {
            var viewId = Interlocked.Increment(ref _previousId).ToString();
            var view = await BrowserView.CreateAsync(_context);
            if (_views.TryAdd(viewId, view)) return viewId;
            await view.DestroyAsync();
            throw new Exception();
        }

        public async Task DestroyAsync(BrowserDataModel model)
        {
            if (!_views.TryRemove(model.Id, out var view)) throw new Exception();
            await view.DestroyAsync();
        }

        public async Task<JToken> EvaluateAsync(BrowserEvaluateDataModel model)
        {
            if (!_views.TryGetValue(model.Id, out var view)) throw new Exception();
            return await view.EvaluateAsync(model.Invoke);
        }

        public async Task NavigateAsync(BrowserNavigateDataModel model)
        {
            if (!_views.TryGetValue(model.Id, out var view)) throw new Exception();
            await view.NavigateAsync(model.Url);
        }

        public async Task<string> ResponseAsync(BrowserResponseDataModel model)
        {
            if (!_views.TryGetValue(model.Id, out var view)) throw new Exception();
            var buffer = await view.ResponseAsync(model.Url);
            return Convert.ToBase64String(buffer);
        }

        public async Task WaitForNavigateAsync(BrowserDataModel model)
        {
            if (!_views.TryGetValue(model.Id, out var view)) throw new Exception();
            await view.WaitForNavigateAsync();
        }

        #endregion
    }
}