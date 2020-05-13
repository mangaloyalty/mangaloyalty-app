using App.Core.Client;

namespace App.Platform.Android.Client.Plugins
{
    public class BasePlugin : IBasePlugin
    {
        #region Constructor

        public BasePlugin(Activity activity, ClientCore core)
        {
            Proxy = new ProxyPlugin(core);
            Shell = new ShellPlugin(activity);
        }

        #endregion

        #region Implementation of ICorePlugin

        public IProxyPlugin Proxy { get; }
        public IShellPlugin Shell { get; }

        #endregion
    }
}