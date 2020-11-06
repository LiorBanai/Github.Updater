using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Github.Updater.Client
{
    public class DownloadUpdater
    {
        public static IWebProxy? Proxy;
        private readonly string _downloadURL;
        private readonly string _targetFolder;
        private readonly TargetFrameworkAttribute _currentFrameworkAttribute;
        private readonly TaskCompletionSource<bool> _taskCompletionSource;
        private string _tempFile;
        private MyWebClient? _webClient;
        private Action<string>? ReportDownloadSpeed { get; }
        private Action<string>? ReportDownloadProgress { get; }
        private Action<int>? ReportDownloadProgressValue { get; }
        private DateTime _startedAt;

        public DownloadUpdater(string downloadUrl, string targetFolder, Action<string>? reportDownloadSpeed,
            Action<string>? reportDownloadProgress, Action<int>? reportDownloadProgressValue,
            TargetFrameworkAttribute currentFrameworkAttribute, TaskCompletionSource<bool> taskCompletionSource)
        {
            _downloadURL = downloadUrl;
            _targetFolder = targetFolder;
            _currentFrameworkAttribute = currentFrameworkAttribute;
            _taskCompletionSource = taskCompletionSource;
            ReportDownloadProgressValue = reportDownloadProgressValue;
            ReportDownloadSpeed = reportDownloadSpeed;
            ReportDownloadProgress = reportDownloadProgress;
            _tempFile = Path.Combine(Path.GetTempPath(), "Github.Updater.zip");

            if (!Directory.Exists(_targetFolder))
            {
                Directory.CreateDirectory(_targetFolder);
            }
        }

        public async Task Download()
        {
            _webClient = new MyWebClient
            {
                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore)
            };

            if (Proxy != null)
            {
                _webClient.Proxy = Proxy;
            }

            var uri = new Uri(_downloadURL);


            _webClient.DownloadProgressChanged += OnDownloadProgressChanged;

            _webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;

            _webClient.DownloadFileAsync(uri, _tempFile);

            void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
            {
                if (asyncCompletedEventArgs.Cancelled)
                {
                    _taskCompletionSource.SetResult(false);
                    return;
                }

                UnzipZipFileIntoTempFolder(_tempFile, _targetFolder);
            }

            await _taskCompletionSource.Task;
        }

        private void UnzipZipFileIntoTempFolder(string zipPath, string extractPath)
        {
            string version = "";
            if (_currentFrameworkAttribute.FrameworkName.EndsWith("4.7.1") ||
                _currentFrameworkAttribute.FrameworkName.EndsWith("4.7.2") ||
                _currentFrameworkAttribute.FrameworkName.EndsWith("4.8"))
            {
                version = "net472";
            }
            else if (_currentFrameworkAttribute.FrameworkName.EndsWith("3.1"))
            {
                version = "netcoreapp3.1";
            }

            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    //build a list of files to be extracted
                    var entries = archive.Entries.Where(entry =>
                        !entry.FullName.EndsWith("/") && entry.FullName.Contains(version));

                    foreach (ZipArchiveEntry entry in entries)
                    {
                        string target = Path.Combine(extractPath, entry.Name);
                        string directory = Path.GetDirectoryName(target);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        try
                        {
                            entry.ExtractToFile(target, true);
                        }
                        catch (Exception e)
                        {
                            //   AnalogyLogger.Instance.LogException($"Error unpacking Updater: {e.Message}", e);
                        }

                    }
                }
            }
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (_startedAt == default)
            {
                _startedAt = DateTime.Now;
            }
            else
            {
                var timeSpan = DateTime.Now - _startedAt;
                long totalSeconds = (long) timeSpan.TotalSeconds;
                if (totalSeconds > 0)
                {
                    var bytesPerSecond = e.BytesReceived / totalSeconds;
                    ReportDownloadSpeed?.Invoke(BytesToString(bytesPerSecond));
                }
            }

            ReportDownloadProgress?.Invoke($@"{BytesToString(e.BytesReceived)} / {BytesToString(e.TotalBytesToReceive)})");
            ReportDownloadProgressValue?.Invoke(e.ProgressPercentage);
        }

        private static string BytesToString(long byteCount)
        {
            string[] suf = {"B", "KB", "MB", "GB", "TB", "PB", "EB"};
            if (byteCount == 0)
            {
                return "0" + suf[0];
            }

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {suf[place]}";
        }

        public class MyWebClient : WebClient
        {
            /// <summary>
            ///     Response Uri after any redirects.
            /// </summary>
            public Uri ResponseUri;

            /// <inheritdoc />
            protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
            {
                WebResponse webResponse = base.GetWebResponse(request, result);
                ResponseUri = webResponse.ResponseUri;
                return webResponse;
            }
        }
    }
}
