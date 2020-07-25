using System;
using System.Media;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace WallPaper
{
    class Form1 : Form
    {
        public Form1()
        {
            DoubleBuffered = true;
            Opacity = 0;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            MediaPlayer player = new MediaPlayer();
            player.Open( new Uri(@"C:\Users\grady\Videos\Captures\are.mp4") );
            player.Play();
            Timer timer = new Timer()
            {
                Interval = 1000 / 30,
                Enabled = true
            };
            timer.Tick += Update;
        }
        private IntPtr[] getWindow()
        {
            DLL.SendMessageTimeout(DLL.FindWindow("Progman", null), 0x052C, new IntPtr(0), IntPtr.Zero, 0x0, 1000, out var result);

            var hoge = new IntPtr[2];
            DLL.EnumWindows((h, l) =>
            {
                var shell = DLL.FindWindowEx( h, IntPtr.Zero, "SHELLDLL_DefView", null );
                if(shell != IntPtr.Zero)
                {
                    hoge[0] = DLL.FindWindowEx( IntPtr.Zero, h, "WorkerW", null );
                }
                var c = DLL.FindWindowEx(h, IntPtr.Zero, "Chrome_RenderWidgetHostHWND", null);
                if (c != IntPtr.Zero)
                {
                    hoge[1] = DLL.FindWindowEx(IntPtr.Zero, h, "Chrome_WidgetWin_1", null);
                }
                return true;
            },IntPtr.Zero);
            return hoge;
        }

        private void Update(object sender, EventArgs e)
        {
            var window = getWindow();
            var hdc = DLL.GetDCEx(window[0], IntPtr.Zero, 0x403);
            var chromeDC = DLL.GetDCEx(Handle, IntPtr.Zero, 0x403);

            Console.WriteLine( chromeDC );
            DLL.BitBlt( hdc, 0, 0, 1980, 1280, chromeDC, 0, 0, DLL.TernaryRasterOperations.SRCCOPY );
            Invalidate();
            DLL.ReleaseDC( window[0], hdc );
            DLL.ReleaseDC( Handle, chromeDC );
        }

    }
}
