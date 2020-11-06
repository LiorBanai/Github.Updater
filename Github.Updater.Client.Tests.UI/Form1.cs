using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Github.Updater.Client.Tests.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
          
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSoundPath.Text = dlg.SelectedPath;
            }
        }

        private async void btnDownloadUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSoundPath.Text) && Directory.Exists(txtSoundPath.Text))
            {
                var updateFolder = txtSoundPath.Text;
                Action<string> downloadSpeed = (string s) => {UpdateText(lblDownloadSpeed,s); };
                Action<string> downloadProgress = (string s) => { UpdateText(lblDownloadProgress, s); };
                Action<int> downloadProgressValue = (int v) => {UpdateProgress(v); };
                var client = new Updater(updateFolder, "", downloadSpeed, downloadProgress, downloadProgressValue);
                await client.DownloadUpdaterIfNeeded((s)=>UpdateText(tsslabel,s));
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

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSoundPath.Text = Environment.CurrentDirectory;
        }
    }
}
