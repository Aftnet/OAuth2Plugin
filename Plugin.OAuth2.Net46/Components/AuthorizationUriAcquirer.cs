using Plugin.OAuth2.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Plugin.OAuth2.Components
{
    internal class AuthorizationUriAcquirer : IAuthorizationUriAcquirer
    {
        public Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            string output = null;

            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var browserWindow = new BrowserWindow(authorizeUri, redirectUriRoot);
            browserWindow.Owner = currentWindow;
            bool? dialogResult;
            try
            {
                dialogResult = browserWindow.ShowDialog();
            }
            catch
            {
                return Task.FromResult(output);
            }
            if (dialogResult == false)
            {
                return Task.FromResult(output);
            }

            output = browserWindow.EndUri.ToString();
            return Task.FromResult(output);
        }
    }
}
