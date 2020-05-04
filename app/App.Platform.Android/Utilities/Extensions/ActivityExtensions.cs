using System;
using System.Threading.Tasks;
using Android.App;

namespace App.Platform.Android.Utilities.Extensions
{
    public static class ActivityExtensions
    {
        public static async Task RunOnUiThreadAsync(this Activity activity, Func<Task> handler)
        {
            var tcs = new TaskCompletionSource<bool>();
            activity.RunOnUiThread(() => handler().ContinueWith(t => t.IsFaulted
                ? tcs.TrySetException(t.Exception ?? new Exception())
                : tcs.TrySetResult(true)));
            await tcs.Task;
        }

        public static async Task<T> RunOnUiThreadAsync<T>(this Activity activity, Func<Task<T>> handler)
        {
            var tcs = new TaskCompletionSource<T>();
            activity.RunOnUiThread(() => handler().ContinueWith(t => t.IsFaulted
                ? tcs.TrySetException(t.Exception ?? new Exception())
                : tcs.TrySetResult(t.Result)));
            return await tcs.Task;
        }
    }
}