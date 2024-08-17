using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

//Link dump
// kernel32 dep policy https://docs.microsoft.com/en-us/windows/desktop/api/winbase/nf-winbase-getsystemdeppolicy
//https://retep998.github.io/doc/kernel32/fn.GetSystemDEPPolicy.html
//https://en.wikipedia.org/wiki/Service_Control_Manager


namespace TestingGUI
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
#if DEBUG
            Engine.Core.Engine engine = new Engine.Core.Engine();
            Engine.Core.Scoring.StartEngine(engine);

            MouseDown += Form1_MouseDown1;
            Location = new Point(0,0);
            TestTimer.Start();
#endif
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = FormatMD5(MD5F32(textBox1.Text));
        }

        private string FormatMD5(byte[] data)
        {
            string result = "";
            foreach (byte b in data)
                result += (b < 0x10 ? "0" : "") + b.ToString("X");
            return result;
        }

        private MD5 __MD5__ = System.Security.Cryptography.MD5.Create();

        private byte[] MD5F32(string content)
        {
            byte[] data = __MD5__.ComputeHash(System.Text.Encoding.ASCII.GetBytes(content));
            byte[] final = new byte[4];

            for (int i = 0; i < 4; i++) //Fold md5 using addition. Could also xor.
            {
                for (int j = 0; j < 4; j++)
                {
                    final[i] += data[i * 4 + j];
                }
            }

            return final;
        }

        private string GroupEQString(string[] users)
        {
            int MaxSize = 0;

            foreach (var s in users)
                MaxSize = Math.Max(s.Length, MaxSize);
            char[] ResultantString = new char[MaxSize];

            for (int i = 0; i < MaxSize; i++)
                ResultantString[i] = '?';

            foreach (string s in users)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    ResultantString[i] = (char)(ResultantString[i] ^ s[i]);
                }
            }

            string data = new string(ResultantString);
            return data;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var FD = new OpenFileDialog();

            if (FD.ShowDialog() != DialogResult.OK)
                return;

            if (!File.Exists(FD.FileName))
                return;

            FIleMD5Box.Text = FormatMD5(__MD5__.ComputeHash(File.ReadAllBytes(FD.FileName)));
            FileSizeBox.Text = new FileInfo(FD.FileName).Length.ToString();
        }

        private void GroupEQIn_TextChanged(object sender, EventArgs e)
        {
            GroupEQOut.Text = GroupEQString(GroupEQIn.Text.Split(','));
        }

        bool CaseSensitive = false;

        private string ForensicsMask(string[] lines)
        {
            List<char> byteronis = new List<char>();
            foreach (string line in lines)
            {
                string tline = Regex.Replace(line.Trim().ToLower(), "\\s+", "");
                if (tline.StartsWith("answer:", StringComparison.InvariantCultureIgnoreCase))
                {
                    string rline = line.Trim();
                    if (!CaseSensitive)
                        rline = rline.ToLower();

                    rline = Regex.Replace(rline, "^[Aa][Nn][Ss][Ww][Ee][Rr][\\s]*:[\\s]*", "");
                    for (int i = 0; i < rline.Length; i++)
                    {
                        if (byteronis.Count <= i)
                            byteronis.Add(rline[i]);
                        else
                            byteronis[i] = (char)(byteronis[i] ^ rline[i]);
                    }
                }
            }
            return new string(byteronis.ToArray());
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        string ForensicsFileName = "";
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var FD = new OpenFileDialog();

            if (FD.ShowDialog() != DialogResult.OK)
                return;

            ForensicsFileName = FD.FileName;


            UpdateForensicsBox();
        }

        private void UpdateForensicsBox()
        {
            if (!File.Exists(ForensicsFileName))
            {
                ForenTB.Text = "NO FILE";
                return;
            }
            ForenTB.Text = FormatMD5(MD5F32(ForensicsMask(File.ReadAllLines(ForensicsFileName))));
        }

        private void IsCSCheck_CheckedChanged(object sender, EventArgs e)
        {
            CaseSensitive = IsCSCheck.Checked;
            UpdateForensicsBox();
        }
    }
}
