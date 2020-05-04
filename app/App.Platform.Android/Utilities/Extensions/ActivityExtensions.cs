using System;
using System.Threading.Tasks;
using Android.App;

namespace App.Platform.Android.Utilities.Extensions
{
    public static class ActivityExtensions
    {
        #region Abstracts

        private static void Run<T>(Func<T> handler, TaskCompletionSource<T> tcs)
        {
            try
            {
                tcs.TrySetResult(handler());
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }

        #endregion

        #region Statics

        public static async Task InvokeAsync(this Activity activity, Action handler)
        {
            await activity.InvokeAsync(() =>
            {
                handler();
                return true;
            });
        }

        public static async Task<T> InvokeAsync<T>(this Activity activity, Func<T> handler)
        {
            var tcs = new TaskCompletionSource<T>();
            activity.RunOnUiThread(() => Run(handler, tcs));
            return await tcs.Task;
        }

        #endregion
    }
}