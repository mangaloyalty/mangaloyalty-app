using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;
using App.Core;
using App.Platform.Android.Client.Plugins;
using App.Platform.Android.Server;
using App.Platform.Android.Server.Interfaces;
using Java.Interop;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Client
{
    public class ClientCore : Java.Lang.Object, IServerCoreListener, IServiceConnection
    {
        private readonly Activity _activity;
        private readonly Bridge _bridge;
        private TaskCompletionSource<IServerCore> _connectionTcs;

        #region Constructor

        private void Initialize(WebView view)
        {
            view.AddJavascriptInterface(this, "onix");
            view.OverScrollMode = OverScrollMode.Never;
            view.HorizontalScrollBarEnabled = false;
            view.VerticalScrollBarEnabled = false;
            view.Settings.DomStorageEnabled = true;
            view.Settings.JavaScriptEnabled = true;
            view.LoadUrl("file:///android_asset/client.html");
        }

        public ClientCore(Activity activity, WebView view)
        {
            _activity = activity;
            _bridge = new Bridge(new Callback(activity, view), new BasePlugin(activity, this));
            _connectionTcs = new TaskCompletionSource<IServerCore>();
            Initialize(view);
        }

        #endregion

        #region Methods

        public async Task<JToken> ForwardAsync(JToken model)
        {
            var server = await _connectionTcs.Task;
            var response = await server.RequestAsync(model);
            return response;
        }

        [Export("fromJs")]
        [JavascriptInterface]
        public void FromJavascript(string json)
        {
            _ = _bridge.RequestAsync(json);
        }

        public void OnBackButton()
        {
            _ = _bridge.EventAsync("backbutton");
        }

        public void OnStart()
        {
            _activity.BindService(new Intent(_activity, typeof(ServerService)), this, Bind.None);
        }

        public void OnStop()
        {
            _connectionTcs.TrySetCanceled();
            _connectionTcs = new TaskCompletionSource<IServerCore>();
            _activity.UnbindService(this);
        }

        #endregion

        #region Implementation of IServerCoreListener

        public async Task SocketAsync(JToken model)
        {
            await _bridge.EventAsync("socket", model);
        }

        #endregion

        #region Implementation of IServiceConnection

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            if (!(service is IServerCore server)) return;
            _connectionTcs.TrySetResult(server);
            server.ListenAsync(this).ContinueWith(t => SocketAsync(new JObject(new JProperty("type", "SocketConnect"))));
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            _connectionTcs.TrySetCanceled();
            _connectionTcs = new TaskCompletionSource<IServerCore>();
        }

        #endregion
    }
}