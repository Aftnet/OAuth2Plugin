using System.Threading.Tasks;

namespace Plugin.OAuth2.Common
{
    internal interface IAuthorizationUriAcquirer
    {
        Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot);
    }
}
