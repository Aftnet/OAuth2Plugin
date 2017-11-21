using Plugin.OAuth2;
using Plugin.OAuth2.Common;
using System.Threading.Tasks;

namespace TestApp.Shared
{
    public class Handler
    {
        private readonly IOAuth2Broker Broker = CrossOAuth2.GetGithubBroker("", "", "");

        public async Task<string> LogIn()
        {
            var output = await Broker.GetAccessTokenAsync();
            return output;
        }
    }
}
