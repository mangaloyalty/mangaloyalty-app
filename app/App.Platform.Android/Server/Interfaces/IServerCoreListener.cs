using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Interfaces
{
    public interface IServerCoreListener
    {
        Task SocketAsync(JToken model);
    }
}