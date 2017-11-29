using Plugin.OAuth2.Common;
using System.Threading.Tasks;
using UIKit;

namespace Plugin.OAuth2.Components
{
    internal class AuthorizationUriAcquirer : AuthorizationUriAcquirerBase
    {
        private UINavigationController ModalController { get; set; }

        protected override Task ShowModalBrowserUI()
        {
            var webViewController = new WebViewController(AuthorizeUri);
            webViewController.OnNavigating += NavigationHandler;
            ModalController = new UINavigationController(webViewController);
            webViewController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (d, e) => CancellationHandler());

            var currentController = GetCurrentViewController();
            return currentController.PresentViewControllerAsync(ModalController, true);
        }

        protected override async Task CloseModalBrowserUI()
        {
            if (ModalController != null)
            {
                await ModalController.DismissViewControllerAsync(true);
                ModalController = null;
            }
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
