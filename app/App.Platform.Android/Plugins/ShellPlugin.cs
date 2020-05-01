using Android.App;
using Android.Views;
using App.Core.Plugins;

namespace App.Platform.Android.Plugins
{
    public sealed class ShellPlugin : IShellPlugin
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
            var view = _activity.FindViewById(Resource.Id.splashScreen);
            if (view != null) view.Visibility = ViewStates.Gone;
        }

        #endregion
    }
}