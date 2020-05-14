using Android.Views;
using App.Core.Client;

namespace App.Platform.Android.Client.Plugins
{
    public class ShellPlugin : IShellPlugin
    {
        private readonly Activity _activity;

        #region Constructor

        public ShellPlugin(Activity activity)
        {
            _activity = activity;
        }

        #endregion

        #region Implementation of IShellPlugin

        public void HideSplashScreen()
        {
            _activity.RunOnUiThread(() => _activity.FindViewById(Resource.Id.splashScreen).Visibility = ViewStates.Gone);
        }

        public void MinimizeApp()
        {
            _activity.MoveTaskToBack(true);
        }

        #endregion
    }
}