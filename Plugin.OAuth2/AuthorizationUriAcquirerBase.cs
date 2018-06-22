using System;
using System.Threading.Tasks;

namespace Plugin.OAuth2
{
    internal abstract class AuthorizationUriAcquirerBase : IAuthorizationUriAcquirer
    {
        protected abstract Task ShowModalBrowserUI();
        protected abstract Task CloseModalBrowserUI();

        protected string AuthorizeUri { get; private set; }
        protected string RedirectUriRoot { get; private set; }

        private TaskCompletionSource<string> CompletionSource { get; set; }

        public Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            if (CompletionSource != null)
            {
                return Task.FromResult(default(string));
            }

            CompletionSource = new TaskCompletionSource<string>();

            AuthorizeUri = authorizeUri;
            RedirectUriRoot = redirectUriRoot;

            var task = ShowModalBrowserUI();
            return CompletionSource.Task;
        }

        protected async void NavigationHandler(string uri)
        {
            if (uri.StartsWith(RedirectUriRoot, StringComparison.OrdinalIgnoreCase))
            {
                await CloseModalBrowserUI();
                SetResult(uri);
            }
        }

        protected async void CancellationHandler()
        {
            await CloseModalBrowserUI();
            SetResult(null);
        }

        private void SetResult(string value)
        {
            CompletionSource?.SetResult(value);
            CompletionSource = null;
        }
    }
}
