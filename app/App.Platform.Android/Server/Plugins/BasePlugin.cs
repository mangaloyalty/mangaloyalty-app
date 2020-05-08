using System.Threading.Tasks;
using Android.Content;
using App.Core.Server;

namespace App.Platform.Android.Server.Plugins
{
    public class BasePlugin : IBasePlugin
    {
        #region Constructor

        public BasePlugin(Context context, ServerCore core, TaskCompletionSource<bool> initTcs)
        {
            Browser = new BrowserPlugin(context, core, initTcs);
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