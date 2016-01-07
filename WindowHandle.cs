using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Taskkiller
{
    public class WindowHandle : System.Windows.Forms.IWin32Window
    {
        [DllImport("user32.dll")]
        public static extern int FindWindow(
            string lpClassName,      //   class name    
            string lpWindowName   //   window name    
        );

        public WindowHandle(string Classname, string WindowName)
        {
            _hwnd = (IntPtr)FindWindow(Classname, WindowName);
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
}
