using App.Core.Server;
using App.Platform.Android.Server.Interfaces;
using App.Platform.Android.Shared;

namespace App.Platform.Android.Server.Plugins
{
    public class BasePlugin : IBasePlugin
    {
        #region Constructor

        public BasePlugin(Controller controller, IServerCore core)
        {
            Browser = new BrowserPlugin(controller, core);
            Resource = new ResourcePlugin();
            Trace = new TracePlugin();
        }

        #endregion

        #region Implementation of ICorePlugin

        public IBrowserPlugin Browser { get; }
        public IResourcePlugin Resource { get; }
        public ITracePlugin Trace { get; }

        #endregion
    }
}