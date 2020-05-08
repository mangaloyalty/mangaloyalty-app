using System.Threading.Tasks;
using Android.Webkit;
using App.Core;
using App.Platform.Android.Server.Interfaces;
using App.Platform.Android.Server.Plugins;
using App.Platform.Android.Shared;
using Java.Interop;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server
{
    public class ServerCore : Java.Lang.Object, IServerCore
    {
        private readonly WebView _view;
        private readonly Bridge _viewBridge;

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
            _view = new WebView(controller);
            _viewBridge = new Bridge(new ViewCallback(controller, _view), new BasePlugin(controller, this));
            Initialize();
        }

        #endregion

        #region Methods

        [Export("fromJs")]
        [JavascriptInterface]
        public void FromJavascript(string json)
        {
            _ = _viewBridge.RequestAsync(json);
        }

        #endregion

        #region Implementation of IServerCore

        public async Task<JToken> EventAsync(string key, object value)
        {
            return await _viewBridge.EventAsync(key, value);
        }

        public async Task<JToken> RequestAsync(string key, JToken value)
        {
            return await _viewBridge.EventAsync("request", new { key, value });
        }

        #endregion
    }
}