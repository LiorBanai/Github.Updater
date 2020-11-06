using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Github.Updater.Client
{
    public static class GithubInformation
    {
        public static async Task<(bool newData, T result)> GetAsync<T>(string uri, string userAgent, string? token, DateTime lastModified)
        {
            try
            {
                Uri myUri = new Uri(uri);
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.UserAgent = userAgent;
                if (!string.IsNullOrEmpty(token))
                {
                    myHttpWebRequest.Headers.Add(HttpRequestHeader.Authorization, $"Token {token}");
                }

                myHttpWebRequest.IfModifiedSince = lastModified;

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)await myHttpWebRequest.GetResponseAsync();
                if (myHttpWebResponse.StatusCode == HttpStatusCode.NotModified)
                {
                    return (false, default)!;
                }

                using (var reader = new System.IO.StreamReader(myHttpWebResponse.GetResponseStream()))
                {
                    string responseText = await reader.ReadToEndAsync();
                    return (true, JsonConvert.DeserializeObject<T>(responseText));
                }
            }
            catch (WebException e) when (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotModified)
            {
                return (false, default)!;
            }
            catch (Exception)
            {
                return (false, default)!;
            }
        }

        ///  <summary>
        /// Get latest release info
        ///  </summary>
        ///  <param name="userOrOrgNameWithRepoName">name of repository with user name or organization name(e.g: "Analogy-LogViewer/Analogy.LogViewer")</param>
        ///  <param name="userAgent"></param>
        ///  <param name="optionalGithubToken"></param>
        ///  <param name="lastUpdate"></param>
        ///  <returns></returns>
        public static async Task<(bool newData, GithubObjects.GithubReleaseEntry? release)> GetLatestReleaseInformation(string userOrOrgNameWithRepoName, string userAgent, string? optionalGithubToken, DateTime lastUpdate)
        {
            string apiPath = $"https://api.github.com/repos/{userOrOrgNameWithRepoName}";
            var (newData, entries) = await GetAsync<GithubObjects.GithubReleaseEntry[]>(apiPath + "/releases", userAgent, optionalGithubToken, lastUpdate);
            if (entries != null)
            {
                GithubObjects.GithubReleaseEntry? release = entries.OrderByDescending(r => r.Published).FirstOrDefault();
                return (newData, release);
            }
            return (false, null);
        }

        ///  <summary>
        /// Get latest release info
        ///  </summary>
        ///  <param name="userOrOrgNameWithRepoName">name of repository with user name or organization name(e.g: "Analogy-LogViewer/Analogy.LogViewer")</param>
        ///  <param name="userAgent"></param>
        ///  <param name="optionalGithubToken"></param>
        ///  <returns></returns>
        public static async Task<(bool newData, GithubObjects.GithubReleaseEntry[] release)> GetAllReleases(string userOrOrgNameWithRepoName, string userAgent, string optionalGithubToken)
        {
            try
            {
                string apiPath = $"https://api.github.com/repos/{userOrOrgNameWithRepoName}";
                var (newData, entries) = await GetAsync<GithubObjects.GithubReleaseEntry[]>(apiPath + "/releases", userAgent, optionalGithubToken, DateTime.MinValue);
                return (newData, entries);
            }
            catch (Exception)
            {
                return (false, new GithubObjects.GithubReleaseEntry[0]);
            }

        }
        public static GithubObjects.GithubAsset? GetMatchingAsset(GithubObjects.GithubReleaseEntry githubRelease, TargetFrameworkAttribute frameworkAttribute)
        {
            GithubObjects.GithubAsset? asset = null;
            if (githubRelease.Assets.Count(a => a.Name.EndsWith("zip")) == 1)
            {
                asset = githubRelease.Assets.First(a => a.Name.EndsWith("zip"));
                return asset;

            }
            if (frameworkAttribute.FrameworkName.EndsWith("4.7.1"))
            {
                asset = githubRelease.Assets
                    .FirstOrDefault(a => a.Name.Contains("471") || a.Name.Contains("471"));
            }
            else if (frameworkAttribute.FrameworkName.EndsWith("4.7.2"))
            {
                asset = githubRelease.Assets
                    .FirstOrDefault(a => a.Name.Contains("472") || a.Name.Contains("472"));
            }
            else if (frameworkAttribute.FrameworkName.EndsWith("4.8"))
            {
                asset = githubRelease.Assets
                    .FirstOrDefault(a => a.Name.Contains("48") || a.Name.Contains("48"));
            }
            else if (frameworkAttribute.FrameworkName.EndsWith("3.1"))
            {
                asset = githubRelease.Assets
                    .FirstOrDefault(a => a.Name.Contains("3.1") || a.Name.Contains("netcoreapp3.1"));
            }

            return asset;
        }


    }
}
