using System;

namespace Github.Updater
{
    public class Updater
    {
        private string ApplicationTitle { get; }
        private string DownloadURL { get; }
        private string TargetFolder { get; }

        public Updater(string applicationTitle, string downloadUrl, string targetFolder)
        {
            ApplicationTitle = applicationTitle;
            DownloadURL = downloadUrl;
            TargetFolder = targetFolder;
        }
   


        public void InitiateApplicationUpdate()
        {
        
        }
    }
}
