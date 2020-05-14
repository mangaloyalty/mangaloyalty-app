using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using App.Platform.Android.Client;
using App.Platform.Android.Server;

namespace App.Platform.Android
{
    [Activity(Label = "@string/app_name", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class Activity : global::Android.App.Activity
    {
        private ClientCore _client;
        
        #region Overrides of Activity

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Initialize the server.
            base.OnCreate(savedInstanceState);
            ServerService.StartService(this);

            // Initialize the client.
            SetContentView(Resource.Layout.Main);
            _client = new ClientCore(this, FindViewById<WebView>(Resource.Id.webView));
        }
        
        protected override void OnStart()
        {
            base.OnStart();
            _client.OnStart();
        }

        protected override void OnStop()
        {
            base.OnStop();
            _client.OnStop();
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode != Keycode.Back) return base.OnKeyDown(keyCode, e);
            _client.OnBackButton();
            return true;
        }

        #endregion
    }
}