using App.Core.Client;

namespace App.Platform.Android.Client.Plugins
{
    public class BasePlugin : IBasePlugin
    {
        #region Constructor

        public BasePlugin(Activity activity)
        {
            Shell = new ShellPlugin(activity);
        }

        #endregion

        #region Implementation of ICorePlugin

        public IShellPlugin Shell { get; }

        #endregion
    }
}