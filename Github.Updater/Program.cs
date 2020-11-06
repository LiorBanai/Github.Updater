using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Github.Updater
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
           
#if NETCOREAPP
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(location);
            AutoUpdater.DownloadPath = directory;
            string title = null;
            string downloadURL = null;
            string targetFolder = null;
            string processToKill = string.Empty;
            string applicationToRunPostUpdate = string.Empty;
            if (args.Length >= 4)
            {
                title = args[0];
                downloadURL = args[1];
                targetFolder = args[2];
                processToKill = args[3];
            }
            else
            {
                Application.Exit();
                return;
            }
            if (args.Length >= 5)
            {
                applicationToRunPostUpdate = args[4];
            }
            if (args.Length == 6 && args[5]=="LaunchDebugger")
            {
                Debugger.Launch();
            }
            KilAnalogyIfNeeded(processToKill);
            Application.Run(new MainForm(title, downloadURL, targetFolder,applicationToRunPostUpdate));
        }

        private static void KilAnalogyIfNeeded(string processToKIll)
        {
            var analogies = Process.GetProcessesByName(processToKIll);
            foreach (var analogy in analogies)
            {
                try
                {
                    analogy.Kill();
                }
                catch (Exception)
                {
                    //
                }
            }
        }
    }
}
