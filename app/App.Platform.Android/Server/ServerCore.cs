using System.Threading.Tasks;
using Android.Webkit;
using App.Core;
using App.Core.Shared;
using App.Platform.Android.Server.Interfaces;
using App.Platform.Android.Server.Plugins;
using Java.Interop;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server
{
    public class ServerCore : Java.Lang.Object, IServerCore
    {
        private readonly TaskCompletionSource<bool> _bootTcs;
        private readonly WebView _view;
        private readonly Bridge _viewBridge;
        private IServerCoreListener _listener;

        #region Abstracts

        private void Initialize()
        {
            _view.AddJavascriptInterface(this, "onix");
            _view.Settings.JavaScriptEnabled = true;
            _view.LoadUrl("file:///android_asset/server.html");
        }

        #endregion

        #region Constructor

        public ServerCore(Controller controller)
        {
            _bootTcs = new TimeoutTaskCompletionSource<bool>();
            _view = new WebView(controller);
            _viewBridge = new Bridge(new Callback(controller, _view), new BasePlugin(_bootTcs, controller, this));
            Initialize();
        }

        #endregion

        #region Methods

        public async Task EmitAsync(JToken model)
        {
            var task = _listener?.SocketAsync(model);
            if (task != null) await task;
        }

        public async Task<JToken> EventAsync(string key, object value)
        {
            return await _viewBridge.EventAsync(key, value);
        }

        [Export("fromJs")]
        [JavascriptInterface]
        public void FromJavascript(string json)
        {
            _ = _viewBridge.RequestAsync(json);
        }

        #endregion

        #region Implementation of IServerCore

        public Task ListenAsync(IServerCoreListener listener)
        {
            _listener = listener;
            return Task.CompletedTask;
        }

        public async Task<JToken> RequestAsync(JToken model)
        {
            await _bootTcs.Task;
            return await _viewBridge.EventAsync("request", model);
        }

        #endregion
    }
}