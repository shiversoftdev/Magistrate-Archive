namespace SSWinNotify
{
    partial class NotifyMain
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
            this.NotifyRTB = new SSWinNotify.NotifyMain.NotifyRichTextBox();
            this.hacklabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NotifyRTB
            // 
            this.NotifyRTB.AccessibleRole = System.Windows.Forms.AccessibleRole.Alert;
            this.NotifyRTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.NotifyRTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NotifyRTB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NotifyRTB.DetectUrls = false;
            this.NotifyRTB.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NotifyRTB.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.NotifyRTB.Location = new System.Drawing.Point(14, 12);
            this.NotifyRTB.MaximumSize = new System.Drawing.Size(376, 1200);
            this.NotifyRTB.Name = "NotifyRTB";
            this.NotifyRTB.ReadOnly = true;
            this.NotifyRTB.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.NotifyRTB.ShortcutsEnabled = false;
            this.NotifyRTB.Size = new System.Drawing.Size(376, 100);
            this.NotifyRTB.TabIndex = 0;
            this.NotifyRTB.Text = "Notification Title\nNotification Description\n";
            this.NotifyRTB.TextChanged += new System.EventHandler(this.NotifyRTB_TextChanged);
            // 
            // hacklabel
            // 
            this.hacklabel.AutoSize = true;
            this.hacklabel.Location = new System.Drawing.Point(0, 0);
            this.hacklabel.Name = "hacklabel";
            this.hacklabel.Size = new System.Drawing.Size(0, 15);
            this.hacklabel.TabIndex = 1;
            // 
            // NotifyMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(400, 120);
            this.ControlBox = false;
            this.Controls.Add(this.hacklabel);
            this.Controls.Add(this.NotifyRTB);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(400, 1200);
            this.MinimumSize = new System.Drawing.Size(400, 120);
            this.Name = "NotifyMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Notify";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NotifyRichTextBox NotifyRTB;
        private System.Windows.Forms.Label hacklabel;
    }
}

