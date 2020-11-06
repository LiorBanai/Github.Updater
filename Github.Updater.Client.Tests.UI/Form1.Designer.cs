
namespace Github.Updater.Client.Tests.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnFolder = new System.Windows.Forms.Button();
            this.txtSoundPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblDownloadSpeed = new System.Windows.Forms.Label();
            this.btnDownloadUpdate = new System.Windows.Forms.Button();
            this.lblDownloadProgress = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFolder
            // 
            this.btnFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolder.Location = new System.Drawing.Point(903, 26);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(32, 27);
            this.btnFolder.TabIndex = 33;
            this.btnFolder.Text = "..";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // txtSoundPath
            // 
            this.txtSoundPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSoundPath.Location = new System.Drawing.Point(216, 26);
            this.txtSoundPath.Name = "txtSoundPath";
            this.txtSoundPath.Size = new System.Drawing.Size(681, 27);
            this.txtSoundPath.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 20);
            this.label1.TabIndex = 34;
            this.label1.Text = "Download Updater to folder:";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 110);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(923, 33);
            this.progressBar1.TabIndex = 35;
            // 
            // lblDownloadSpeed
            // 
            this.lblDownloadSpeed.AutoSize = true;
            this.lblDownloadSpeed.Location = new System.Drawing.Point(6, 71);
            this.lblDownloadSpeed.Name = "lblDownloadSpeed";
            this.lblDownloadSpeed.Size = new System.Drawing.Size(161, 20);
            this.lblDownloadSpeed.TabIndex = 36;
            this.lblDownloadSpeed.Text = "Downloading Update...";
            // 
            // btnDownloadUpdate
            // 
            this.btnDownloadUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadUpdate.Location = new System.Drawing.Point(774, 64);
            this.btnDownloadUpdate.Name = "btnDownloadUpdate";
            this.btnDownloadUpdate.Size = new System.Drawing.Size(161, 34);
            this.btnDownloadUpdate.TabIndex = 37;
            this.btnDownloadUpdate.Text = "Download Update";
            this.btnDownloadUpdate.UseVisualStyleBackColor = true;
            this.btnDownloadUpdate.Click += new System.EventHandler(this.btnDownloadUpdate_Click);
            // 
            // lblDownloadProgress
            // 
            this.lblDownloadProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDownloadProgress.Location = new System.Drawing.Point(506, 71);
            this.lblDownloadProgress.Name = "lblDownloadProgress";
            this.lblDownloadProgress.Size = new System.Drawing.Size(262, 20);
            this.lblDownloadProgress.TabIndex = 38;
            this.lblDownloadProgress.Text = "Downloading Update...";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 149);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(956, 26);
            this.statusStrip1.TabIndex = 39;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslabel
            // 
            this.tsslabel.Name = "tsslabel";
            this.tsslabel.Size = new System.Drawing.Size(36, 20);
            this.tsslabel.Text = "N/A";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.txtSoundPath);
            this.groupBox1.Controls.Add(this.lblDownloadProgress);
            this.groupBox1.Controls.Add(this.btnFolder);
            this.groupBox1.Controls.Add(this.btnDownloadUpdate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblDownloadSpeed);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(956, 149);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Updater Test";
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(956, 207);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Test Application";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 382);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Example of client usage";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.TextBox txtSoundPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblDownloadSpeed;
        private System.Windows.Forms.Button btnDownloadUpdate;
        private System.Windows.Forms.Label lblDownloadProgress;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

