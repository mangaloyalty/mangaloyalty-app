using App.Core;
using App.Platform.iOS.Clients;
using App.Platform.iOS.Plugins;
using Foundation;
using UIKit;
using WebKit;

namespace App.Platform.iOS
{
    [Register("AppDelegate")]
    public sealed class AppDelegate : UIApplicationDelegate
    {
        private Bridge _bridge;

        #region Overrides of UIApplicationDelegate

        public override void DidEnterBackground(UIApplication application)
        {
            _bridge.UpdateState(false);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Initialize the content controller.
            var bounds = UIScreen.MainScreen.Bounds;
            var contentController = new WKUserContentController();
            var contentMessageHandler = new WebClient();
            contentController.AddScriptMessageHandler(contentMessageHandler, "native");

            // Initialize the content.
            var webView = new WKWebView(bounds, new WKWebViewConfiguration {UserContentController = contentController});
            var view = new ViewClient(webView);
            _bridge = new Bridge(new BridgeClient(webView), new CorePlugin(view));

            // Initialize the content configuration.
            contentMessageHandler.UseBridge(_bridge);
            webView.ScrollView.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
            webView.ScrollView.ShowsHorizontalScrollIndicator = false;
            webView.ScrollView.ShowsVerticalScrollIndicator = false;

            // Initialize the view splash screen.
            var launchScreen = NSBundle.MainBundle.LoadNib("LaunchScreen", null, null);
            var launchView = launchScreen.GetItem<UIView>(0);
            launchView.Frame = bounds;

            // Initialize the window.
            Window = new UIWindow(bounds) {RootViewController = view};
            Window.RootViewController.View = launchView;
            Window.MakeKeyAndVisible();
            return true;
        }

        public override void OnActivated(UIApplication application)
        {
            _bridge.UpdateState(true);
        }
        
        public override UIWindow Window
        {
            get;
            set;
        }

        #endregion
    }
}