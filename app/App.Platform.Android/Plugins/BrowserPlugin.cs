using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Models.Plugins;
using App.Core.Plugins;
using App.Platform.Android.Plugins.Browser;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Plugins
{
    public sealed class BrowserPlugin : IBrowserPlugin
    {
        private readonly MainActivity _activity;
        private readonly ConcurrentDictionary<string, BrowserView> _views;
        private int _previousId;

        #region Constructor

        public BrowserPlugin(MainActivity activity)
        {
            _activity = activity;
            _views = new ConcurrentDictionary<string, BrowserView>();
        }

        #endregion

        #region Implementation of IBrowserPlugin
        
        public Task BootAsync()
        {
            return Task.CompletedTask;
        }

        public async Task<string> CreateAsync()
        {
            var viewId = Interlocked.Increment(ref _previousId).ToString();
            var view = await BrowserView.CreateAsync(_activity, viewId);
            if (!_views.TryAdd(viewId, view)) await view.DestroyAsync();
            return viewId;
        }

        public async Task DestroyAsync(BrowserDataModel model)
        {
            if (!_views.TryRemove(model.Id, out var view)) return;
            await view.DestroyAsync();
        }

        public async Task<JToken> EvaluateAsync(BrowserEvaluateDataModel model)
        {
            if (!_views.TryGetValue(model.Id, out var view)) return null;
            return await view.EvaluateAsync(model.Invoke);
        }

        public async Task NavigateAsync(BrowserNavigateDataModel model)
        {
            if (!_views.TryGetValue(model.Id, out var view)) return;
            await view.NavigateAsync(model.Url);
        }

        public async Task<string> ResponseAsync(BrowserResponseDataModel model)
        {
            if (!_views.TryGetValue(model.Id, out var view)) return null;
            var buffer = await view.ResponseAsync(model.Url);
            return Convert.ToBase64String(buffer);
        }

        public async Task WaitForNavigateAsync(BrowserDataModel model)
        {
            if (!_views.TryGetValue(model.Id, out var view)) return;
            await view.WaitForNavigateAsync();
        }

        #endregion
    }
}