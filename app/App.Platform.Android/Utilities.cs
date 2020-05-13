using System;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using App.Core.Shared;

namespace App.Platform.Android
{
    public static class Utilities
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

        public static async Task RunAsync(this Context context, Action action)
        {
            await context.RunAsync(() =>
            {
                action();
                return true;
            });
        }

        public static async Task<T> RunAsync<T>(this Context context, Func<T> func)
        {
            using var handler = new Handler(context.MainLooper);
            var tcs = new TimeoutTaskCompletionSource<T>();
            handler.Post(() => Run(func, tcs));
            return await tcs.Task;
        }

        #endregion
    }
}