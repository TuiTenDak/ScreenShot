using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Task12_ScreenShot
{
    internal class Program
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("gdi32.dll")]
        private static extern IntPtr DeleteDC(IntPtr hdc);

        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        //---------------------------------------------------------------------------------------------------------
        private static Bitmap CaptureScreen()
        {
            try
            {
                IntPtr desktopHwnd = GetDesktopWindow();
                IntPtr desktopDC = GetWindowDC(desktopHwnd);
                IntPtr compatibleDC = CreateCompatibleDC(desktopDC);

                //lấy chiều dài và chiều rộng màn hình
                int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = Screen.PrimaryScreen.Bounds.Height;

                IntPtr compatibleBitmap = CreateCompatibleBitmap(desktopDC, screenWidth, screenHeight);
                IntPtr oldBitmap = SelectObject(compatibleDC, compatibleBitmap);

                BitBlt(compatibleDC, 0, 0, screenWidth, screenHeight, desktopDC, 0, 0, 0xCC0020);
                Bitmap screenshotBitmap = Bitmap.FromHbitmap(compatibleBitmap);
                SelectObject(compatibleDC, oldBitmap);
                DeleteObject(compatibleBitmap);
                DeleteDC(compatibleDC);
                ReleaseDC(desktopHwnd, desktopDC);

                return screenshotBitmap;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        //main
        //---------------------------------------------------------------------------------------------------------
        static void Main(string[] args)
        {
              Bitmap screenshot = CaptureScreen();
              screenshot.Save(@"D:\screenshot.png", ImageFormat.Png);
           
        }
        //---------------------------------------------------------------------------------------------------------
    }
}

