using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using App.Core;
using App.Platform.Android.Clients;
using App.Platform.Android.Plugins;
using Java.Interop;

namespace App.Platform.Android
{
    [Activity(Label = "@string/app_name", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]
    public sealed class MainActivity : Activity
    {
        private Bridge _bridge;

        #region Events

        [Export("request")]
        [JavascriptInterface]
        public void ReceiveRequest(string json)
        {
            _bridge?.ProcessRequest(json);
        }

        #endregion

        #region Methods

        public void DispatchEvent(string eventName, object value = null)
        {
            _bridge?.DispatchEvent(eventName, value);
        }

        #endregion

        #region Overrides of Activity

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Initialize the component.
            base.OnCreate(savedInstanceState);
            WebView.SetWebContentsDebuggingEnabled(true);

            // Initialize the content.
            SetContentView(Resource.Layout.Main);
            var view = FindViewById<WebView>(Resource.Id.webView);
            _bridge = new Bridge(new ViewClient(view), new CorePlugin(this));
            view.AddJavascriptInterface(this, "onix");

            // Initialize the view.
            view.OverScrollMode = OverScrollMode.Never;
            view.HorizontalScrollBarEnabled = false;
            view.VerticalScrollBarEnabled = false;
            view.Settings.DomStorageEnabled = true;
            view.Settings.JavaScriptEnabled = true;
            view.LoadUrl("file:///android_asset/index.html");
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode != Keycode.Back) return base.OnKeyDown(keyCode, e);
            DispatchEvent("backbutton");
            return true;
        }

        #endregion
    }
}