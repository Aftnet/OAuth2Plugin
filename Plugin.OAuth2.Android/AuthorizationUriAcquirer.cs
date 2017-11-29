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
    internal class AuthorizationUriAcquirer : AuthorizationUriAcquirerBase
    {
        public const string IntentStartUriKey = nameof(IntentStartUriKey);

        private WebViewActivity ModalActivity { get; set; }

        private System.Threading.Timer WebViewAcquisitionTimer { get; set; }

        protected override Task ShowModalBrowserUI()
        {
            var context = GetCurrentActivity();
            if (context is WebViewActivity)
            {
                return Task.CompletedTask;
            }

            var intent = new Intent(context, typeof(WebViewActivity));
            intent.PutExtra(IntentStartUriKey, AuthorizeUri);
            context.StartActivity(intent);

            var timerPeriod = TimeSpan.FromMilliseconds(1000);
            WebViewAcquisitionTimer = new System.Threading.Timer(WebViewAcquisitionTimerCallback, null, timerPeriod, timerPeriod);

            return Task.CompletedTask;
        }

        protected override Task CloseModalBrowserUI()
        {
            ModalActivity = GetCurrentActivity() as WebViewActivity;
            ModalActivity?.Finish();
            ModalActivity = null;

            WebViewAcquisitionTimer?.Dispose();
            WebViewAcquisitionTimer = null;

            return Task.CompletedTask;
        }
        
        private void WebViewAcquisitionTimerCallback(object state)
        {
            var webView = GetCurrentActivity() as WebViewActivity;
            if (ModalActivity == webView)
            {
                return;
            }

            ModalActivity = webView;
            if (ModalActivity != null)
            {
                ModalActivity.OnNavigating += NavigationHandler;
                ModalActivity.OnCanceling += CancellationHandler;
            }
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
