using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Github.Updater.Client.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public string repoName = @"LiorBanai\Github.Updater";
        [TestMethod]
        public async Task TestGetCheckVersion()
        {
            var updateFolder = Environment.CurrentDirectory;
            Action<string> downloadSpeed = (string s) => { };
            Action<string> downloadProgress = (string s) => { }; 
            Action<int> downloadProgressValue = (int v) => { };
            var client = new Updater(updateFolder, "", downloadSpeed, downloadProgress, downloadProgressValue);
            var result = await client.CheckNewVersionExistsForUpdater();
        }

        [TestMethod]
        public async Task TestGetLatestUpdater()
        {
            var client = await Updater.GetLatestUpdater();
            Assert.IsTrue(client.HasValue && !string.IsNullOrEmpty(client.Value.TagName));
        }

        [TestMethod]
        public void TestGetMatchingAsset()
        {
        }
        
    }
}
