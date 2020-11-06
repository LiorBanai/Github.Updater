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
        public static string UpdaterExecutable { get; } = "Github.Updater.exe";
        public string? GitHubToken { get; set; }
        public Action<string>? ReportDownloadSpeed { get; }
        public Action<string>? ReportDownloadProgress { get; }
        public Action<int>? ReportDownloadProgressValue { get; }
        public string FullUpdaterPath => Path.Combine(UpdaterFolder, UpdaterExecutable);
        public static string UpdaterRepository { get; } = "LiorBanai/Github.Updater";
        public Version? UpdaterVersion
        {
            get
            {
                if (File.Exists(FullUpdaterPath))
                {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(UpdaterExecutable);
                    return new Version(fvi.FileVersion);
                }

                return null;
            }
        }
        public TargetFrameworkAttribute CurrentFrameworkAttribute => (TargetFrameworkAttribute)Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(TargetFrameworkAttribute));
        public Updater(string updaterFolder, string? gitHubToken, Action<string>? reportDownloadSpeed,
            Action<string>? reportDownloadProgress, Action<int>? reportDownloadProgressValue)
        {
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
        private async Task DownloadUpdater(GithubObjects.GithubAsset updaterAsset)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var downloadUpdater = new DownloadUpdater(updaterAsset.BrowserDownloadUrl, Path.GetDirectoryName(FullUpdaterPath),ReportDownloadSpeed,ReportDownloadProgress,ReportDownloadProgressValue, CurrentFrameworkAttribute, tcs);
            await downloadUpdater.Download();
        }
        private Version? GetVersionFromTagName(string? tagName)
        {
            return tagName == null ? null : new Version(tagName.Replace("V", "").Replace("v", ""));
        }
       
        public static async Task<(string TagName, GithubObjects.GithubAsset UpdaterAsset)?> GetLatestUpdater(string? githubToken=null)
        {
            var (newData, release) = await GithubInformation.GetLatestReleaseInformation(UpdaterRepository, "Github.Updater.Client", githubToken, DateTime.MinValue);
            return release == null
                ? ((string TagName, GithubObjects.GithubAsset UpdaterAsset)?) null
                : (release.TagName, release.Assets.First(a => a.Name.Contains("Github.Updater")));
        }
    }
}
