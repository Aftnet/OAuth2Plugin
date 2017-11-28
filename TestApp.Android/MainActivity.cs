using Android.App;
using Android.OS;
using Android.Widget;

namespace TestApp.Android
{
    [Activity(Label = "TestApp.Android", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private readonly Shared.Handler Handler = new Shared.Handler();

        private Button LogInBtn { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            LogInBtn = FindViewById<Button>(Resource.Id.LogInBtn);
            LogInBtn.Click += LogInBtn_Click;
        }

        private void LogInBtn_Click(object sender, System.EventArgs e)
        {
            var task = Handler.LogIn();
        }
    }
}