using System;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;
using App.Core;
using App.Platform.Android.Client.Plugins;
using App.Platform.Android.Server.Interfaces;
using Java.Interop;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Client
{
    public class ClientCore : Java.Lang.Object, IServerCoreListener, IServiceConnection
    {
        private readonly Bridge _bridge;
        private IServerCore _server;

        #region Constructor

        private void Initialize(WebView view)
        {
            view.AddJavascriptInterface(this, "onix");
            view.OverScrollMode = OverScrollMode.Never;
            view.HorizontalScrollBarEnabled = false;
            view.VerticalScrollBarEnabled = false;
            view.Settings.DomStorageEnabled = true;
            view.Settings.JavaScriptEnabled = true;
            view.LoadUrl("file:///android_asset/client.html");
        }

        public ClientCore(Activity activity, WebView view)
        {
            _bridge = new Bridge(new Callback(activity, view), new BasePlugin(activity, this));
            Initialize(view);
        }

        #endregion

        #region Methods

        public async Task<JToken> ForwardAsync(JToken model)
        {
            var responseTask = _server?.RequestAsync(model);
            if (responseTask == null) throw new Exception();
            return await responseTask;
        }

        [Export("fromJs")]
        [JavascriptInterface]
        public void FromJavascript(string json)
        {
            _ = _bridge.RequestAsync(json);
        }

        public void OnBackButton()
        {
            _ = _bridge.EventAsync("backbutton");
        }

        #endregion

        #region Implementation of IServerCoreListener

        public async Task SocketAsync(JToken model)
        {
            await _bridge.EventAsync("socket", model);
        }

        #endregion

        #region Implementation of IServiceConnection

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            _server = service as IServerCore;
            _server?.ListenAsync(this).ContinueWith(t => SocketAsync(new JObject(new JProperty("type", "SocketConnect"))));
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            _server = null;
        }

        #endregion
    }
}