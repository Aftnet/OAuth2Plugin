using Android.App;
using Android.Content;
using Plugin.OAuth2.Common;
using System.Threading.Tasks;

namespace Plugin.OAuth2.Components
{
    public class AuthorizationUriAcquirer : IAuthorizationUriAcquirer
    {
        public const string IntentStartUriKey = nameof(IntentStartUriKey);

        public Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            var context = Application.Context;
            var intent = new Intent(context, typeof(WebViewActivity));
            intent.PutExtra(IntentStartUriKey, authorizeUri);
            context.StartActivity(intent);

            return null;
        }
    }
}
