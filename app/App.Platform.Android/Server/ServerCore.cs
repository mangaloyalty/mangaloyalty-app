using System.Threading.Tasks;
using Android.Content;
using Android.Webkit;
using App.Core;
using App.Core.Shared;
using App.Platform.Android.Server.Plugins;
using Java.Interop;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server
{
    public class ServerCore : Java.Lang.Object
    {
        private readonly TaskCompletionSource<bool> _initTcs;
        private readonly Bridge _viewBridge;

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
            _initTcs = new TimeoutTaskCompletionSource<bool>();
            _viewBridge = new Bridge(new Callback(context, view), new BasePlugin(context, _initTcs));
            Initialize(view);
        }

        #endregion

        #region Methods
        
        [Export("fromJs")]
        [JavascriptInterface]
        public void FromJavascript(string json)
        {
            _ = _viewBridge.RequestAsync(json);
        }

        public async Task<JToken> RequestAsync(JToken model)
        {
            await _initTcs.Task;
            return await _viewBridge.EventAsync("request", model);
        }

        #endregion
    }
}