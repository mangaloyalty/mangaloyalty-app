using System.Threading.Tasks;
using Android.OS;
using App.Platform.Android.Server.Interfaces;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server
{
    public class ServerCoreBinder : Binder, IServerCore
    {
        private readonly IServerCore _server;

        #region Constructor

        public ServerCoreBinder(IServerCore server)
        {
            _server = server;
        }

        #endregion

        #region Implementation of IServerCore

        public async Task ListenAsync(IServerCoreListener listener)
        {
            await _server.ListenAsync(listener);
        }

        public async Task<JToken> RequestAsync(JToken value)
        {
            return await _server.RequestAsync(value);
        }

        #endregion
    }
}