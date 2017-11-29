using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Java.Lang;
using Java.Util;
using Plugin.OAuth2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.OAuth2.Components
{
    public class AuthorizationUriAcquirer : IAuthorizationUriAcquirer
    {
        public const string IntentStartUriKey = nameof(IntentStartUriKey);

        private TaskCompletionSource<string> CompletionSource { get; set; }

        private WebViewActivity WebView { get; set; }

        private System.Threading.Timer WebViewAcquisitionTimer { get; set; }

        public Task<string> GetAuthorizationUriAsync(string authorizeUri, string redirectUriRoot)
        {
            if (CompletionSource != null)
            {
                return Task.FromResult(default(string));
            }

            CompletionSource = new TaskCompletionSource<string>();

            var context = GetCurrentActivity();
            var intent = new Intent(context, typeof(WebViewActivity));
            intent.PutExtra(IntentStartUriKey, authorizeUri);
            context.StartActivity(intent);

            var timerPeriod = TimeSpan.FromMilliseconds(500);
            WebViewAcquisitionTimer = new System.Threading.Timer(WebViewAcquisitionTimerCallback, null, timerPeriod, timerPeriod);

            return CompletionSource.Task;
        }
        
        private void WebViewAcquisitionTimerCallback(object state)
        {
            var webViewInstance = GetCurrentActivity() as WebViewActivity;
            if (WebView != webViewInstance)
            {
                
            }
        }

        private void CancelBtnHandler(object sender, System.EventArgs e)
        {
            var task = CloseModalControllerAndSetTCSResult(null);
        }

        void NavigationHandler(string Uri)
        {
            if (Uri.StartsWith(RedirectUriRoot, StringComparison.InvariantCulture))
            {
                var task = CloseModalControllerAndSetTCSResult(Uri);
            }
        }

        private async Task CloseModalControllerAndSetTCSResult(string result)
        {
            if (ModalController != null)
            {
                await ModalController.DismissViewControllerAsync(true);
                ModalController = null;
            }

            CompletionSource?.SetResult(result);
            CompletionSource = null;
        }

        private static Activity GetCurrentActivity()
        {
            Activity activity = null;
            List<Java.Lang.Object> objects = null;

            var activityThreadClass = Class.ForName("android.app.ActivityThread");
            var activityThread = activityThreadClass.GetMethod("currentActivityThread").Invoke(null);
            var activityFields = activityThreadClass.GetDeclaredField("mActivities");
            activityFields.Accessible = true;

            var obj = activityFields.Get(activityThread);

            if (obj is JavaDictionary)
            {
                var activities = (JavaDictionary)obj;
                objects = new List<Java.Lang.Object>(activities.Values.Cast<Java.Lang.Object>().ToList());
            }
            else if (obj is ArrayMap)
            {
                var activities = (ArrayMap)obj;
                objects = new List<Java.Lang.Object>(activities.Values().Cast<Java.Lang.Object>().ToList());
            }
            else if (obj is IMap)
            {
                var activities = (IMap)activityFields.Get(activityThread);
                objects = new List<Java.Lang.Object>(activities.Values().Cast<Java.Lang.Object>().ToList());
            }

            if (objects != null && objects.Any())
            {
                foreach (var activityRecord in objects)
                {
                    var activityRecordClass = activityRecord.Class;
                    var pausedField = activityRecordClass.GetDeclaredField("paused");
                    pausedField.Accessible = true;

                    if (!pausedField.GetBoolean(activityRecord))
                    {
                        var activityField = activityRecordClass.GetDeclaredField("activity");
                        activityField.Accessible = true;
                        activity = (Activity)activityField.Get(activityRecord);
                        break;
                    }
                }
            }

            return activity;
        }

    }
}
