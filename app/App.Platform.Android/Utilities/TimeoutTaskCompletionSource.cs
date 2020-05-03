using System;
using System.Threading.Tasks;
using Tasks = System.Threading.Tasks;

namespace App.Platform.Android.Utilities
{
    public class TimeoutTaskCompletionSource<T> : TaskCompletionSource<T>
    {
        public TimeoutTaskCompletionSource(int timeoutInSeconds)
        {
            Tasks.Task.Delay(TimeSpan.FromSeconds(timeoutInSeconds)).ContinueWith(t => TrySetCanceled());
        }
    }
}