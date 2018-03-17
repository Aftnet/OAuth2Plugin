using IdentityModel;
using IdentityModel.Client;
using Plugin.OAuth2.Common.Components;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Plugin.OAuth2.Common.Brokers
{
    internal abstract class BrokerBase : IOAuth2Broker
    {
        public abstract Task<bool> SignInAsync();
        public abstract Task<string> GetAccessTokenAsync();

        internal static readonly Regex AuthCodeRegex = new Regex("code=([^&]+)");
        internal static readonly Regex StateCodeRegex = new Regex("state=([^&]+)");

        internal const string RedirectUriRoot = "https://localhost";

        protected readonly IAuthorizationUriAcquirer AuthUriAcquirer;
        private readonly SecureTokenCache SecureCache;

        protected string SecureToken
        {
            get { return SecureCache.Token; }
            set { SecureCache.Token = value; }
        }

        public bool SignedIn => !string.IsNullOrEmpty(SecureToken);

        protected readonly string ClientId;
        protected readonly string ClientSecret;
        protected readonly string Scope;

        private string CurrentStateToken;

        public BrokerBase(string clientId, string clientSecret, string scope, IAuthorizationUriAcquirer authUriAcquirer)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Scope = scope;

            AuthUriAcquirer = authUriAcquirer;
            var storageKey = (clientId + clientSecret + scope).Sha1();
            SecureCache = new SecureTokenCache(storageKey);
        }

        public void SignOut()
        {
            SecureToken = null;
        }

        public void SetTokenStorageCallbacks(SecureStorageSaveDelegate save, SecureStorageLoadDelegate load, SecureStorageDeleteDelegate delete)
        {
            SecureCache.SetTokenStorageCallbacks(save, load, delete);
        }

        protected async Task<string> DefaultGetAuthorizationCode(string authorizationEndpoint)
        {
            CurrentStateToken = GenerateAuthorizationStateToken();
            var request = new RequestUrl(authorizationEndpoint);
            var authorizeUri = request.CreateAuthorizeUrl(ClientId, OidcConstants.ResponseTypes.Code, Scope, RedirectUriRoot, CurrentStateToken);

            var redirectUri = await AuthUriAcquirer.GetAuthorizationUriAsync(authorizeUri, RedirectUriRoot);
            if (string.IsNullOrEmpty(redirectUri))
            {
                return null;
            }

            var returnedStateToken = StateCodeRegex.Match(redirectUri).Groups[1].Value;
            if (returnedStateToken != CurrentStateToken)
            {
                return null;
            }

            var authCode = AuthCodeRegex.Match(redirectUri).Groups[1].Value;
            authCode = WebUtility.UrlDecode(authCode);
            return authCode;
        }

        protected string GenerateAuthorizationStateToken()
        {
            const int NumRandomBytes = 16;

            using (var generator = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[NumRandomBytes];
                generator.GetBytes(randomBytes);
                return Base64Url.Encode(randomBytes);
            }
        }
    }
}
