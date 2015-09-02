using System;
using System.Windows.Forms;
using Rift.Utils;

namespace Rift.Forms
{
    /// <summary>
    /// Represents a Windows label control that can display hyperlinks.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class WebLabel : LinkLabel
    {
        protected override Cursor DefaultCursor
        {
            get
            {
                var handCursor = Win32.LoadCursor(IntPtr.Zero, Win32.IDC_HAND);
                var temp = new Cursor(handCursor);

                Win32.DestroyCursor(handCursor);

                return temp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 32)
            {
                var cursor = Win32.LoadCursor(IntPtr.Zero, Win32.IDC_HAND);
                Win32.SetCursor(cursor);
                m.Result = IntPtr.Zero; // Handled

                Win32.DestroyCursor(cursor);

                return;
            }

            base.WndProc(ref m);
        }
    }
}