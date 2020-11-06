using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Github.Updater
{

    public partial class MainForm : Form
    {
        private string Title { get; }
        private string DownloadURL { get; }
        private string TargetFolder { get; }
        private string ApplicationToRunPostUpdate { get; set; }
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string title, string downloadURL, string targetFolder, string applicationToRun) : this()
        {
            ApplicationToRunPostUpdate = applicationToRun;
            Title = title;
            DownloadURL = downloadURL;
            TargetFolder = targetFolder;
            if (!Directory.Exists(TargetFolder))
            {
                Directory.CreateDirectory(TargetFolder);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (DownloadURL != null)
            {
                lblTitleValue.Text = Title;
                AutoUpdater.DownloadURL = DownloadURL;
                AutoUpdater.DownloadPath = TargetFolder;
                if (AutoUpdater.DownloadUpdate(this))
                {
                    DialogResult = DialogResult.OK;
                    btnStartAnalogy.Visible = !string.IsNullOrEmpty(ApplicationToRunPostUpdate) &&
                                              File.Exists(ApplicationToRunPostUpdate);

                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnStartAnalogy_Click(object sender, EventArgs e)
        {
            Process.Start(ApplicationToRunPostUpdate);
        }


    }
}
