using System.Threading.Tasks;

namespace Plugin.OAuth2.Common
{
    public interface IOpenIDConnectBroker : IOAuth2Broker
    {
        Task<string> GetIDTokenAsync();
    }
}
