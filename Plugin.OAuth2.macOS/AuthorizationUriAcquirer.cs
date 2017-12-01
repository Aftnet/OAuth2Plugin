using Plugin.OAuth2.Common;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;

namespace Plugin.OAuth2.Components
{
    internal class AuthorizationUriAcquirer : AuthorizationUriAcquirerBase
    {
        private NSApplication Application => NSApplication.SharedApplication;

        private NSWindow ModalWindow { get; set; }

        protected override Task ShowModalBrowserUI()
        {
            ModalWindow = new NSWindow()
            {
                StyleMask = NSWindowStyle.Closable | NSWindowStyle.Titled
            };

            ModalWindow.WillClose += (sender, e) => CancellationHandler();

            Application.RunModalForWindow(ModalWindow);
            return Task.CompletedTask;
        }

        protected override async Task CloseModalBrowserUI()
        {
            Application.AbortModal();

            //Need to give the UI thread some time to recover
            await Task.Delay(100);
        }
    }
}
