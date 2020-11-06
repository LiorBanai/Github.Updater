using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace Github.Updater.Client
{
    public class Updater
    {
        public string UpdaterFolder { get; }
        public string ApplicationTitle { get; set; }
        public string UserOrOrgNameWithRepoName { get; }
        public static string UpdaterExecutable { get; } = "Github.Updater.exe";
        public string? GitHubToken { get; set; }
        public Action<string>? ReportDownloadSpeed { get; }
        public Action<string>? ReportDownloadProgress { get; }
        public Action<int>? ReportDownloadProgressValue { get; }
        public string FullUpdaterPath => Path.Combine(UpdaterFolder, UpdaterExecutable);
        public Version? UpdaterVersion
        {
            get
            {
                if (File.Exists(FullUpdaterPath))
                {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(FullUpdaterPath);
                    return new Version(fvi.FileVersion);
                }

                return null;
            }
        }
        private static string UpdaterRepository { get; } = "LiorBanai/Github.Updater";
        private TargetFrameworkAttribute CurrentFrameworkAttribute => (TargetFrameworkAttribute)Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(TargetFrameworkAttribute));
        public Updater(string applicationTitle, string userOrOrgNameWithRepoName,string updaterFolder, string? gitHubToken, Action<string>? reportDownloadSpeed,
            Action<string>? reportDownloadProgress, Action<int>? reportDownloadProgressValue)
        {
            ApplicationTitle = applicationTitle;
            UserOrOrgNameWithRepoName = userOrOrgNameWithRepoName;
            UpdaterFolder = updaterFolder;
            GitHubToken = gitHubToken;
            ReportDownloadSpeed = reportDownloadSpeed;
            ReportDownloadProgress = reportDownloadProgress;
            ReportDownloadProgressValue = reportDownloadProgressValue;
        }

        public async Task<bool> CheckNewVersionExistsForUpdater()
        {
            var update = await GetLatestUpdater();
            if (!update.HasValue)
            {
                return false;
            }

            return GetVersionFromTagName(update.Value.TagName) > UpdaterVersion;
        }
        public async Task<bool> CheckNewVersionExistsForApplication(Version applicationVersion)
        {
            var (newData, release) = await GithubInformation.GetLatestReleaseInformation(UserOrOrgNameWithRepoName, ApplicationTitle, GitHubToken, DateTime.MinValue);
            if (release==null)
            {
                return false;
            }
            return GetVersionFromTagName(release.TagName) > applicationVersion;
        }
        public async Task DownloadUpdaterIfNeeded(Action<string>? callback=null)
        {
            var update = await GetLatestUpdater();
            if (!update.HasValue)
            {
                callback?.Invoke("Updater was not found");
                return;
            }
            if (!File.Exists(FullUpdaterPath))
            {
                callback?.Invoke("Updater Manager was not found. It will be downloaded right now");
                await DownloadUpdater(update.Value.UpdaterAsset);

            }
            else if (GetVersionFromTagName(update.Value.TagName) > UpdaterVersion)
            {
                callback?.Invoke("Updater Manager is out of date. Latest version will be downloaded right now");
                await DownloadUpdater(update.Value.UpdaterAsset);
            }
            else
            {
                callback?.Invoke("No need to download Updater");
            }
        }
        public static async Task<(string TagName, GithubObjects.GithubAsset UpdaterAsset)?> GetLatestUpdater(string? githubToken=null)
        {
            var (newData, release) = await GithubInformation.GetLatestReleaseInformation(UpdaterRepository, "Github.Updater.Client", githubToken, DateTime.MinValue);
            return release == null
                ? ((string TagName, GithubObjects.GithubAsset UpdaterAsset)?) null
                : (release.TagName, release.Assets.First(a => a.Name.Contains("Github.Updater")));
        }

        private async Task DownloadUpdater(GithubObjects.GithubAsset updaterAsset)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var downloadUpdater = new DownloadUpdater(updaterAsset.BrowserDownloadUrl, Path.GetDirectoryName(FullUpdaterPath), ReportDownloadSpeed, ReportDownloadProgress, ReportDownloadProgressValue, CurrentFrameworkAttribute, tcs);
            await downloadUpdater.Download();
        }
        private Version? GetVersionFromTagName(string? tagName)
        {
            return tagName == null ? null : new Version(tagName.Replace("V", "").Replace("v", ""));
        }

        public async Task StartApplicationUpdateProcess(string targetFolder,string assetFileName)
        {
            if (!File.Exists(FullUpdaterPath))
            {
                Console.WriteLine("Update executable was not found in the expected location");
                return;
            }
            var (newData, release) = await GithubInformation.GetLatestReleaseInformation(UserOrOrgNameWithRepoName, ApplicationTitle, GitHubToken, DateTime.MinValue);

            var asset = release?.Assets.FirstOrDefault(a => a.Name.Equals(assetFileName));
            if (asset == null)
            {
                return;
            }

            var currentProcessName = Process.GetCurrentProcess().ProcessName;
            string launchDebugger = "LaunchDebugger";
            var processStartInfo = new ProcessStartInfo();
            string data = $"\"{ApplicationTitle}\" {asset.BrowserDownloadUrl} \"{targetFolder}\" {currentProcessName} {launchDebugger}";
            processStartInfo.Arguments = data;
            processStartInfo.Verb = "runas";
            processStartInfo.FileName = FullUpdaterPath;
            try
            {
                Process.Start(processStartInfo);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Updater: {ex.Message}");
            }
        }
    }
}
