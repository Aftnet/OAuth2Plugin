using Foundation;
using Plugin.OAuth2.Common;
using SafariServices;
using System.Threading.Tasks;

namespace Plugin.OAuth2.Components
{
    internal class AuthorizationUriAcquirer : IAuthorizationUriAcquirer
    {
        private TaskCompletionSource<string> TCS;

        public Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            if (TCS != null)
            {
                return null;
            }

            var startUri = new NSUrl(authorizeUri, false);
            var session = new SFAuthenticationSession(startUri, redirectUriRoot, AuthenticationSessionCompletionHandler);
            session.Start();

            TCS = new TaskCompletionSource<string>();
            return TCS.Task;
        }

        private void AuthenticationSessionCompletionHandler(NSUrl redirectUri, NSError error)
        {
            string output = null;
            if (error == null)
            {
                output = redirectUri.AbsoluteString;
            }

            TCS.SetResult(redirectUri.AbsoluteString);
            TCS = null;
        }
    }
}
