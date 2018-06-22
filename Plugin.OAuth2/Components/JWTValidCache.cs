using IdentityModel;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Plugin.OAuth2.Components
{
    internal class JWTValidCache
    {
        private class JWTStructure
        {
            [JsonProperty("exp")]
            public int ExpiryTimestamp { get; set; }
        }

        private DateTimeOffset Expiry = DateTimeOffset.MinValue;
        private bool Expired => DateTimeOffsetIsPast(Expiry);

        private string value = null;
        public string Value => Expired ? null : value;

        public bool Update(string jwt)
        {
            if (string.IsNullOrEmpty(jwt))
                return false;

            var valid = ParseJWT(jwt, out var parsedExpiry);
            if (valid && !DateTimeOffsetIsPast(parsedExpiry))
            {
                value = jwt;
                Expiry = parsedExpiry;
            }

            return valid;
        }

        internal bool ParseJWT(string jwt, out DateTimeOffset expiry)
        {
            var components = jwt.Split('.');
            if (components.Length != 3)
            {
                return false;
            }

            var payload = components[1];
            try
            {
                payload = Encoding.UTF8.GetString(Base64Url.Decode(payload));
                var parsedPayload = JsonConvert.DeserializeObject<JWTStructure>(payload);
                expiry = parsedPayload.ExpiryTimestamp.ToDateTimeFromEpoch();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool DateTimeOffsetIsPast(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.CompareTo(DateTimeOffset.UtcNow) <= 0;
        }
    }
}
