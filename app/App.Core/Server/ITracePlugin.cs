using System.Threading.Tasks;
using App.Core.Server.Models;

namespace App.Core.Server
{
    public interface ITracePlugin
    {
        Task InfoAsync(TraceDataModel model);
    }
}