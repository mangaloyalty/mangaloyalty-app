using Android.Webkit;
using App.Core;
using App.Platform.Android.Server.Plugins;
using App.Platform.Android.Shared;
using Java.Interop;

namespace App.Platform.Android.Server
{
    public class ServerCore : Java.Lang.Object
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
            _viewBridge = new Bridge(new ViewCallback(controller, _view), new BasePlugin(controller));
            Initialize();
        }

        #endregion

        #region Methods

        [Export("request")]
        [JavascriptInterface]
        public void ProcessRequest(string json)
        {
            _ = _viewBridge.ProcessRequestAsync(json);
        }

        #endregion
    }
}