using System.Threading.Tasks;
using Android.Content;
using App.Core.Server;

namespace App.Platform.Android.Server.Plugins
{
    public class BasePlugin : IBasePlugin
    {
        #region Constructor

        public BasePlugin(Context context, TaskCompletionSource<bool> initTcs)
        {
            Browser = new BrowserPlugin(context, initTcs);
            Resource = new ResourcePlugin(context);
            Trace = new TracePlugin(context);
        }

        #endregion

        #region Implementation of ICorePlugin

        public IBrowserPlugin Browser { get; }
        public IResourcePlugin Resource { get; }
        public ITracePlugin Trace { get; }

        #endregion
    }
}