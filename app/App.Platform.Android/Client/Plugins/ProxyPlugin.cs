using System.Threading.Tasks;
using App.Core.Client;
using App.Platform.Android.Server.Interfaces;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Client.Plugins
{
    public class ProxyPlugin : IProxyPlugin
    {
        private readonly IServerCore _server;

        #region Constructor

        public ProxyPlugin(IServerCore server)
        {
            _server = server;
        }

        #endregion

        #region Implementation of IProxyPlugin

        public async Task<JToken> ForwardAsync(JToken model)
        {
            return await _server.RequestAsync(model);
        }

        #endregion
    }
}