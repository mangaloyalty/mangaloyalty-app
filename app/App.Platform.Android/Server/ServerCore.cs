using System.Threading.Tasks;
using Android.Content;
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
        private readonly Callback _callback;
        private readonly TaskCompletionSource<bool> _initTcs;
        private readonly Bridge _viewBridge;
        private IServerCoreListener _listener;

        #region Abstracts

        private void Initialize(WebView view)
        {
            view.AddJavascriptInterface(this, "onix");
            view.Settings.JavaScriptEnabled = true;
            view.LoadUrl("file:///android_asset/server.html");
        }

        #endregion

        #region Constructor

        public ServerCore(Context context, WebView view)
        {
            _callback = new Callback(context, view);
            _initTcs = new TimeoutTaskCompletionSource<bool>();
            _viewBridge = new Bridge(_callback, new BasePlugin(context, this, _initTcs));
            Initialize(view);
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
            await _initTcs.Task;
            return await _viewBridge.EventAsync("request", model);
        }

        #endregion

        #region Overrides of Object

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            _callback.Dispose();
        }

        #endregion
    }
}