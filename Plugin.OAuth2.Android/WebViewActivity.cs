using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Webkit;

namespace Plugin.OAuth2
{
    [Activity(Label = "WebViewActivity")]
    internal class WebViewActivity : Activity
    {
        public delegate void OnNavigatingDelegate(string Uri);

        private class BrowserViewClient : WebViewClient
        {
            private readonly OnNavigatingDelegate OnNavigating;

            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                OnNavigating(url);
            }

            public BrowserViewClient(OnNavigatingDelegate onNavigating)
            {
                OnNavigating = onNavigating;
            }
        };

        public event OnNavigatingDelegate OnNavigating;

        private readonly string StartUri;

        private WebView BrowserView { get; set; }

        public WebViewActivity() : this(string.Empty)
        {

        }

        public WebViewActivity(string startUri)
        {
            StartUri = startUri;

            SetTheme(Android.Resource.Style.ThemeHoloDialogWhenLarge);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            BrowserView = new WebView(this);
            var client = new BrowserViewClient(d => OnNavigating?.Invoke(d));
            BrowserView.SetWebViewClient(client);
            SetContentView(BrowserView);

            BrowserView.LoadUrl(StartUri);
        }
    }
}