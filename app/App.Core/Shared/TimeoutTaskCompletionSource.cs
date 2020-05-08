using System;
using System.Threading.Tasks;
using Tasks = System.Threading.Tasks;

namespace App.Core.Shared
{
    public class TimeoutTaskCompletionSource<T> : TaskCompletionSource<T>
    {
        public TimeoutTaskCompletionSource(int timeoutInSeconds = 30)
        {
            Tasks.Task.Delay(TimeSpan.FromSeconds(timeoutInSeconds)).ContinueWith(t => TrySetCanceled());
        }
    }
}