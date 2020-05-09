using Android.App;
using Android.Content;

namespace App.Platform.Android.Server.Receivers
{
    [BroadcastReceiver]
    [IntentFilter(new[] {Intent.ActionMyPackageReplaced})]
    public sealed class MyPackageReplacedReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            ServerService.StartService(context);
        }
    }
}