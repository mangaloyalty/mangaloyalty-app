using App.Core.Plugins;
using App.Platform.iOS.Clients;

namespace App.Platform.iOS.Plugins
{
    public sealed class CorePlugin : ICorePlugin
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