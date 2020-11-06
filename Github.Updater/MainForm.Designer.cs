
namespace Github.Updater
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStartAnalogy = new System.Windows.Forms.Button();
            this.lblTitleValue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(658, 46);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(130, 40);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnStartAnalogy
            // 
            this.btnStartAnalogy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartAnalogy.Location = new System.Drawing.Point(522, 46);
            this.btnStartAnalogy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStartAnalogy.Name = "btnStartAnalogy";
            this.btnStartAnalogy.Size = new System.Drawing.Size(130, 40);
            this.btnStartAnalogy.TabIndex = 7;
            this.btnStartAnalogy.Text = "Start Application";
            this.btnStartAnalogy.UseVisualStyleBackColor = true;
            // 
            // lblTitleValue
            // 
            this.lblTitleValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitleValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTitleValue.Location = new System.Drawing.Point(231, 9);
            this.lblTitleValue.Name = "lblTitleValue";
            this.lblTitleValue.Size = new System.Drawing.Size(557, 26);
            this.lblTitleValue.TabIndex = 6;
            this.lblTitleValue.Text = "N/A";
            this.lblTitleValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 26);
            this.label1.TabIndex = 5;
            this.label1.Text = "Downloaded Component:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 97);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStartAnalogy);
            this.Controls.Add(this.lblTitleValue);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Github Updater";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStartAnalogy;
        private System.Windows.Forms.Label lblTitleValue;
        private System.Windows.Forms.Label label1;
    }
}