using System;
using System.IO;
using System.Windows.Forms;

namespace Github.Updater.Client.Tests.UI
{
    public partial class Form1 : Form
    {
        private Updater Updater { get; set; }

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txtbUpdaterFolder.Text = Path.Combine(Environment.CurrentDirectory,"updater");
            txtbApplicationFolder.Text = Path.Combine(Environment.CurrentDirectory, "Application");
            CreateUpdater();
        }
        private void btnFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtbUpdaterFolder.Text = dlg.SelectedPath;
            }
        }

        private async void btnDownloadUpdate_Click(object sender, EventArgs e)
        {
            CreateUpdater();
            await Updater.DownloadUpdaterIfNeeded((s) => UpdateText(tsslabel, s));
        }

        private void CreateUpdater()
        {
            if (!string.IsNullOrEmpty(txtbUpdaterFolder.Text))
            {
                if (!Directory.Exists(txtbUpdaterFolder.Text))
                {
                    Directory.CreateDirectory(txtbUpdaterFolder.Text);
                }
                var updateFolder = txtbUpdaterFolder.Text;
                Action<string> downloadSpeed = (string s) => { UpdateText(lblDownloadSpeed, s); };
                Action<string> downloadProgress = (string s) => { UpdateText(lblDownloadProgress, s); };
                Action<int> downloadProgressValue = (int v) => { UpdateProgress(v); };
                Updater = new Updater(txtbRepoName.Text, txtRepo.Text, updateFolder, "", downloadSpeed,
                    downloadProgress, downloadProgressValue);
            }
        }
        private void UpdateText(Label label, string text)
        {
            BeginInvoke(new MethodInvoker(() => { label.Text = text; }));
        }

        private void UpdateText(ToolStripStatusLabel label, string text)
        {
            BeginInvoke(new MethodInvoker(() => { label.Text = text; }));
        }

        private void UpdateProgress(int value)
        {
            BeginInvoke(new MethodInvoker(() => { progressBar1.Value = value; }));

        }



        private async void btnDownloadApplication_Click(object sender, EventArgs e)
        {
            if (Updater != null && !string.IsNullOrEmpty(txtbUpdaterFolder.Text))
            {
                var currentVersion=new Version(4,2,9);
                var newVersion = await Updater.CheckNewVersionExistsForApplication(currentVersion);
                if (newVersion)
                {
                    await Updater.DownloadUpdaterIfNeeded((s) => UpdateText(tsslabel, s));
                    await Updater.StartApplicationUpdateProcess(txtbApplicationFolder.Text,"net472.zip");
                }

            }
        }
    }
}