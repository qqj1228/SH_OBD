using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SH_OBD {
    public class RichTextBoxPrintCtrl : RichTextBox {
        private const double anInch = 14.4;
        private const int EM_FORMATRANGE = 1081;

        [DllImport("user32.dll")]
        extern static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        public int Print(int charFrom, int charTo, PrintPageEventArgs e) {
            RichTextBoxPrintCtrl.RECT rect1;
            rect1.Top = (int)(e.MarginBounds.Top * anInch);
            rect1.Bottom = (int)(e.MarginBounds.Bottom * anInch);
            rect1.Left = (int)(e.MarginBounds.Left * anInch);
            rect1.Right = (int)(e.MarginBounds.Right * anInch);
            RichTextBoxPrintCtrl.RECT rect2;
            rect2.Top = (int)(e.PageBounds.Top * anInch);
            rect2.Bottom = (int)(e.PageBounds.Bottom * anInch);
            rect2.Left = (int)(e.PageBounds.Left * anInch);
            rect2.Right = (int)(e.PageBounds.Right * anInch);
            IntPtr hdc = e.Graphics.GetHdc();
            RichTextBoxPrintCtrl.FORMATRANGE formatrange;
            formatrange.chrg.cpMax = charTo;
            formatrange.chrg.cpMin = charFrom;
            formatrange.hdc = hdc;
            formatrange.hdcTarget = hdc;
            formatrange.rc = rect1;
            formatrange.rcPage = rect2;
            IntPtr wp = new IntPtr(1);
            IntPtr num1 = Marshal.AllocCoTaskMem(Marshal.SizeOf(formatrange));
            Marshal.StructureToPtr(formatrange, num1, false);
            IntPtr num2 = RichTextBoxPrintCtrl.SendMessage(this.Handle, EM_FORMATRANGE, wp, num1);
            Marshal.FreeCoTaskMem(num1);
            e.Graphics.ReleaseHdc(hdc);
            return num2.ToInt32();
        }

        private struct RECT {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private struct CHARRANGE {
            public int cpMin;
            public int cpMax;
        }

        private struct FORMATRANGE {
            public IntPtr hdc;
            public IntPtr hdcTarget;
            public RichTextBoxPrintCtrl.RECT rc;
            public RichTextBoxPrintCtrl.RECT rcPage;
            public RichTextBoxPrintCtrl.CHARRANGE chrg;
        }
    }
}