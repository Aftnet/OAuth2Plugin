using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Webkit;

namespace Plugin.OAuth2.Components
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

        private WebView BrowserView { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetTheme(Android.Resource.Style.ThemeDeviceDefaultDialogNoActionBar);

            BrowserView = new WebView(this);
            var client = new BrowserViewClient(d => OnNavigating?.Invoke(d));
            BrowserView.SetWebViewClient(client);
            SetContentView(BrowserView);

            var startUri = Intent.GetStringExtra(AuthorizationUriAcquirer.IntentStartUriKey);
            BrowserView.LoadUrl(startUri);
        }
    }
}