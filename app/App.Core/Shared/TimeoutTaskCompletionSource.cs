using System;
using System.Threading.Tasks;
using Tasks = System.Threading.Tasks;

namespace App.Core.Shared
{
    public class TimeoutTaskCompletionSource<T> : TaskCompletionSource<T>
    {
        public TimeoutTaskCompletionSource()
        {
            Tasks.Task.Delay(TimeSpan.FromSeconds(30)).ContinueWith(t => TrySetCanceled());
        }
    }
}