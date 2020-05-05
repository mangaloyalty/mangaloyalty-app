using App.Core.Client;
using App.Core.Server;
using App.Platform.iOS.Clients;
using IBasePlugin = App.Core.Client.IBasePlugin;

namespace App.Platform.iOS.Plugins
{
    public class CorePlugin : IBasePlugin
    {
        #region Constructor

        public CorePlugin(ViewClient view)
        {
            Shell = new ShellPlugin(view);
        }

        #endregion

        #region Implementation of ICorePlugin
        
        public IShellPlugin Shell { get; }

        #endregion
    }
}