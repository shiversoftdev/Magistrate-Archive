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

namespace WindowsOfflineForensics
{
    public partial class MainForm : Form
    {

        private int ForensicsIndex;
        List<OfflineAnswerBox> answers = new List<OfflineAnswerBox>();
        private int NumAnswers;
        private string GetLoc
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }
        public MainForm(string[] args) //forensicsIndex
        {
            InitializeComponent();
            Icon = Properties.Resources.santhonk;
            if (args.Length < 1)
            {
                MessageBox.Show("This application was not configured correctly. Please contact the image creators for more help.");
                Environment.Exit(0);
            }
            try
            {
                ForensicsIndex = Convert.ToInt32(args[0]);
                string[] lines = File.ReadAllLines(Path.Combine(GetLoc, string.Format("CSSE_FRNSC_{0}.txt", ForensicsIndex)));
                if (lines.Length < 2)
                    throw new ArgumentException();

                NumAnswers = Convert.ToInt32(lines[0].Trim());

                string FinalText = "";
                for(int i = 1; i < lines.Length; i++)
                {
                    FinalText += lines[i] + "\r\n";
                }

                int VOffset = 505;
                for(int i = 0; i < NumAnswers; i++)
                {
                    OfflineAnswerBox ansr = new OfflineAnswerBox();
                    answers.Add(ansr);
                    Controls.Add(ansr);
                    ansr.Location = new Point(10, VOffset += 40);
                    Size = new Size(Size.Width, Size.Height + 40);
                }

                ForensicsBox.Text = FinalText;
                MouseDown += Form1_MouseDown;
            }
            catch
            {
                MessageBox.Show("This application was not configured correctly. Please contact the image creators for more help.");
                Environment.Exit(0);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void AnswerBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Submit_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Path.Combine(GetLoc, string.Format("CSSE_ANSWR_{0}.txt", ForensicsIndex)), GetAllAnswers());
            MessageBox.Show("Submitted!");
            Environment.Exit(0);
        }

        private string GetAllAnswers()
        {
            string result = "";

            foreach(var a in answers)
            {
                result += a.GetText().Trim().ToLower() + "+";
            }

            if(result.Length > 0)
                result = result.Substring(0, result.Length - 1);

            return result;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
