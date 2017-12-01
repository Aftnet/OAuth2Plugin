using System;

using Foundation;
using AppKit;

namespace Plugin.OAuth2
{
    internal partial class ModalWindow : NSWindow
    {
        public ModalWindow(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public ModalWindow(NSCoder coder) : base(coder)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }
    }
}
