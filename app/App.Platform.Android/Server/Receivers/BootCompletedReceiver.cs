using Android.App;
using Android.Content;

namespace App.Platform.Android.Server.Receivers
{
    [BroadcastReceiver]
    [IntentFilter(new[] {Intent.ActionBootCompleted})]
    public sealed class BootCompletedReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            ServerService.StartService(context);
        }
    }
}