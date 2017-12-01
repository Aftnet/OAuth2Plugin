using System;

using Foundation;
using AppKit;

namespace Plugin.OAuth2
{
    public partial class ModalWindowController : NSWindowController
    {
        private class WindowDelegate : NSWindowDelegate
        {
            public override void WillClose(NSNotification notification)
            {
                NSApplication.SharedApplication.AbortModal();
            }
        }

        public ModalWindowController(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public ModalWindowController(NSCoder coder) : base(coder)
        {
        }

        public ModalWindowController() : base("ModalWindow")
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public new ModalWindow Window
        {
            get { return (ModalWindow)base.Window; }
        }

        public override void WindowWillLoad()
        {
            base.WindowWillLoad();
        }

        public override void WindowDidLoad()
        {
            base.WindowDidLoad();

            Window.Delegate = new WindowDelegate();
        }
    }
}
