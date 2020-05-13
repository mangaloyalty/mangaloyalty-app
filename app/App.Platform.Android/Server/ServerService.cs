using Android.App;
using Android.Content;
using Android.OS;
using Android.Webkit;
using App.Platform.Android.Server.Receivers;

namespace App.Platform.Android.Server
{
    [Service]
    public class ServerService : Service
    {
        private NotificationChannel _channel;
        private ServerCore _core;
        private WebView _view;

        #region Abstracts

        private void BindChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;
            _channel = new NotificationChannel("ServiceChannel", "Service", NotificationImportance.Default);
            _channel.SetSound(null, null);
            _channel.SetShowBadge(false);
            NotificationManager.FromContext(this).CreateNotificationChannel(_channel);
        }

        private void BindForeground()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;
            StartForeground(1, new Notification.Builder(this, "ServiceChannel")
                .SetContentIntent(PendingIntent.GetBroadcast(this, 0, new Intent(this, typeof(NotificationReceiver)), 0))
                .SetContentText("The service is active.")
                .SetSmallIcon(ApplicationInfo.Icon)
                .Build());
        }
        
        private void UnbindForeground()
        {
            NotificationManager.FromContext(this).Notify(1, new Notification.Builder(this, "ServiceChannel")
                .SetContentIntent(PendingIntent.GetBroadcast(this, 0, new Intent(this, typeof(NotificationReceiver)), 0))
                .SetContentText("The service has been killed.")
                .SetSmallIcon(ApplicationInfo.Icon)
                .Build());
        }

        #endregion

        #region Statics

        public static ServerCoreConnection StartService(Context context)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                context.StartForegroundService(new Intent(context, typeof(ServerService)));
                return ServerCoreConnection.Create(context);
            }
            else
            {
                context.StartService(new Intent(context, typeof(ServerService)));
                return ServerCoreConnection.Create(context);
            }
        }

        #endregion

        #region Overrides of Service

        public override IBinder OnBind(Intent intent)
        {
            return new ServerCoreBinder(_core);
        }

        public override void OnCreate()
        {
            _view = new WebView(this);
            _core = new ServerCore(this, _view);
        }

        public override void OnDestroy()
        {
            UnbindForeground();
            _view?.Destroy();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (_channel != null) return StartCommandResult.Sticky;
            BindChannel();
            BindForeground();
            return StartCommandResult.Sticky;
        }

        #endregion
    }
}