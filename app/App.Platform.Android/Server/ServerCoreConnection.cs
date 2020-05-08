using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using App.Platform.Android.Server.Interfaces;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server
{
    public class ServerCoreConnection : Java.Lang.Object, IServerCore, IServiceConnection
    {
        private TaskCompletionSource<ServerCoreBinder> _binder;

        #region Constructor

        private ServerCoreConnection()
        {
            _binder = new TaskCompletionSource<ServerCoreBinder>();
        }

        public static ServerCoreConnection Create(Context context)
        {
            var connection = new ServerCoreConnection();
            context.BindService(new Intent(context, typeof(ServerService)), connection, Bind.None);
            return connection;
        }

        #endregion

        #region Implementation of IServerCore

        public async Task<JToken> EventAsync(string key, object value)
        {
            var binder = await _binder.Task;
            return await binder.EventAsync(key, value);
        }

        public async Task<JToken> RequestAsync(string key, JToken value)
        {
            var binder = await _binder.Task;
            return await binder.RequestAsync(key, value);
        }

        #endregion

        #region Implementation of IServiceConnection

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            _binder.TrySetResult((ServerCoreBinder) service);
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            _binder = new TaskCompletionSource<ServerCoreBinder>();
        }

        #endregion
    }
}