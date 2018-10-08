using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Snippets
{
    internal class NativeMethods
    {
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static System.Threading.Mutex singleton = new Mutex(true, "Snippets");

        [STAThread]
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var category =
                e.Args.Length >= 2 && e.Args[0] == "-c"
                    ? e.Args[1]
                    : null;
            if (singleton.WaitOne(TimeSpan.Zero, true))
            {
                singleton.ReleaseMutex();
                var mainWindow = new MainWindow(category);
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.Show();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
                this.Shutdown();
            }

        }
    }
}
