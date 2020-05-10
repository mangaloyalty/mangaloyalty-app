using System;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using App.Core.Shared;
using App.Platform.Android.Server.Interfaces;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server
{
    public class ServerCoreConnection : Java.Lang.Object, IServerCore, IServiceConnection
    {
        private readonly TaskCompletionSource<ServerCoreBinder> _binderTcs;
        private readonly Context _context;

        #region Constructor

        private ServerCoreConnection(Context context)
        {
            _binderTcs = new TimeoutTaskCompletionSource<ServerCoreBinder>();
            _context = context;
        }

        public static ServerCoreConnection Create(Context context)
        {
            var connection = new ServerCoreConnection(context);
            context.BindService(new Intent(context, typeof(ServerService)), connection, Bind.None);
            return connection;
        }

        #endregion

        #region Implementation of IServerCore

        public async Task ListenAsync(IServerCoreListener listener)
        {
            var binder = await _binderTcs.Task;
            await binder.ListenAsync(listener);
        }

        public async Task<JToken> RequestAsync(JToken model)
        {
            var binder = await _binderTcs.Task;
            return await binder.RequestAsync(model);
        }

        #endregion

        #region Implementation of IServiceConnection

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            _binderTcs.TrySetResult((ServerCoreBinder) service);
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            throw new Exception();
        }

        #endregion

        #region Overrides of Object

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            _context.UnbindService(this);
        }

        #endregion
    }
}