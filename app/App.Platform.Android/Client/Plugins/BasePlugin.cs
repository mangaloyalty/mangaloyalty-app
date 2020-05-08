using App.Core.Client;
using App.Platform.Android.Server.Interfaces;

namespace App.Platform.Android.Client.Plugins
{
    public class BasePlugin : IBasePlugin
    {
        #region Constructor

        public BasePlugin(Activity activity, IServerCore server)
        {
            Proxy = new ProxyPlugin(server);
            Shell = new ShellPlugin(activity);
        }

        #endregion

        #region Implementation of ICorePlugin

        public IProxyPlugin Proxy { get; }
        public IShellPlugin Shell { get; }

        #endregion
    }
}