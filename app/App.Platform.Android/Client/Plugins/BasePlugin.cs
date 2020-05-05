using Android.App;
using App.Core.Client;

namespace App.Platform.Android.Client.Plugins
{
    public class BasePlugin : IBasePlugin
    {
        #region Constructor

        public BasePlugin(Activity activity)
        {
            Proxy = new ProxyPlugin(activity);
            Shell = new ShellPlugin(activity);
        }

        #endregion

        #region Implementation of ICorePlugin

        public IProxyPlugin Proxy { get; }
        public IShellPlugin Shell { get; }

        #endregion
    }
}