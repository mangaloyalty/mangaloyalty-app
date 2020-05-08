using System.Threading.Tasks;
using App.Core.Server;

namespace App.Platform.Android.Server.Plugins
{
    public class BasePlugin : IBasePlugin
    {
        #region Constructor

        public BasePlugin(TaskCompletionSource<bool> bootTcs, Controller controller, ServerCore core)
        {
            Browser = new BrowserPlugin(bootTcs, controller, core);
            Resource = new ResourcePlugin();
            Socket = new SocketPlugin(core);
            Trace = new TracePlugin();
        }

        #endregion

        #region Implementation of ICorePlugin

        public IBrowserPlugin Browser { get; }
        public IResourcePlugin Resource { get; }
        public ISocketPlugin Socket { get; }
        public ITracePlugin Trace { get; }

        #endregion
    }
}