using System;
using System.Threading.Tasks;
using Plugin.OAuth2.Common;
using UIKit;

namespace Plugin.OAuth2.Components
{
    internal class AuthorizationUriAcquirer : IAuthorizationUriAcquirer
    {
        private string RedirectUriRoot { get; set; }
        private TaskCompletionSource<string> CompletionSource { get; set; }
        private UINavigationController ModalController { get; set; }

        public Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            if (CompletionSource != null)
            {
                return Task.FromResult(default(string));
            }

            CompletionSource = new TaskCompletionSource<string>();

            RedirectUriRoot = redirectUriRoot;
            var webViewController = new WebViewController(authorizeUri);
            webViewController.OnNavigating += NavigationHandler;
            ModalController = new UINavigationController(webViewController);
            webViewController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, CancelBtnHandler);

            var currentController = GetCurrentViewController();
            currentController.PresentViewControllerAsync(ModalController, true);

            return CompletionSource.Task;
        }

        private void CancelBtnHandler(object sender, System.EventArgs e)
        {
            var task = CloseModalControllerAndSetTCSResult(null);
        }

        void NavigationHandler(string Uri)
        {
            if (Uri.StartsWith(RedirectUriRoot, StringComparison.InvariantCulture))
            {
                var task = CloseModalControllerAndSetTCSResult(Uri);
            }
        }

        private async Task CloseModalControllerAndSetTCSResult(string result)
        {
            if (ModalController != null)
            {
                await ModalController.DismissViewControllerAsync(true);
                ModalController = null;
            }

            CompletionSource?.SetResult(result);
            CompletionSource = null;
        }

        private UIViewController GetCurrentViewController()
        {
            var window = UIApplication.SharedApplication.Delegate.GetWindow();
            UIViewController output = window.RootViewController;

            while (output.PresentedViewController != null)
            {
                output = output.PresentedViewController;
            }

            return output;
        }
    }
}
