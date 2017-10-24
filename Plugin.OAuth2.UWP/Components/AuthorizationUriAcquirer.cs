using Plugin.OAuth2.Common;
using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace Plugin.OAuth2.Components
{
    internal class AuthorizationUriAcquirer : IAuthorizationUriAcquirer
    {
        public async Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(authorizeUri), new Uri(redirectUriRoot));
            if (result.ResponseStatus != WebAuthenticationStatus.Success)
            {
                return null;
            }
            
            return result.ResponseData;
        }
    }
}
