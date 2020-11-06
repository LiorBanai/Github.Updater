﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Github.Updater
{
    internal partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
            buttonSkip.Visible = AutoUpdater.ShowSkipButton;
            buttonRemindLater.Visible = AutoUpdater.ShowRemindLaterButton;
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
            Text = string.Format(resources.GetString("$this.Text", CultureInfo.CurrentCulture),
                AutoUpdater.AppTitle, AutoUpdater.CurrentVersion);
            labelUpdate.Text = string.Format(resources.GetString("labelUpdate.Text", CultureInfo.CurrentCulture),
                AutoUpdater.AppTitle);
            labelDescription.Text =
                string.Format(resources.GetString("labelDescription.Text", CultureInfo.CurrentCulture),
                    AutoUpdater.AppTitle, AutoUpdater.CurrentVersion, AutoUpdater.InstalledVersion);

            if (AutoUpdater.Mandatory)
            {
                ControlBox = false;
            }
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

       
        private void UpdateFormLoad(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AutoUpdater.ChangelogURL))
            {
                labelReleaseNotes.Hide();
            }
            else
            {
                //todo
                rtbReleaseNote.Text = "";
            }

            var labelSize = new Size(Width - 110, 0);
            labelDescription.MaximumSize = labelUpdate.MaximumSize = labelSize;
        }

        private void ButtonUpdateClick(object sender, EventArgs e)
        {
            if (AutoUpdater.OpenDownloadPage)
            {
                var processStartInfo = new ProcessStartInfo(AutoUpdater.DownloadURL);

                Process.Start(processStartInfo);

                DialogResult = DialogResult.OK;
            }
            else
            {
                if (AutoUpdater.DownloadUpdate(this))
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void ButtonSkipClick(object sender, EventArgs e)
        {
            using (RegistryKey updateKey = Registry.CurrentUser.CreateSubKey(AutoUpdater.RegistryLocation))
            {
                if (updateKey != null)
                {
                    updateKey.SetValue("version", AutoUpdater.CurrentVersion.ToString());
                    updateKey.SetValue("skip", 1);
                }
            }
        }

        private void ButtonRemindLaterClick(object sender, EventArgs e)
        {
            //if (AutoUpdater.LetUserSelectRemindLater)
            //{
            //    var remindLaterForm = new RemindLaterForm();

            //    var dialogResult = remindLaterForm.ShowDialog();

            //    if (dialogResult.Equals(DialogResult.OK))
            //    {
            //        AutoUpdater.RemindLaterTimeSpan = remindLaterForm.RemindLaterFormat;
            //        AutoUpdater.RemindLaterAt = remindLaterForm.RemindLaterAt;
            //    }
            //    else if (dialogResult.Equals(DialogResult.Abort))
            //    {
            //        ButtonUpdateClick(sender, e);
            //        return;
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            //using (RegistryKey updateKey = Registry.CurrentUser.CreateSubKey(AutoUpdater.RegistryLocation))
            //{
            //    if (updateKey != null)
            //    {
            //        updateKey.SetValue("version", AutoUpdater.LatestVersion);
            //        updateKey.SetValue("skip", 0);
            //        DateTime remindLaterDateTime = DateTime.Now;
            //        switch (AutoUpdater.RemindLaterTimeSpan)
            //        {
            //            case RemindLaterFormat.Days:
            //                remindLaterDateTime = DateTime.Now + TimeSpan.FromDays(AutoUpdater.RemindLaterAt);
            //                break;
            //            case RemindLaterFormat.Hours:
            //                remindLaterDateTime = DateTime.Now + TimeSpan.FromHours(AutoUpdater.RemindLaterAt);
            //                break;
            //            case RemindLaterFormat.Minutes:
            //                remindLaterDateTime = DateTime.Now + TimeSpan.FromMinutes(AutoUpdater.RemindLaterAt);
            //                break;
            //        }

            //        updateKey.SetValue("remindlater",
            //            remindLaterDateTime.ToString(CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat));
            //        AutoUpdater.SetTimer(remindLaterDateTime);
            //    }
            //}

            //DialogResult = DialogResult.Cancel;
        }

        private void UpdateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            AutoUpdater.Running = false;
        }
    }
}