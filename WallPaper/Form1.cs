using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Drawing;

namespace WallPaper
{
    class Form1 : Form
    {
        public Form1()
        {

            Bitmap p = new Bitmap( "./haikei.jpg" );
            var workerW = getWorkerW();
            var b = DLL.SelectObject( workerW, p.GetHbitmap() );
            var aa = DLL.BitBlt(workerW, 50, 50, p.Width, p.Height, workerW, 0, 0, DLL.TernaryRasterOperations.SRCCOPY);
            Console.WriteLine( b );
            DLL.DeleteDC( workerW );

            Timer timer = new Timer()
            {
                Interval = 13,
                Enabled = true
            };
            timer.Tick += Update;
            Paint += Draw;
        }
        private IntPtr getWorkerW()
        {
            var workerw = IntPtr.Zero;
            DLL.EnumWindows((h, l) =>
            {
                var shell = DLL.FindWindowEx( h, IntPtr.Zero, "SHELLDLL_DefView", null );
                if(shell != null)
                {
                    workerw = DLL.FindWindowEx( IntPtr.Zero, h, "WorkerW", null );
                }
                return true;
            },IntPtr.Zero);
            return workerw;
        }
        private void Draw(object sender, PaintEventArgs e)
        {
        }

        private void Update(object sender, EventArgs e)
        {
            Invalidate();
        }

    }
}
