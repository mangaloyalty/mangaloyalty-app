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
        private readonly TaskCompletionSource<ServerCoreBinder> _binder;

        #region Constructor

        private ServerCoreConnection()
        {
            _binder = new TimeoutTaskCompletionSource<ServerCoreBinder>();
        }

        public static ServerCoreConnection Create(Context context)
        {
            var connection = new ServerCoreConnection();
            context.BindService(new Intent(context, typeof(ServerService)), connection, Bind.None);
            return connection;
        }

        #endregion

        #region Implementation of IServerCore

        public async Task ListenAsync(IServerCoreListener listener)
        {
            var binder = await _binder.Task;
            await binder.ListenAsync(listener);
        }

        public async Task<JToken> RequestAsync(JToken model)
        {
            var binder = await _binder.Task;
            return await binder.RequestAsync(model);
        }

        #endregion

        #region Implementation of IServiceConnection

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            _binder.TrySetResult((ServerCoreBinder) service);
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            throw new Exception();
        }

        #endregion
    }
}