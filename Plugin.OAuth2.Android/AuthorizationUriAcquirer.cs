using Plugin.OAuth2.Common;
using System;
using System.Threading.Tasks;

namespace Plugin.OAuth2.Components
{
    public class AuthorizationUriAcquirer : IAuthorizationUriAcquirer
    {
        public Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            throw new NotImplementedException();
        }
    }
}
