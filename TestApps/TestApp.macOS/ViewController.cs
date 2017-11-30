using System;

using AppKit;
using Foundation;

namespace TestApp.macOS
{
    public partial class ViewController : NSViewController
    {
        private readonly Shared.Handler Handler = new Shared.Handler();

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void LogInBtnClick(NSObject sender)
        {
            var task = Handler.LogIn();
        }
    }
}
