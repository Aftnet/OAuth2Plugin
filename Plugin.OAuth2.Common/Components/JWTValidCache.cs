using IdentityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.OAuth2.Common.Components
{
    internal class JWTValidCache
    {
        private DateTimeOffset Expiry;
        private bool Expired => Expiry.CompareTo(DateTimeOffset.UtcNow) <= 0;

        private string value;
        public string Value => Expired ? null : value;

        public bool Update(string jwt)
        {
            if (string.IsNullOrEmpty(jwt))
                return false;

            var valid = ParseJWT(jwt, out var parsedExpiry);
            if (valid)
            {
                value = jwt;
                Expiry = parsedExpiry;
            }

            return valid;
        }

        private bool ParseJWT(string jwt, out DateTimeOffset expiry)
        {
            var components = jwt.Split('.');
            if (components.Length != 3)
                return false;

            var payload = components[1];
            payload = Encoding.UTF8.GetString(Base64Url.Decode(payload));

            return false;
        }
    }
}
