using System.Threading.Tasks;
using App.Core.Models.Plugins;
using Newtonsoft.Json.Linq;

namespace App.Core.Plugins
{
    public interface IResourcePlugin
    {
        Task<bool> MoveAsync(ResourceMoveDataModel model);
        Task<string[]> ReaddirAsync(ResourceDataModel model);
        Task<string> ReadFileAsync(ResourceDataModel model);
        Task<JToken> ReadJsonAsync(ResourceDataModel model);
        Task RemoveAsync(ResourceDataModel model);
        Task WriteFileAsync(ResourceWriteFileDataModel model);
        Task WriteJsonAsync(ResourceWriteJsonDataModel model);
    }
}