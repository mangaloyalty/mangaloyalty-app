using System.Threading.Tasks;
using App.Core.Client;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Client.Plugins
{
    public class ProxyPlugin : IProxyPlugin
    {
        private readonly ClientCore _core;

        #region Constructor

        public ProxyPlugin(ClientCore core)
        {
            _core = core;
        }

        #endregion

        #region Implementation of IProxyPlugin

        public async Task<JToken> ForwardAsync(JToken model)
        {
            return await _core.ForwardAsync(model);
        }

        #endregion
    }
}