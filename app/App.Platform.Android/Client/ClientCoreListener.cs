using System.Threading.Tasks;
using App.Platform.Android.Server.Interfaces;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Client
{
    public class ClientCoreListener : IServerCoreListener
    {
        private readonly ClientCore _core;

        #region Constructor

        public ClientCoreListener(ClientCore core)
        {
            _core = core;
        }

        #endregion

        #region Implementation of IServerCoreListener

        public async Task SocketAsync(JToken model)
        {
            await _core.SocketAsync(model);
        }

        #endregion
    }
}