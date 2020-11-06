using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Github.Updater
{
    /// <summary>
    ///     Enum representing the remind later time span.
    /// </summary>
    public enum RemindLaterFormat
    {
        /// <summary>
        ///     Represents the time span in minutes.
        /// </summary>
        Minutes,

        /// <summary>
        ///     Represents the time span in hours.
        /// </summary>
        Hours,

        /// <summary>
        ///     Represents the time span in days.
        /// </summary>
        Days
    }

    /// <summary>
    ///     Main class that lets you auto update applications by setting some static fields and executing its Start method.
    /// </summary>
    public static class AutoUpdater
    {
        private static System.Timers.Timer _remindLaterTimer;

        internal static string ChangelogURL;

        internal static string DownloadURL;

        internal static string InstallerArgs;

        internal static string RegistryLocation = $@"Software\Analogy.LogViewer\Analogy.Updater\";

        internal static string Checksum;

        internal static string HashingAlgorithm;

        internal static Version CurrentVersion;

        internal static Version InstalledVersion;

        internal static bool IsWinFormsApplication;

        internal static bool Running;

        /// <summary>
        ///     Set it to folder path where you want to download the update file. If not provided then it defaults to Temp folder.
        /// </summary>
        public static string DownloadPath;

        /// <summary>
        ///     Set the Application Title shown in Update dialog. Although AutoUpdater.NET will get it automatically, you can set this property if you like to give custom Title.
        /// </summary>
        public static string AppTitle;

        /// <summary>
        ///     URL of the xml file that contains information about latest version of the application.
        /// </summary>
        public static string AppCastURL;

        /// <summary>
        ///     Opens the download URL in default browser if true. Very usefull if you have portable application.
        /// </summary>
        public static bool OpenDownloadPage;

        /// <summary>
        ///     Set Basic Authentication credentials required to download the file.
        /// </summary>
        public static BasicAuthentication BasicAuthDownload;

        /// <summary>
        ///     Set Basic Authentication credentials required to download the XML file.
        /// </summary>
        public static BasicAuthentication BasicAuthXML;

        /// <summary>
        ///     If this is true users can see the skip button.
        /// </summary>
        public static bool ShowSkipButton = true;

        /// <summary>
        ///     If this is true users can see the Remind Later button.
        /// </summary>
        public static bool ShowRemindLaterButton = true;

        /// <summary>
        ///     If this is true users see dialog where they can set remind later interval otherwise it will take the interval from
        ///     RemindLaterAt and RemindLaterTimeSpan fields.
        /// </summary>
        public static bool LetUserSelectRemindLater = true;

        /// <summary>
        ///     Remind Later interval after user should be reminded of update.
        /// </summary>
        public static int RemindLaterAt = 2;

        ///<summary>
        ///     AutoUpdater.NET will report errors if this is true.
        /// </summary>
        public static bool ReportErrors = false;

        /// <summary>
        ///     Set this to false if your application doesn't need administrator privileges to replace the old version.
        /// </summary>
        public static bool RunUpdateAsAdmin = true;

        ///<summary>
        ///     Set this to true if you want to ignore previously assigned Remind Later and Skip settings. It will also hide Remind Later and Skip buttons.
        /// </summary>
        public static bool Mandatory;
        /// <summary>
        ///     Set Proxy server to use for all the web requests in AutoUpdater.NET.
        /// </summary>
        public static IWebProxy Proxy;

        /// <summary>
        ///     Set if RemindLaterAt interval should be in Minutes, Hours or Days.
        /// </summary>
        public static RemindLaterFormat RemindLaterTimeSpan = RemindLaterFormat.Days;

        /// <summary>
        ///     A delegate type to handle how to exit the application after update is downloaded.
        /// </summary>
        public delegate void ApplicationExitEventHandler();

        /// <summary>
        ///     An event that developers can use to exit the application gracefully.
        /// </summary>
        public static event ApplicationExitEventHandler ApplicationExitEvent;

        /// <summary>
        ///     A delegate type for hooking up update notifications.
        /// </summary>
        /// <param name="args">An object containing all the parameters recieved from AppCast XML file. If there will be an error while looking for the XML file then this object will be null.</param>
        public delegate void CheckForUpdateEventHandler(string args);

        /// <summary>
        ///     An event that clients can use to be notified whenever the update is checked.
        /// </summary>
        public static event CheckForUpdateEventHandler CheckForUpdateEvent;

        /// <summary>
        ///     A delegate type for hooking up parsing logic.
        /// </summary>
        /// <param name="args">An object containing the AppCast file received from server.</param>
        public delegate void ParseUpdateInfoHandler(ParseUpdateInfoEventArgs args);

        /// <summary>
        ///     An event that clients can use to be notified whenever the AppCast file needs parsing.
        /// </summary>
        public static event ParseUpdateInfoHandler ParseUpdateInfoEvent;

        /// <summary>
        ///     Set if you want the default update form to have a different size.
        /// </summary>
        public static Size? UpdateFormSize = null;

        public static Form MainForm;
        /// <summary>
        ///     Start checking for new version of application and display dialog to the user if update is available.
        /// </summary>
        public static void Start(string url, MainForm mainForm)
        {
            MainForm = mainForm;
            try
            {
                ServicePointManager.SecurityProtocol |= (SecurityProtocolType)192 | (SecurityProtocolType)768 |
                                                        (SecurityProtocolType)3072;
            }
            catch (NotSupportedException) { }

            if (Mandatory && _remindLaterTimer != null)
            {
                _remindLaterTimer.Stop();
                _remindLaterTimer.Close();
                _remindLaterTimer = null;
            }

            if (!Running && _remindLaterTimer == null)
            {
                Running = true;

                //  AppCastURL = fileUrl;

                IsWinFormsApplication = Application.MessageLoop;

                var backgroundWorker = new BackgroundWorker();

                backgroundWorker.DoWork += BackgroundWorkerDoWork;


                backgroundWorker.RunWorkerAsync(url);
            }
        }

        private static void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is string args)
            {
                if (CheckForUpdateEvent != null)
                {
                    CheckForUpdateEvent(args);
                }
                else
                {


                    if (!IsWinFormsApplication)
                    {
                        Application.EnableVisualStyles();
                    }

                    if (Mandatory)
                    {
                        DownloadUpdate(MainForm);
                        Exit();
                    }
                    else
                    {
                        if (Thread.CurrentThread.GetApartmentState().Equals(ApartmentState.STA))
                        {
                            ShowUpdateForm();
                        }
                        else
                        {
                            Thread thread = new Thread(ShowUpdateForm);
                            thread.CurrentCulture = thread.CurrentUICulture = CultureInfo.CurrentCulture;
                            thread.SetApartmentState(ApartmentState.STA);
                            thread.Start();
                            thread.Join();
                        }
                    }

                    return;

                }

            }


            Running = false;
        }

        /// <summary>
        /// Shows standard update dialog.
        /// </summary>
        public static void ShowUpdateForm()
        {
            var updateForm = new UpdateForm();
            if (UpdateFormSize.HasValue)
            {
                updateForm.Size = UpdateFormSize.Value;
            }

            if (updateForm.ShowDialog().Equals(DialogResult.OK))
            {
                Exit();
            }
        }


        private static string GetURL(Uri baseUri, string url)
        {
            if (!string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Relative))
            {
                Uri uri = new Uri(baseUri, url);

                if (uri.IsAbsoluteUri)
                {
                    url = uri.AbsoluteUri;
                }
            }

            return url;
        }

        /// <summary>
        /// Detects and exits all instances of running assembly, including current.
        /// </summary>
        private static void Exit()
        {
            if (ApplicationExitEvent != null)
            {
                ApplicationExitEvent();
            }
            else
            {
                var currentProcess = Process.GetCurrentProcess();
                foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    string processPath;
                    try
                    {
                        processPath = process.MainModule.FileName;
                    }
                    catch (Win32Exception)
                    {
                        // Current process should be same as processes created by other instances of the application so it should be able to access modules of other instances. 
                        // This means this is not the process we are looking for so we can safely skip this.
                        continue;
                    }

                    if (process.Id != currentProcess.Id &&
                        currentProcess.MainModule.FileName == processPath
                    ) //get all instances of assembly except current
                    {
                        if (process.CloseMainWindow())
                        {
                            process.WaitForExit((int)TimeSpan.FromSeconds(10)
                                .TotalMilliseconds); //give some time to process message
                        }

                        if (!process.HasExited)
                        {
                            process.Kill(); //TODO show UI message asking user to close program himself instead of silently killing it
                        }
                    }
                }

                if (IsWinFormsApplication)
                {
                    MethodInvoker methodInvoker = Application.Exit;
                    methodInvoker.Invoke();
                }
#if NETWPF
                else if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        System.Windows.Application.Current.Shutdown()));
                }
