using System;
using Xunit;

namespace Plugin.OAuth2.Common.Components.Test
{
    public class JWTValidCacheTest
    {
        private const string SampleJWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2p3dC1pZHAuZXhhbXBsZS5jb20iLCJzdWIiOiJtYWlsdG86bWlrZUBleGFtcGxlLmNvbSIsIm5iZiI6MTUwODkwNjQ3MiwiZXhwIjoxNTA4OTEwMDcyLCJpYXQiOjE1MDg5MDY0NzIsImp0aSI6ImlkMTIzNDU2IiwidHlwIjoiaHR0cHM6Ly9leGFtcGxlLmNvbS9yZWdpc3RlciJ9.SvEvaVHqg3XS_gpJ-mxkm1f0ITcF8jGbQFVYf5lvdUE";
        private static readonly DateTimeOffset SampleExpiry = new DateTimeOffset(2017, 10, 25, 5, 41, 12, TimeSpan.Zero);

        private readonly JWTValidCache Target = new JWTValidCache();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("a.b")]
        [InlineData("a.b.c")]
        public void InvalidTokenIsRejected(string value)
        {
            Assert.False(Target.Update(value));
        }

        [Fact]
        public void ParsingWorks()
        {
            var valid = Target.ParseJWT(SampleJWT, out var parsedExpiry);
            Assert.True(valid);
            var timeSpanDifference = parsedExpiry - SampleExpiry;
            Assert.True(Math.Abs(timeSpanDifference.TotalSeconds) < 1);
        }
    }
}
