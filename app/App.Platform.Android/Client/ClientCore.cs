using Android.Views;
using Android.Webkit;
using App.Core;
using App.Platform.Android.Client.Plugins;
using Java.Interop;

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

        public ClientCore(Activity activity, WebView view)
        {
            _bridge = new Bridge(new Callback(activity, view), new BasePlugin(activity));
            Initialize(view);
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

        #endregion
    }
}