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
    // TODO: Screen orientation change support.
    [Activity(Label = "@string/app_name", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]
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
            _ = CreateAsync();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _server?.Dispose();
            _core?.Dispose();
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