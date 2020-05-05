using System;
using System.Threading.Tasks;
using App.Core.Server;
using App.Core.Server.Models;

namespace App.Platform.Android.Server.Plugins
{
    // TODO: Write the trace information to an accessible file.
    public class TracePlugin : ITracePlugin
    {
        #region Implementation of ITracePlugin

        public Task InfoAsync(TraceDataModel model)
        {
            Console.WriteLine(model.Message);
            return Task.CompletedTask;
        }

        #endregion
    }
}