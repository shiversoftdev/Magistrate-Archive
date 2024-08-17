namespace WindowsInstaller
{
    partial class Installer
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.WelcomeLabel = new System.Windows.Forms.Label();
            this.IntroTimer = new System.Windows.Forms.Timer(this.components);
            this.UniqueID = new System.Windows.Forms.MaskedTextBox();
            this.StatusMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1, 1);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // WelcomeLabel
            // 
            this.WelcomeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(48)))));
            this.WelcomeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WelcomeLabel.Font = new System.Drawing.Font("Segoe UI Semilight", 48F);
            this.WelcomeLabel.ForeColor = System.Drawing.Color.White;
            this.WelcomeLabel.Location = new System.Drawing.Point(0, 0);
            this.WelcomeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.WelcomeLabel.Name = "WelcomeLabel";
            this.WelcomeLabel.Size = new System.Drawing.Size(1280, 720);
            this.WelcomeLabel.TabIndex = 1;
            this.WelcomeLabel.Text = "Welcome";
            this.WelcomeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IntroTimer
            // 
            this.IntroTimer.Interval = 2000;
            // 
            // UniqueID
            // 
            this.UniqueID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UniqueID.AsciiOnly = true;
            this.UniqueID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(48)))));
            this.UniqueID.BeepOnError = true;
            this.UniqueID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UniqueID.Font = new System.Drawing.Font("Segoe UI Semilight", 24F);
            this.UniqueID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(48)))));
            this.UniqueID.Location = new System.Drawing.Point(0, 432);
            this.UniqueID.Mask = "00AA-AAAA-AAAA-AAAA";
            this.UniqueID.Name = "UniqueID";
            this.UniqueID.ResetOnSpace = false;
            this.UniqueID.Size = new System.Drawing.Size(1280, 43);
            this.UniqueID.TabIndex = 3;
            this.UniqueID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UniqueID.Visible = false;
            this.UniqueID.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.UniqueID_MaskInputRejected);
            // 
            // StatusMessage
            // 
            this.StatusMessage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.StatusMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(48)))));
            this.StatusMessage.Font = new System.Drawing.Font("Segoe UI Semilight", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusMessage.ForeColor = System.Drawing.Color.LightCoral;
            this.StatusMessage.Location = new System.Drawing.Point(0, 479);
            this.StatusMessage.Margin = new System.Windows.Forms.Padding(0);
            this.StatusMessage.Name = "StatusMessage";
            this.StatusMessage.Size = new System.Drawing.Size(1280, 44);
            this.StatusMessage.TabIndex = 4;
            this.StatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Installer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.ControlBox = false;
            this.Controls.Add(this.StatusMessage);
            this.Controls.Add(this.UniqueID);
            this.Controls.Add(this.WelcomeLabel);
            this.Controls.Add(this.button1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Installer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label WelcomeLabel;
        private System.Windows.Forms.Timer IntroTimer;
        private System.Windows.Forms.MaskedTextBox UniqueID;
        private System.Windows.Forms.Label StatusMessage;
    }
}

