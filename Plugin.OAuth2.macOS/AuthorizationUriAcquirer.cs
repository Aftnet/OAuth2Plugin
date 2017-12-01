using Plugin.OAuth2.Common;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;

namespace Plugin.OAuth2.Components
{
    internal class AuthorizationUriAcquirer : IAuthorizationUriAcquirer
    {
        private NSApplication Application => NSApplication.SharedApplication;

        public Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            var controller = new ModalWindowController();
            Application.RunModalForWindow(controller.Window);

            return null;
        }
    }
}
