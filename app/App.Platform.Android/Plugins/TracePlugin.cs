using System;
using System.Threading.Tasks;
using App.Core.Models.Plugins;
using App.Core.Plugins;

namespace App.Platform.Android.Plugins
{
    public sealed class TracePlugin : ITracePlugin
    {
        public Task InfoAsync(TraceDataModel model)
        {
            Console.WriteLine(model.Message);
            return Task.CompletedTask;
        }
    }
}