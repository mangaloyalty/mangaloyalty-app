using System.Threading.Tasks;
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
    // TODO: Cleanup client code, callback/activity/utilities code..
    // TODO: Service connection can be lost. Reconnect and fire * action event.

    // TODO: Base64'ing on different thread to avoid locking the UI thread?
    // TODO: Batch operations are horrendously slow... consider batch APIs.
    // TODO: On activity kill, service is also killed.
    [Activity(Label = "@string/app_name", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class Activity : global::Android.App.Activity
    {
        private ClientCore _core;
        private ServerCoreConnection _server;

        #region Abstracts

        private async Task CreateAsync()
        {
            _server = ServerService.StartService(this);
            _core = await ClientCore.CreateAsync(this, _server, FindViewById<WebView>(Resource.Id.webView));
        }

        #endregion

        #region Overrides of Activity

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            _ = CreateAsync(); // TODO: Synchronous please.
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode != Keycode.Back) return base.OnKeyDown(keyCode, e);
            _core.OnBackButton();
            return true;
        }

        #endregion
    }
}