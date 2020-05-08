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
    // TODO: Automations on unmetered?
    public class ClientCore : Java.Lang.Object
    {
        private readonly Bridge _bridge;
        private readonly WebView _view;

        #region Constructor

        private void Initialize()
        {
            _view.AddJavascriptInterface(this, "onix");
            _view.OverScrollMode = OverScrollMode.Never;
            _view.HorizontalScrollBarEnabled = false;
            _view.VerticalScrollBarEnabled = false;
            _view.Settings.DomStorageEnabled = true;
            _view.Settings.JavaScriptEnabled = true;
            _view.LoadUrl("file:///android_asset/client.html");
        }

        private ClientCore(Activity activity, IServerCore server, WebView view)
        {
            var controller = new Controller(activity); // TODO: Weird use of controller in client.
            _bridge = new Bridge(new Callback(controller, view), new BasePlugin(activity, server));
            _view = view;
            Initialize();
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
            _ = _bridge?.RequestAsync(json);
        }

        public void NavigateBack()
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