using Xunit;

namespace Plugin.OAuth2.Common.Components.Test
{
    public class JWTValidCacheTest
    {
        private readonly JWTValidCache Target = new JWTValidCache();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("a.b")]
        public void InvalidTokenIsRejected(string value)
        {
            Assert.False(Target.Update(value));
        }
    }
}
