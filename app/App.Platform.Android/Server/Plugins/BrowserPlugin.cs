using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Server;
using App.Core.Server.Models;
using App.Platform.Android.Server.Interfaces;
using App.Platform.Android.Server.Plugins.Browser;
using App.Platform.Android.Shared;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Plugins
{
    public class BrowserPlugin : IBrowserPlugin
    {
        private readonly Controller _controller;
        private readonly IServerCore _core;
        private readonly ConcurrentDictionary<string, BrowserView> _views;
        private int _previousId;

        #region Constructor

        public BrowserPlugin(Controller controller, IServerCore core)
        {
            _controller = controller;
            _core = core;
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
            var view = await BrowserView.CreateAsync(_controller, _core, viewId);
            if (_views.TryAdd(viewId, view)) return viewId;
            await view.DestroyAsync();
            throw new Exception();
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