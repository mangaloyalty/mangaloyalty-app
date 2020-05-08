using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace App.Core.Client
{
    public interface IProxyPlugin
    {
        Task<JToken> ForwardAsync(JToken model);
    }
}