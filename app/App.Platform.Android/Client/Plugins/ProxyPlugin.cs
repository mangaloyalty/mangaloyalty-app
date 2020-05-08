using System.Threading.Tasks;
using Android.Content;
using App.Core.Client;
using App.Core.Client.Models;
using App.Platform.Android.Server;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Client.Plugins
{
    public class ProxyPlugin : IProxyPlugin
    {
        private readonly ServerCoreConnection _connection;

        #region Constructor

        public ProxyPlugin(Context context)
        {
            _connection = ServerCoreConnection.Create(context);
        }

        #endregion

        #region Implementation of IProxyPlugin

        public async Task<JToken> ForwardAsync(ProxyDataModel model)
        {
            return await _connection.RequestAsync(model.Key, model.Value);
        }

        #endregion
    }
}