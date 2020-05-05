using System.Threading.Tasks;
using App.Core.Client.Models;
using Newtonsoft.Json.Linq;

namespace App.Core.Client
{
    public interface IProxyPlugin
    {
        Task<JToken> ForwardAsync(ProxyDataModel model);
    }
}