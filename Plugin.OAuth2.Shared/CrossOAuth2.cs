using IdentityModel.Client;
using Plugin.OAuth2.Common;
using System;

#if !(NETSTANDARD1_4)
using Plugin.OAuth2.Common.Brokers;
using Plugin.OAuth2.Components;
#endif

namespace Plugin.OAuth2
{
    public static class CrossOAuth2
    {
        public static bool Supported
        {
            get
            {
#if (NETSTANDARD1_4)
            return false;
#else
            return true;
#endif
            }
        }

        public static IOpenIDConnectBroker GetAzureActiveDirectoryAccountBroker(string tenantId, string clientId, string clientSecret, string scope)
        {
            return GetOpenIdConnectBroker($"https://login.microsoftonline.com/{tenantId}/v2.0", clientId, clientSecret, scope, new DiscoveryPolicy { ValidateEndpoints = false, ValidateIssuerName = false });
        }

        public static IOAuth2Broker GetGithubBroker(string clientId, string clientSecret, string scope)
        {
#if (NETSTANDARD1_4)
            throw new NotImplementedException();
#else
            return new GithubBroker(clientId, clientSecret, scope, new AuthorizationUriAcquirer());
#endif
        }

        public static IOpenIDConnectBroker GetGoogleAccountBroker(string clientId, string clientSecret, string scope)
        {
            return GetOpenIdConnectBroker("https://accounts.google.com", clientId, clientSecret, scope, new DiscoveryPolicy { ValidateEndpoints = false });
        }

        public static IOpenIDConnectBroker GetMicrosoftAccountBroker(string clientId, string clientSecret, string scope)
        {
            return GetAzureActiveDirectoryAccountBroker("common", clientId, clientSecret, scope);
        }

        public static IOpenIDConnectBroker GetOpenIdConnectBroker(string authority, string clientId, string clientSecret, string scope, DiscoveryPolicy discoveryPolicy = null)
        {
#if (NETSTANDARD1_4)
            throw new NotImplementedException();
#else
            return new OpenIdConnectBroker(authority, clientId, clientSecret, scope, new AuthorizationUriAcquirer(), discoveryPolicy);
#endif
        }
    }
}
