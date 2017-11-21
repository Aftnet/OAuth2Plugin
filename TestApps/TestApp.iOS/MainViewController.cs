using System;

using UIKit;

namespace TestApp.iOS
{
    public partial class MainViewController : UIViewController
    {
        private readonly Shared.Handler Handler = new Shared.Handler();

        public MainViewController() : base("MainViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void Tapped(UIButton sender)
        {
            var task = Handler.LogIn();
        }
    }
}

