using System;
using System.Windows;
using System.Windows.Navigation;

namespace Plugin.OAuth2.Components
{
    /// <summary>
    /// Interaction logic for BrowserWindow.xaml
    /// </summary>
    internal partial class BrowserWindow : Window
    {
        public readonly string StartUri;
        public readonly string EndUriRoot;
        public Uri EndUri { get; private set; }

        public BrowserWindow()
        {
            InitializeComponent();
        }

        public BrowserWindow(string startUri, string endUriRoot) : this()
        {
            StartUri = startUri;
            EndUriRoot = endUriRoot;
        }

        private void BrowserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BrowserControl.Navigate(StartUri);
        }

        private void BrowserControl_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if(e.Uri.ToString().StartsWith(EndUriRoot))
            {
                EndUri = e.Uri;
                DialogResult = true;
                Close();
            }
        }
    }
}
