using System.Threading.Tasks;

namespace Plugin.OAuth2.Common
{
    public delegate void SecureStorageSaveDelegate(string key, string value);

    public delegate string SecureStorageLoadDelegate(string key);

    public delegate void SecureStorageDeleteDelegate(string key);

    public interface IOAuth2Broker
    {
        bool SignedIn { get; }

        Task<bool> SignInAsync();

        void SignOut();

        Task<string> GetAccessTokenAsync();

        void SetTokenStorageCallbacks(SecureStorageSaveDelegate save, SecureStorageLoadDelegate load, SecureStorageDeleteDelegate delete);
    }
}
