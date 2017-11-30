using Plugin.OAuth2.Common;
using System.Threading.Tasks;

namespace Plugin.OAuth2.Components
{
    internal class AuthorizationUriAcquirer : AuthorizationUriAcquirerBase
    {
        protected override Task ShowModalBrowserUI()
        {
            return Task.CompletedTask;
        }

        protected override Task CloseModalBrowserUI()
        {
            return Task.CompletedTask;
        }
    }
}
