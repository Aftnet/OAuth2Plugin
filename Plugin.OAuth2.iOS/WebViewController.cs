using CoreGraphics;
using UIKit;
using WebKit;
using Foundation;
using System;

namespace Plugin.OAuth2.Components
{
    internal class WebViewController : UIViewController
    {
        private const string UserAgentString = "Mozilla/5.0 AppleWebKit/600.1.4 (KHTML, like Gecko) FxiOS/1.0 Mobile/12F69 Safari/600.1.4";

        public delegate void OnNavigatingDelegate(string Uri);

        private class NavDelegate : WKNavigationDelegate
        {
            private readonly OnNavigatingDelegate OnNavigating;

            public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
            {
                decisionHandler(WKNavigationActionPolicy.Allow);
                OnNavigating(navigationAction.Request.Url.ToString());
            }

            public NavDelegate(OnNavigatingDelegate onNavigating)
            {
                OnNavigating = onNavigating;
            }
        }

        public event OnNavigatingDelegate OnNavigating;

        private readonly NavDelegate WebViewDelegate;

        private readonly string StartUri;

        private WKWebView WebView { get; set; }

        public WebViewController(string startUri)
        {
            StartUri = startUri;
            WebViewDelegate = new NavDelegate(d => OnNavigating?.Invoke(d));
        }

        public override void LoadView()
        {
            base.LoadView();

            WebView = new WKWebView(CGRect.Null, new WKWebViewConfiguration());
            WebView.NavigationDelegate = WebViewDelegate;
            WebView.CustomUserAgent = UserAgentString;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var request = new NSUrlRequest(new NSUrl(StartUri)); 
            WebView.LoadRequest(request);
            View = WebView;
        }
    }
}
