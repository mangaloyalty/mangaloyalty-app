using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using App.Core;
using App.Platform.Android.Client.Plugins;
using App.Platform.Android.Server;
using App.Platform.Android.Shared;
using Java.Interop;

namespace App.Platform.Android
{
    [Activity(Label = "@string/app_name", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity
    {
        private Bridge _bridge;

        #region Events

        [Export("fromJs")]
        [JavascriptInterface]
        public void ProcessJavascriptRequest(string json)
        {
            _ = _bridge?.RequestAsync(json);
        }

        #endregion
        
        #region Overrides of Activity

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Initialize the component.
            base.OnCreate(savedInstanceState);
            ServerService.StartService(this);

            // Initialize the content.
            SetContentView(Resource.Layout.Main);
            var controller = new Controller(this);
            var view = FindViewById<WebView>(Resource.Id.webView);
            _bridge = new Bridge(new ViewCallback(controller, view), new BasePlugin(this));
            view.AddJavascriptInterface(this, "onix");

            // Initialize the view.
            view.OverScrollMode = OverScrollMode.Never;
            view.HorizontalScrollBarEnabled = false;
            view.VerticalScrollBarEnabled = false;
            view.Settings.DomStorageEnabled = true;
            view.Settings.JavaScriptEnabled = true;
            view.LoadUrl("file:///android_asset/client.html");
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode != Keycode.Back) return base.OnKeyDown(keyCode, e);
            _ = _bridge.EventAsync("backbutton");
            return true;
        }

        #endregion
    }
}