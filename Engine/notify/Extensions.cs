using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSWinNotify
{
    internal static class Extensions
    {
        public enum Effect { Roll, Slide, Center, Blend }
        private static int[] dirmap = { 1, 5, 4, 6, 2, 10, 8, 9 };
        private static int[] effmap = { 0, 0x40000, 0x10, 0x80000 };
        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr handle, int msec, int flags);

        internal static void Animate(this Control ctrl, Effect fx, int msec, int angle)
        {
            int flags = effmap[(int)fx];
            if (ctrl.Visible) { flags |= 0x10000; angle += 180; }
            else
            {
                if (ctrl.TopLevelControl == ctrl) flags |= 0x20000;
                else if (fx == Effect.Blend) throw new ArgumentException();
            }
            flags |= dirmap[(angle % 360) / 45];
            bool ok = AnimateWindow(ctrl.Handle, msec, flags);
            /*if (!ok) throw new Exception("Animation failed");*/
            ctrl.Visible = !ctrl.Visible;
        }
    }
}
