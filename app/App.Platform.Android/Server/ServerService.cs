using Android.App;
using Android.Content;
using Android.OS;
using App.Platform.Android.Server.Interfaces;
using App.Platform.Android.Shared;
using App.Platform.Android.Utilities;

namespace App.Platform.Android.Server
{
    [Service]
    public class ServerService : Service
    {
        private Controller _controller;
        private IServerCore _core;
        private bool _isActive;

        #region Abstracts

        private void BindChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;
            var channel = new NotificationChannel(Constants.ChannelServiceKey, Constants.ChannelServiceName, NotificationImportance.Default);
            channel.SetSound(null, null);
            channel.SetShowBadge(false);
            NotificationManager.FromContext(this).CreateNotificationChannel(channel);
        }

        private void BindForeground()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;
            StartForeground(Constants.NotificationId, new Notification.Builder(this, Constants.ChannelServiceKey)
                .SetContentText(Constants.NotificationActive)
                .SetSmallIcon(ApplicationInfo.Icon)
                .Build());
        }
        
        private void UnbindForeground()
        {
            NotificationManager.FromContext(this).Notify(Constants.NotificationId, new Notification.Builder(this, Constants.ChannelServiceKey)
                .SetContentText(Constants.NotificationKilled)
                .SetSmallIcon(ApplicationInfo.Icon)
                .Build());
        }

        #endregion

        #region Statics

        public static void StartService(Context context)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                context.StartForegroundService(new Intent(context, typeof(ServerService)));
            }
            else
            {
                context.StartService(new Intent(context, typeof(ServerService)));
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
            _controller = new Controller(this);
            _core = new ServerCore(_controller);
        }

        public override void OnDestroy()
        {
            if (!_isActive) return;
            _isActive = false;
            UnbindForeground();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (_isActive) return StartCommandResult.Sticky;
            _isActive = true;
            BindChannel();
            BindForeground();
            return StartCommandResult.Sticky;
        }

        #endregion
    }
}