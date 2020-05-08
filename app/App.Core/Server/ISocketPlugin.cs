using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace App.Core.Server
{
    public interface ISocketPlugin
    {
        Task EmitAsync(JToken model);
    }
}