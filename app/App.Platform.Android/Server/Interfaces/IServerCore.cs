using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Interfaces
{
    public interface IServerCore
    {
        Task<JToken> EventAsync(string key, object value);
        Task<JToken> RequestAsync(string key, JToken value);
    }
}