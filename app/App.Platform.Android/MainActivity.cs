using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using App.Core;
using App.Platform.Android.Clients;
using App.Platform.Android.Plugins;

namespace App.Platform.Android
{
    [Activity(Label = "@string/app_name", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]
    public sealed class MainActivity : Activity
    {
        private Bridge _bridge;
        private WebView _webView;

        #region Methods

        public void DispatchEvent(string eventName, object value)
        {
            _bridge.DispatchEvent(eventName, value);
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
            _webView = FindViewById<WebView>(Resource.Id.webView);
            _bridge = new Bridge(new BridgeClient(_webView), new CorePlugin(this));

            // Initialize the view.
            _webView.OverScrollMode = OverScrollMode.Never;
            _webView.HorizontalScrollBarEnabled = false;
            _webView.VerticalScrollBarEnabled = false;
            _webView.Settings.DomStorageEnabled = true;
            _webView.Settings.JavaScriptEnabled = true;
            _webView.SetWebChromeClient(new ChromeClient(_bridge));
            _webView.SetWebViewClient(new WebClient(this));
            _webView.LoadUrl("file:///android_asset/index.html");
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode != Keycode.Back) return base.OnKeyDown(keyCode, e);
            _bridge.DispatchEvent("backbutton");
            return true;
        }
        
        protected override void OnPause()
        {
            base.OnPause();
            _bridge.UpdateState(false);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _bridge.UpdateState(true);
        }

        #endregion
    }
}