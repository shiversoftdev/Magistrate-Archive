using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace srpview
{
    public partial class Browser : Form
    {
        private string TargetURL = "https://www.bing.com";
        Panel MainPanel;
        WebBrowser MainBrowser;
        Label WindowTitle;
        Button WinExit, WinMin;

        private Control activeControl;
        private Point previousLocation;

        public Browser(string[] args)
        {
            if (args.Length > 0)
                TargetURL = args[0];

            InitializeComponent();
            InitalizeComponents();
            InitializeLayout();
            _Finalize();
        }

        private void InitalizeComponents()
        {
            MainPanel = new Panel()
            {
                Location = new System.Drawing.Point(0, 36),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top,
                Size = new System.Drawing.Size(512, 476)
            };

            MainBrowser = new WebBrowser()
            {
                Location = new System.Drawing.Point(0, 0),
                Dock = DockStyle.Fill,
            };

            WindowTitle = new Label()
            {
                Location = new System.Drawing.Point(4, 4),
                Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Bold),
                Text = "Your Progress",
                ForeColor = Color.FromArgb(224, 224, 245),
                AutoSize = true,

            };

            WinExit = new Button()
            {
                Size = new Size(28,28),
                Location = new Point(this.Size.Width - 28 - 4, 4),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackgroundImage = srpview.Properties.Resources.close,
            };

            WinMin = new Button()
            {
                Size = new Size(28, 28),
                Location = new Point(this.Size.Width - 28 - 4 - 28 - 4, 4),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackgroundImage = srpview.Properties.Resources.min,
            };
        }

        private void Browser_MouseUp(object sender, MouseEventArgs e)
        {
            activeControl = null;
            Cursor = Cursors.Default;
        }

        private void Browser_MouseMove(object sender, MouseEventArgs e)
        {
            var location = (sender as Control).Location;

            if (location.Y <= 1)
            {
                WindowState = FormWindowState.Maximized;
                return;
            }

            if (WindowState != FormWindowState.Normal)
                WindowState = FormWindowState.Normal;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Browser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void InitializeLayout()
        {
            Controls.Add(WindowTitle);
            Controls.Add(WinExit);
            Controls.Add(WinMin);

            MainPanel.Controls.Add(MainBrowser);
            Controls.Add(MainPanel);
        }


        private void _Finalize()
        {
            WinExit.FlatAppearance.BorderSize = 0;
            WinExit.Cursor = Cursors.Hand;

            WinMin.FlatAppearance.BorderSize = 0;
            WinMin.Cursor = Cursors.Hand;

            BackColor = Color.FromArgb(26, 64, 96);

            WindowState = FormWindowState.Maximized;

            MainBrowser.Navigate(new Uri(TargetURL));

            MouseDown += Browser_MouseDown;
            MouseMove += Browser_MouseMove;

            WindowTitle.MouseDown += WindowTitle_MouseDown;
            WindowTitle.MouseUp += Browser_MouseUp;
            WindowTitle.MouseMove += WindowTitle_MouseMove;

            WinExit.Click += WinExit_Click;
            WinMin.Click += WinMin_Click;
        }

        private void WinMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void WinExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void WindowTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (activeControl == null || activeControl != sender)
                return;

            var location = (sender as Control).Location;
            location.Offset(e.Location.X - previousLocation.X, e.Location.Y - previousLocation.Y);
            location.Offset(this.Location);

            if(location.Y <= 1)
            {
                WindowState = FormWindowState.Maximized;
                Browser_MouseUp(null, null);
                return;
            }

            this.Location = location;
        }

        private void WindowTitle_MouseDown(object sender, MouseEventArgs e)
        {
            activeControl = sender as Control;
            previousLocation = e.Location;
            Cursor = Cursors.Hand;
            WindowState = FormWindowState.Normal;
        }
    }
}
