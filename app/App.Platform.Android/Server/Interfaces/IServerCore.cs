using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Interfaces
{
    public interface IServerCore
    {
        Task ListenAsync(IServerCoreListener listener);
        Task<JToken> RequestAsync(JToken model);
    }
}