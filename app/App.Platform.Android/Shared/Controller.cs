using System;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;

namespace App.Platform.Android.Shared
{
    public class Controller
    {
        private readonly Context _context;
        private readonly Handler _handler;

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

        #region Constructor

        public Controller(Context context)
        {
            _context = context;
            _handler = new Handler();
        }

        #endregion

        #region Methods

        public async Task RunAsync(Action action)
        {
            await RunAsync(() =>
            {
                action();
                return true;
            });
        }

        public async Task<T> RunAsync<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            _handler.Post(() => Run(func, tcs));
            return await tcs.Task;
        }

        #endregion

        #region Statics

        public static implicit operator Context(Controller controller)
        {
            return controller._context;
        }

        #endregion
    }
}