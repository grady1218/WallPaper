using System;
using System.Media;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Media;

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

            StartPosition = FormStartPosition.CenterScreen;
            Location = Screen.AllScreens[1].Bounds.Location;

            Screen.GetBounds(this);
            //BackgroundImage = new Bitmap(new Bitmap(@"./annna.jpg"), new Size(Bounds.Width, Bounds.Height));

            MediaPlayer player = new MediaPlayer();
            Timer timer = new Timer()
            {
                Interval = 1000 / 60,
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
                var c = DLL.FindWindowEx( h, IntPtr.Zero, "Chrome_WidgetWin_1", null);
                if (c != IntPtr.Zero)
                {
                    hoge[1] = c;
                }
                return true;
            },IntPtr.Zero);
            return hoge;
        }

        private void Update(object sender, EventArgs e)
        {
            var window = getWindow();
            var hdc = DLL.GetDCEx(window[0], IntPtr.Zero, 0x403);
            var chromeDC = DLL.GetDCEx(DLL.GetForegroundWindow(), IntPtr.Zero, 0x403);
            DLL.BitBlt( hdc, Math.Abs(Screen.AllScreens[1].Bounds.X), Math.Abs(Screen.AllScreens[1].Bounds.Y), 3000, 1280, chromeDC, 0, 0, DLL.TernaryRasterOperations.SRCCOPY );
            Invalidate();
            DLL.ReleaseDC( window[0], hdc );
            DLL.ReleaseDC( Handle, chromeDC );
        }

    }
}
