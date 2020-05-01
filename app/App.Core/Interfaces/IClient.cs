using App.Core.Models;

namespace App.Core.Interfaces
{
    public interface IClient
    {
        void Submit(string functionName, SubmitDataModel model);
    }
}