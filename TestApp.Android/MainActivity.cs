using Android.App;
using Android.Widget;
using Android.OS;

namespace TestApp.Android
{
    [Activity(Label = "TestApp.Android", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private readonly Shared.Handler Handler = new Shared.Handler();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

