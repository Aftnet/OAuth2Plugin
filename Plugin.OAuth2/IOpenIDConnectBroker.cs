using System.Threading.Tasks;

namespace Plugin.OAuth2
{
    public interface IOpenIDConnectBroker : IOAuth2Broker
    {
        Task<string> GetIDTokenAsync();
    }
}
