using System;
using System.Threading.Tasks;
using App.Core.Server;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Plugins
{
    public class SocketPlugin : ISocketPlugin
    {
        private readonly ServerCore _core;

        #region Constructor

        public SocketPlugin(ServerCore core)
        {
            _core = core;
        }

        #endregion

        #region Implementation of ISocketPlugin

        public async Task EmitAsync(JToken model)
        {
            try
            {
                await _core.EmitAsync(model);
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion
    }
}