using Plugin.OAuth2.Common;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;

namespace Plugin.OAuth2.Components
{
    internal class AuthorizationUriAcquirer : IAuthorizationUriAcquirer
    {
        public async Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            var controller = new ModalWindowController(authorizeUri, redirectUriRoot);
            NSApplication.SharedApplication.RunModalForWindow(controller.Window);

            await Task.Delay(100);
            return controller.Result;
        }
    }
}
