using System;

using Foundation;
using AppKit;
using WebKit;

namespace Plugin.OAuth2
{
    public partial class ModalWindowController : NSWindowController
    {
        private class WindowDelegate : NSWindowDelegate
        {
            internal delegate void OnClosingDelegate();

            private readonly OnClosingDelegate OnClosing;

            public override void WillClose(NSNotification notification)
            {
                OnClosing();
            }

            public WindowDelegate(OnClosingDelegate onClosing)
            {
                OnClosing = onClosing;
            }
        }

        private class NavDelegate : WKNavigationDelegate
        {
            internal delegate void OnNavigatingDelegate(string Uri);

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

        private NSApplication App => NSApplication.SharedApplication;

        private string StartUri { get; set; }
        private string EndUriRoot { get; set; }
        public string Result { get; private set; }

        private readonly WindowDelegate ModalDelegate;
        private NavDelegate NavigationDelegate;

        public ModalWindowController(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public ModalWindowController(NSCoder coder) : base(coder)
        {
        }

        public ModalWindowController(string startUri, string endUriRoot) : base("ModalWindow")
        {
            StartUri = startUri;
            EndUriRoot = endUriRoot;
            Result = null;

            ModalDelegate = new WindowDelegate(() =>
            {
                App.AbortModal();
            });

            NavigationDelegate = new NavDelegate(d =>
            {
                if (d.StartsWith(EndUriRoot, StringComparison.InvariantCultureIgnoreCase))
                {
                    Result = d;
                    Window.Close();
                }
            });
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public new ModalWindow Window
        {
            get { return (ModalWindow)base.Window; }
        }

        public override void WindowWillLoad()
        {
            base.WindowWillLoad();
        }

        public override void WindowDidLoad()
        {
            base.WindowDidLoad();

            Window.Delegate = ModalDelegate;

            WebView.NavigationDelegate = NavigationDelegate;
            WebView.LoadRequest(new NSUrlRequest(new NSUrl(StartUri)));
        }
    }
}
