using System;
using System.Runtime.InteropServices;

namespace Rift.Utils
{
    /// <summary>
    /// Defines the Win32 API methods, enumerations, structures and constants.
    /// </summary>
    public static class Win32
    {
        public static readonly int WM_NCSTART = RegisterWindowMessage("WM_NCSTART");

        public static readonly int WM_NCHITTEST = 0x84;

        public static readonly int WM_NCLBUTTONDOWN = 0xA1;
        
        public static readonly IntPtr HWND_BROADCAST = (IntPtr)0xffff;
        
        public static readonly IntPtr HWND_MESSAGE = (IntPtr) (-3);

        public static readonly int HTCAPTION = 2;

        public static readonly int HTBOTTOMRIGHT = 17;

        public const int CS_DROPSHADOW = 0x00020000;

        public const int CS_DBLCLKS = 0x8;

        public const int WS_MINIMIZEBOX = 0x20000;

        public const int WS_EX_COMPOSITED = 0x02000000;

        public const int IDC_HAND = 32649;

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
        
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);
        
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int cursor);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool DestroyCursor(IntPtr hCursor);

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight;

            public MARGINS(int left, int top, int right, int bottom)
            {
                cxLeftWidth = left;
                cxRightWidth = right;
                cyTopHeight = top;
                cyBottomHeight = bottom;
            }
        }
    }
}