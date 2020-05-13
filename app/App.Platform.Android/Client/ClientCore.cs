using System.Threading.Tasks;
using Android.Views;
using Android.Webkit;
using App.Core;
using App.Platform.Android.Client.Plugins;
using App.Platform.Android.Server.Interfaces;
using Java.Interop;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Client
{
    public class ClientCore : Java.Lang.Object
    {
        private readonly Bridge _bridge;

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

        private ClientCore(Activity activity, IServerCore server, WebView view)
        {
            _bridge = new Bridge(new Callback(activity, view), new BasePlugin(activity, server));
            Initialize(view);
        }

        public static async Task<ClientCore> CreateAsync(Activity activity, IServerCore server, WebView view)
        {
            var core = new ClientCore(activity, server, view);
            await server.ListenAsync(new ClientCoreListener(core));
            return core;
        }

        #endregion

        #region Methods

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

        public async Task SocketAsync(JToken model)
        {
            await _bridge.EventAsync("socket", model);
        }

        #endregion
    }
}