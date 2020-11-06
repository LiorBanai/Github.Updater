using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Github.Updater
{

    public partial class MainForm : Form
    {
        private string Title { get; }
        private string DownloadURL { get; }
        private string TargetFolder { get; }
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string title, string downloadURL, string targetFolder) : this()
        {
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

                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnStartAnalogy_Click(object sender, EventArgs e)
        {

        }
    }
}
