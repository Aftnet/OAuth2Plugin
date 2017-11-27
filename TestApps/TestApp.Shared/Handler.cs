using Plugin.OAuth2;
using Plugin.OAuth2.Common;
using System.Threading.Tasks;

namespace TestApp.Shared
{
    public class Handler
    {
        private readonly IOAuth2Broker Broker = CrossOAuth2.GetGoogleAccountBroker("158105969996-qqevrl33qa6t011llqu50tt6k7je8bk8.apps.googleusercontent.com", "VF12gGXL_TsYjSLauMsTz3Ei", "openid email");

        public async Task<string> LogIn()
        {
            var output = await Broker.GetAccessTokenAsync();
            return output;
        }
    }
}
