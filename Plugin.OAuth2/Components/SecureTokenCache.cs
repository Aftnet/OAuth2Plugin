namespace Plugin.OAuth2.Components
{
    internal class SecureTokenCache
    {
        private readonly string StorageKey;

        private SecureStorageSaveDelegate SecureStorageSaveCallback;
        private SecureStorageLoadDelegate SecureStorageLoadCallback;
        private SecureStorageDeleteDelegate SecureStorageDeleteCallback;

        private string token = null;
        public string Token
        {
            get
            {
                if (token == null && SecureStorageLoadCallback != null)
                {
                    token = SecureStorageLoadCallback(StorageKey);
                }

                return token;
            }

            set
            {
                token = value;
                if (token != null)
                {
                    SecureStorageSaveCallback?.Invoke(StorageKey, token);
                }
                else
                {
                    SecureStorageDeleteCallback?.Invoke(StorageKey);
                }
            }
        }


        public SecureTokenCache(string storageKey)
        {
            StorageKey = storageKey;
        }

        public void SetTokenStorageCallbacks(SecureStorageSaveDelegate save, SecureStorageLoadDelegate load, SecureStorageDeleteDelegate delete)
        {
            if (save == null || load == null || delete == null)
            {
                throw new System.ArgumentException();
            }

            SecureStorageSaveCallback = save;
            SecureStorageLoadCallback = load;
            SecureStorageDeleteCallback = delete;
        }
    }
}
