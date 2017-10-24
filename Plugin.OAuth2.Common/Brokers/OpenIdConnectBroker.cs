using IdentityModel;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace Plugin.OAuth2.Common.Brokers
{
    internal class OpenIdConnectBroker : BrokerBase, IOpenIDConnectBroker
    {
        private readonly string Authority;
        private readonly DiscoveryCache Discovery;

        public OpenIdConnectBroker(string authority, string clientId, string clientSecret, string scope, IAuthorizationUriAcquirer authUriAcquirer) :
            this(authority, clientId, clientSecret, scope, authUriAcquirer, null)
        {
        }

        internal OpenIdConnectBroker(string authority, string clientId, string clientSecret, string scope, IAuthorizationUriAcquirer authUriAcquirer, DiscoveryPolicy discoveryPolicy) :
            base(clientId, clientSecret, scope, authUriAcquirer)
        {
            Authority = authority;
            var client = new DiscoveryClient(authority);
            if (discoveryPolicy != null)
            {
                client.Policy = discoveryPolicy;
            }

            Discovery = new DiscoveryCache(client);
        }

        public override async Task<bool> SignInAsync()
        {
            if (SignedIn)
            {
                return true;
            }

            var discoveryResponse = await Discovery.GetAsync();
            var tokenEndpoint = discoveryResponse?.TokenEndpoint;
            if (string.IsNullOrEmpty(tokenEndpoint))
            {
                return false;
            }

            using (var tokenClient = new TokenClient(tokenEndpoint, ClientId, ClientSecret))
            {
                return await SignInAsync(discoveryResponse, tokenClient);
            }
        }

        private async Task<bool> SignInAsync(DiscoveryResponse discoveryResponse, TokenClient tokenClient)
        {
            var authorizeEndpoint = discoveryResponse?.AuthorizeEndpoint;
            if (authorizeEndpoint == null)
            {
                return false;
            }

            var authCode = await DefaultGetAuthorizationCode(authorizeEndpoint);
            if (string.IsNullOrEmpty(authCode))
            {
                return false;
            }

            var authCodeResponse = await tokenClient.RequestAuthorizationCodeAsync(authCode, RedirectUriRoot);
            if (string.IsNullOrEmpty(authCodeResponse.RefreshToken))
            {
                SignOut();
            }
            else
            {
                SecureToken = authCodeResponse.RefreshToken;
            }

            return SignedIn;
        }

        public override async Task<string> GetAccessTokenAsync()
        {
            var response = await GetTokenResponseAsync();
            return response?.AccessToken;
        }

        public async Task<string> GetIDTokenAsync()
        {
            var response = await GetTokenResponseAsync();
            return response?.IdentityToken;
        }

        private async Task<TokenResponse> GetTokenResponseAsync()
        {
            var discoveryResponse = await Discovery.GetAsync();
            using (var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, ClientId, ClientSecret))
            {
                if (!SignedIn)
                {
                    if (await SignInAsync(discoveryResponse, tokenClient) == false)
                    {
                        return null;
                    }
                }

                var response = await tokenClient.RequestRefreshTokenAsync(SecureToken);
                return response;
            }
        }
    }
}
