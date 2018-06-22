using IdentityModel.Client;
using System.Threading.Tasks;

namespace Plugin.OAuth2.Brokers
{
    internal class GithubBroker : BrokerBase
    {
        private const string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        private const string TokenEndpoint = "https://github.com/login/oauth/access_token";

        public GithubBroker(string clientId, string clientSecret, string scope, IAuthorizationUriAcquirer authUriAcquirer) : base(clientId, clientSecret, scope, authUriAcquirer)
        {
        }

        public override async Task<bool> SignInAsync()
        {
            if (SignedIn)
            {
                return true;
            }

            var authCode = await DefaultGetAuthorizationCode(AuthorizationEndpoint);
            if (string.IsNullOrEmpty(authCode))
            {
                return false;
            }

            using (var tokenClient = new TokenClient(TokenEndpoint, ClientId, ClientSecret))
            {
                var authCodeResponse = await tokenClient.RequestAuthorizationCodeAsync(authCode, RedirectUriRoot);
                if (string.IsNullOrEmpty(authCodeResponse.AccessToken))
                {
                    return false;
                }
                else
                {
                    SecureToken = authCodeResponse.AccessToken;
                    return true;
                }
            }
        }

        public override async Task<string> GetAccessTokenAsync()
        {
            if (!SignedIn)
            {
                await SignInAsync();
            }

            return SecureToken;
        }
    }
}
