using System;
using Foundation;
using UIKit;
using WebKit;

namespace App.Platform.iOS.Clients
{
    public class ViewClient : UIViewController, IWKNavigationDelegate, IWKUIDelegate
    {
        private readonly WKWebView _webView;
        private bool _hasAppeared;
        private UIStatusBarStyle _statusBarStyle;

        #region Constructor

        public ViewClient(WKWebView webView)
        {
            _statusBarStyle = UIStatusBarStyle.Default;
            _webView = webView;
        }

        #endregion

        #region Methods

        public void HideSplashScreen()
        {
            _statusBarStyle = UIStatusBarStyle.LightContent;
            _webView.Hidden = false;
            SetNeedsStatusBarAppearanceUpdate();
        }

        #endregion

        #region Overrides of UIViewController

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return _statusBarStyle;
        }

        public override void ViewDidAppear(bool animated)
        {
            if (_hasAppeared) return;
            var fileUrl = new NSUrl(NSBundle.MainBundle.PathForResource("index", "html"), false);
            _hasAppeared = true;
            _webView.Hidden = true;
            _webView.NavigationDelegate = this;
            _webView.UIDelegate = this;
            _webView.LoadFileUrl(fileUrl, fileUrl);
            View.AddSubview(_webView);
        }

        #endregion

        #region Implementation of IWKNavigationDelegate

        [Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
        public void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            if (UIApplication.SharedApplication.CanOpenUrl(navigationAction.Request.Url))
            {
                UIApplication.SharedApplication.OpenUrl(navigationAction.Request.Url);
                decisionHandler(WKNavigationActionPolicy.Cancel);
            }
            else
            {
                decisionHandler(WKNavigationActionPolicy.Allow);
            }
        }

        #endregion

        #region Implementation of IWKUIDelegate

        [Export("webView:runJavaScriptAlertPanelWithMessage:initiatedByFrame:completionHandler:")]
        public void RunJavaScriptAlertPanel(WKWebView webView, string message, WKFrameInfo frame, Action completionHandler)
        {
            var controller = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            controller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, x => completionHandler()));
            PresentViewController(controller, true, null);
        }

        [Export("webView:runJavaScriptConfirmPanelWithMessage:initiatedByFrame:completionHandler:")]
        public void RunJavaScriptConfirmPanel(WKWebView webView, string message, WKFrameInfo frame, Action<bool> completionHandler)
        {
            var controller = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            controller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, x => completionHandler(true)));
            controller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, x => completionHandler(false)));
            PresentViewController(controller, true, null);
        }

        [Export("webView:runJavaScriptTextInputPanelWithPrompt:defaultText:initiatedByFrame:completionHandler:")]
        public void RunJavaScriptTextInputPanel(WKWebView webView, string prompt, string defaultText, WKFrameInfo frame, Action<string> completionHandler)
        {
            var controller = UIAlertController.Create(null, prompt, UIAlertControllerStyle.Alert);
            controller.AddTextField(textField =>
            {
                textField.Placeholder = defaultText;
                controller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, x => completionHandler(textField.Text)));
                controller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, x => completionHandler(null)));
                PresentViewController(controller, true, null);
            });
        }

        #endregion
    }
}