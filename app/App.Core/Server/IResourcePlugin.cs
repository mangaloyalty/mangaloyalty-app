using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Server.Models;
using Newtonsoft.Json.Linq;

namespace App.Core.Server
{
    public interface IResourcePlugin
    {
        Task<bool> MoveAsync(ResourceMoveDataModel model);
        Task<IEnumerable<string>> ReaddirAsync(ResourceDataModel model);
        Task<string> ReadFileAsync(ResourceDataModel model);
        Task<JToken> ReadJsonAsync(ResourceDataModel model);
        Task RemoveAsync(ResourceDataModel model);
        Task WriteFileAsync(ResourceWriteFileDataModel model);
        Task WriteJsonAsync(ResourceWriteJsonDataModel model);
    }
}