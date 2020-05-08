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
    // TODO: Service runs on same main thread as activity. Bad when scraping+reading = shit perf
    // TODO: Upgrade from Play Store -> restart service.
    // TODO: Screen orientation change support.
    [Activity(Label = "@string/app_name", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]
    public class Activity : global::Android.App.Activity
    {
        private ClientCore _core;

        #region Abstracts

        private async Task CreateAsync()
        {
            var server = ServerService.StartService(this);
            var view = FindViewById<WebView>(Resource.Id.webView);
            _core = await ClientCore.CreateAsync(this, server, view);
        }

        #endregion

        #region Overrides of Activity

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            _ = CreateAsync();
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode != Keycode.Back) return base.OnKeyDown(keyCode, e);
            _core.NavigateBack();
            return true;
        }

        #endregion
    }
}