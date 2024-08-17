namespace TestingGUI
{
    partial class Form1
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
            this.TestTimer = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.Sourcelbl = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.FIleMD5Box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FileSizeBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GroupEQIn = new System.Windows.Forms.TextBox();
            this.GroupEQOut = new System.Windows.Forms.TextBox();
            this.IsCSCheck = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ForenTB = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TestTimer
            // 
            this.TestTimer.Interval = 1;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(460, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Sourcelbl
            // 
            this.Sourcelbl.AutoSize = true;
            this.Sourcelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sourcelbl.Location = new System.Drawing.Point(12, 16);
            this.Sourcelbl.Name = "Sourcelbl";
            this.Sourcelbl.Size = new System.Drawing.Size(56, 24);
            this.Sourcelbl.TabIndex = 4;
            this.Sourcelbl.Text = "Input:";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Silver;
            this.textBox1.Location = new System.Drawing.Point(267, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(190, 29);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.Silver;
            this.textBox2.Location = new System.Drawing.Point(267, 49);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(190, 29);
            this.textBox2.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "PrepareState32";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(460, 84);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(28, 29);
            this.button2.TabIndex = 8;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FIleMD5Box
            // 
            this.FIleMD5Box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.FIleMD5Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FIleMD5Box.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FIleMD5Box.ForeColor = System.Drawing.Color.Silver;
            this.FIleMD5Box.Location = new System.Drawing.Point(267, 84);
            this.FIleMD5Box.Name = "FIleMD5Box";
            this.FIleMD5Box.ReadOnly = true;
            this.FIleMD5Box.Size = new System.Drawing.Size(190, 29);
            this.FIleMD5Box.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 24);
            this.label1.TabIndex = 10;
            this.label1.Text = "File MD5";
            // 
            // FileSizeBox
            // 
            this.FileSizeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.FileSizeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FileSizeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileSizeBox.ForeColor = System.Drawing.Color.Silver;
            this.FileSizeBox.Location = new System.Drawing.Point(267, 119);
            this.FileSizeBox.Name = "FileSizeBox";
            this.FileSizeBox.ReadOnly = true;
            this.FileSizeBox.Size = new System.Drawing.Size(190, 29);
            this.FileSizeBox.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "File Size";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 24);
            this.label4.TabIndex = 14;
            this.label4.Text = "GroupEQ";
            // 
            // GroupEQIn
            // 
            this.GroupEQIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.GroupEQIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GroupEQIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupEQIn.ForeColor = System.Drawing.Color.Silver;
            this.GroupEQIn.Location = new System.Drawing.Point(267, 154);
            this.GroupEQIn.Name = "GroupEQIn";
            this.GroupEQIn.Size = new System.Drawing.Size(190, 29);
            this.GroupEQIn.TabIndex = 13;
            this.GroupEQIn.TextChanged += new System.EventHandler(this.GroupEQIn_TextChanged);
            // 
            // GroupEQOut
            // 
            this.GroupEQOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.GroupEQOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GroupEQOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupEQOut.ForeColor = System.Drawing.Color.Silver;
            this.GroupEQOut.Location = new System.Drawing.Point(267, 189);
            this.GroupEQOut.Name = "GroupEQOut";
            this.GroupEQOut.ReadOnly = true;
            this.GroupEQOut.Size = new System.Drawing.Size(190, 29);
            this.GroupEQOut.TabIndex = 15;
            // 
            // IsCSCheck
            // 
            this.IsCSCheck.AutoSize = true;
            this.IsCSCheck.Location = new System.Drawing.Point(267, 259);
            this.IsCSCheck.Name = "IsCSCheck";
            this.IsCSCheck.Size = new System.Drawing.Size(96, 17);
            this.IsCSCheck.TabIndex = 17;
            this.IsCSCheck.Text = "Case Sensitive";
            this.IsCSCheck.UseVisualStyleBackColor = true;
            this.IsCSCheck.CheckedChanged += new System.EventHandler(this.IsCSCheck_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(178, 24);
            this.label5.TabIndex = 19;
            this.label5.Text = "Forensics Formatter";
            // 
            // ForenTB
            // 
            this.ForenTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ForenTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ForenTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForenTB.ForeColor = System.Drawing.Color.Silver;
            this.ForenTB.Location = new System.Drawing.Point(267, 224);
            this.ForenTB.Name = "ForenTB";
            this.ForenTB.ReadOnly = true;
            this.ForenTB.Size = new System.Drawing.Size(190, 29);
            this.ForenTB.TabIndex = 20;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(460, 224);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(28, 29);
            this.button3.TabIndex = 21;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ForenTB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.IsCSCheck);
            this.Controls.Add(this.GroupEQOut);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.GroupEQIn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FileSizeBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FIleMD5Box);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Sourcelbl);
            this.Controls.Add(this.button1);
            this.ForeColor = System.Drawing.Color.Silver;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TestGUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer TestTimer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label Sourcelbl;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox FIleMD5Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FileSizeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox GroupEQIn;
        private System.Windows.Forms.TextBox GroupEQOut;
        private System.Windows.Forms.CheckBox IsCSCheck;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ForenTB;
        private System.Windows.Forms.Button button3;
    }
}

