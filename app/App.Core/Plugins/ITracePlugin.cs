using System.Threading.Tasks;
using App.Core.Models.Plugins;

namespace App.Core.Plugins
{
    public interface ITracePlugin
    {
        Task InfoAsync(TraceDataModel model);
    }
}