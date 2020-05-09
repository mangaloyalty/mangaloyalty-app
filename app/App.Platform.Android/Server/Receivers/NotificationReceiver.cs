using Android.Content;

namespace App.Platform.Android.Server.Receivers
{
    [BroadcastReceiver]
    public class NotificationReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            context.StartActivity(context.PackageManager
                .GetLaunchIntentForPackage(context.PackageName)
                .SetFlags(ActivityFlags.BroughtToFront | ActivityFlags.ReorderToFront | ActivityFlags.NewTask));
        }
    }
}