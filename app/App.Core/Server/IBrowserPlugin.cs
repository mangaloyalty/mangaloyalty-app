using System.Threading.Tasks;
using App.Core.Server.Models;
using Newtonsoft.Json.Linq;

namespace App.Core.Server
{
    public interface IBrowserPlugin
    {
        Task BootAsync();
        Task<string> CreateAsync();
        Task DestroyAsync(BrowserDataModel model);
        Task<JToken> EvaluateAsync(BrowserEvaluateDataModel model);
        Task NavigateAsync(BrowserNavigateDataModel model);
        Task<string> ResponseAsync(BrowserResponseDataModel model);
        Task WaitForNavigateAsync(BrowserDataModel model);
    }
}