#endif
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        private static Attribute GetAttribute(Assembly assembly, Type attributeType)
        {
            object[] attributes = assembly.GetCustomAttributes(attributeType, false);
            if (attributes.Length == 0)
            {
                return null;
            }

            return (Attribute)attributes[0];
        }

        //internal static void SetTimer(DateTime remindLater)
        //{
        //    TimeSpan timeSpan = remindLater - DateTime.Now;

        //    var context = SynchronizationContext.Current;

        //    _remindLaterTimer = new System.Timers.Timer
        //    {
        //        Interval = (int)timeSpan.TotalMilliseconds,
        //        AutoReset = false
        //    };

        //    _remindLaterTimer.Elapsed += delegate
        //    {
        //        _remindLaterTimer = null;
        //        if (context != null)
        //        {
        //            try
        //            {
        //                context.Send(state => Start(), null);
        //            }
        //            catch (InvalidAsynchronousStateException)
        //            {
        //                Start();
        //            }
        //        }
        //        else
        //        {
        //            Start();
        //        }
        //    };

        //    _remindLaterTimer.Start();
        //}

        /// <summary>
        ///     Opens the Download window that download the update and execute the installer when download completes.
        /// </summary>
        public static bool DownloadUpdate(Form parent)
        {
            var downloadDialog = new DownloadUpdateDialog(DownloadURL);

            try
            {
                return downloadDialog.ShowDialog(parent).Equals(DialogResult.OK);
            }
            catch (TargetInvocationException)
            {
            }

            return false;
        }
    }



    /// <summary>
    ///     An object of this class contains the AppCast file received from server.
    /// </summary>
    public class ParseUpdateInfoEventArgs : EventArgs
    {
        /// <summary>
        ///     Remote data received from the AppCast file.
        /// </summary>
        public string RemoteData { get; }

        /// <summary>
        ///     An object containing the AppCast file received from server.
        /// </summary>
        /// <param name="remoteData">A string containing remote data received from the AppCast file.</param>
        public ParseUpdateInfoEventArgs(string remoteData)
        {
            RemoteData = remoteData;
        }
    }

    /// <summary>
    ///     Provides Basic Authentication header for web request.
    /// </summary>
    public class BasicAuthentication
    {
        private string Username { get; }

        private string Password { get; }

        /// <summary>
        /// Initializes credentials for Basic Authentication.
        /// </summary>
        /// <param name="username">Username to use for Basic Authentication</param>
        /// <param name="password">Password to use for Basic Authentication</param>
        public BasicAuthentication(string username, string password)
        {
            Username = username;
            Password = password;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var token = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
            return $"Basic {token}";
        }
    }
}
