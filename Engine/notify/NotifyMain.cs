using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// override OnEnabledChanged for RTB

namespace SSWinNotify
{
    public partial class NotifyMain : Form
    {
        private readonly string NotifyTitle;
        private readonly string NotifyBody;
        private readonly float Duration;
        private const int FadeDuration = 200;
        private const int SlideDuration = 150;
        private const int MoveYDuration = 150;
        private const int MoveYStepMS = 5;
        private int TargetY = 0;
        private bool IsClosing = false;
        private bool HasActed = false;
        private readonly string ActionString = "http://www.shiversoft.net";
        private readonly string SoundString = "";
        public NotifyMain(string title, string body, float duration, string actionstring, string soundstr)
        {
            Duration = duration;
            SoundString = soundstr;
            ActionString = actionstring;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            InitializeComponent();
            NotifyRTB.TabStop = false;
            NotifyRTB.Enter += NotifyRTB_Enter;
            NotifyRTB.ShortcutsEnabled = false;
            NotifyRTB.ShowSelectionMargin = false;
            NotifyTitle = title;
            NotifyBody = body;
            NotifyRTB.ContentsResized += NotifyRTB_ContentsResized;
            NotifyRTB.Text = title + "\n" + body;
            NotifyRTB.MouseClick += NotifyRTB_MouseClick;
            Format();
            NotifyRTB.SelectionChanged += NotifyRTB_SelectionChanged;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - Width, Screen.PrimaryScreen.WorkingArea.Bottom - Height - 20);
            Update();
            this.Animate(Extensions.Effect.Slide, SlideDuration, 180);
            GoFade += () => { this.Animate(Extensions.Effect.Blend, FadeDuration, 180); };
            DestroyControl += () => { IsClosing = true; this.Close(); };
            MoveYTo += (int y) => { this.Location = new Point(Location.X, y); };

            if(SoundString != null && SoundString.Length > 1)
            {
                string loc = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), SoundString);
                if (File.Exists(loc))//SoundString
                {
                    try
                    {
                        SoundPlayer simpleSound = new SoundPlayer(loc);
                        simpleSound.Play();
                    }
                    catch
                    {

                    }
                }
                
            }
            
            new Task(WaitAndFade).Start();
        }

        private delegate void AnimDelegate();
        private AnimDelegate GoFade;
        private AnimDelegate DestroyControl;
        private delegate void MoveYDelegate(int y);
        private MoveYDelegate MoveYTo;

        private async void WaitAndFade()
        {
            await Task.Delay((int)(Duration * 1000));
            AClose();
        }

        private async void AClose()
        {
            Invoke(GoFade);
            await Task.Delay(FadeDuration);
            Invoke(DestroyControl);
        }

        /// <summary>
        /// Automatically shift postion based on our calculated deltas
        /// </summary>
        public void Up()
        {
            MoveTo(Location.Y - Height - 20);
        }

        /// <summary>
        /// Move this notification to a specific Y position
        /// </summary>
        /// <param name="YPos"></param>
        public void MoveTo(int YPos)
        {
            if(!IsClosing)
            {
                TargetY = YPos;
                new Task(MoveYOverTime).Start();
            }
        }

        private async void MoveYOverTime()
        {
            int OldPos = this.Location.Y;
            for(int i = 0; i < MoveYDuration; i += MoveYStepMS)
            {
                Invoke(MoveYTo, (int)Lerp(OldPos, TargetY, (float)i / (float)MoveYDuration));
                await Task.Delay(MoveYStepMS);
            }
        }

        float Lerp(float a, float b, float alpha)
        {
            return a * (1 - alpha) + b * alpha;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private void NotifyRTB_SelectionChanged(object sender, EventArgs e)
        {
            NotifyRTB.SelectionLength = 0;
            hacklabel.Focus();
        }

        private void NotifyRTB_Enter(object sender, EventArgs e)
        {
            hacklabel.Focus();
        }

        private void NotifyRTB_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            var richTextBox = (RichTextBox)sender;
            richTextBox.Width = e.NewRectangle.Width;
            richTextBox.Height = e.NewRectangle.Height + 20;
            richTextBox.Width += richTextBox.Margin.Horizontal + SystemInformation.HorizontalResizeBorderThickness;
            richTextBox.Location = new Point(richTextBox.Location.X, (int)(this.Height / 2f) - (int)(richTextBox.Height / 2f));
        }

        private void NotifyRTB_MouseClick(object sender, MouseEventArgs e)
        {
            hacklabel.Focus();
            if (HasActed)
                return;
            HasActed = true;
            try
            {
                System.Diagnostics.Process.Start(ActionString);
            }
            catch { }
            new Task(AClose).Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SelectNotifyText(string target)
        {
            int index = NotifyRTB.Text.IndexOf(target);

            if (index < 0)
                return;

            NotifyRTB.Select(index, target.Length);
        }
        
        private void Format()
        {
            SelectNotifyText(NotifyTitle);
            FormatTitle();
            SelectNotifyText(NotifyBody);
            FormatBody();
            NotifyRTB.DeselectAll();
        }

        private void FormatTitle()
        {
            NotifyRTB.SelectionColor = Color.White;
            NotifyRTB.SelectionFont = new Font(NotifyRTB.Font, FontStyle.Bold);
        }

        private void FormatBody()
        {
            NotifyRTB.SelectionColor = Color.Silver;
            NotifyRTB.SelectionFont = new Font(NotifyRTB.Font, FontStyle.Regular);
        }

        private void NotifyRTB_TextChanged(object sender, EventArgs e)
        {

        }

        internal class NotifyRichTextBox : RichTextBox
        {
            
            protected override void OnEnabledChanged(EventArgs e)
            {
                
                //do nothing
            }
        }
    }
}
