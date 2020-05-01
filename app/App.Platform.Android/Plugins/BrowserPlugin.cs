using System;
using System.Threading.Tasks;
using App.Core.Models.Plugins;
using App.Core.Plugins;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Plugins
{
    public sealed class BrowserPlugin : IBrowserPlugin
    {
        public Task BootAsync()
        {
            return Task.CompletedTask;
        }

        public Task<string> CreateAsync()
        {
            throw new NotImplementedException();
        }

        public Task DestroyAsync(BrowserDataModel model)
        {
            throw new NotImplementedException();
        }

        public Task<JToken> EvaluateAsync(BrowserEvaluateDataModel model)
        {
            throw new NotImplementedException();
        }

        public Task NavigateAsync(BrowserNavigateDataModel model)
        {
            throw new NotImplementedException();
        }

        public Task<string> ResponseAsync(BrowserResponseDataModel model)
        {
            throw new NotImplementedException();
        }

        public Task WaitForNavigateAsync(BrowserDataModel model)
        {
            throw new NotImplementedException();
        }
    }
}