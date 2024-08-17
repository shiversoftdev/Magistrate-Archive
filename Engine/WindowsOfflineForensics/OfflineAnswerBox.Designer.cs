namespace WindowsOfflineForensics
{
    partial class OfflineAnswerBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AnswerBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // AnswerBox
            // 
            this.AnswerBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(96)))), ((int)(((byte)(24)))));
            this.AnswerBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AnswerBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AnswerBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.AnswerBox.Location = new System.Drawing.Point(0, 0);
            this.AnswerBox.Margin = new System.Windows.Forms.Padding(0);
            this.AnswerBox.Name = "AnswerBox";
            this.AnswerBox.Size = new System.Drawing.Size(680, 30);
            this.AnswerBox.TabIndex = 3;
            // 
            // OfflineAnswerBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AnswerBox);
            this.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "OfflineAnswerBox";
            this.Size = new System.Drawing.Size(680, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AnswerBox;
    }
}
