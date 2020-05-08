using System.Reflection;
using System.Threading.Tasks;

namespace App.Core.Shared.Extensions
{
    public static class TaskExtensions
    {
        public static object ToJson(this Task task)
        {
            var type = task.GetType();
            if (type.IsGenericType) return type.GetRuntimeProperty(nameof(Task<object>.Result)).GetValue(task).ToJson();
            return null;
        }
    }
}