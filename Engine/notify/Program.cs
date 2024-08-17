using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSWinNotify
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string title = args.Length > 0 ? args[0] : "CSSE Notification";
            string body = args.Length > 1 ? args[1] : "You gained points!";
            float duration = 4;
            if(args.Length > 2)
            {
                try
                {
                    duration = (float)Convert.ToDecimal(args[2]);
                }
                catch
                {
                    //dont care, we already have a default
                }
            }
            string ActionString = args.Length > 3 ? args[3] : "http://www.shiversoft.net";
            string SoundString = args.Length > 4 ? args[4] : "";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NotifyMain(title, body, duration, ActionString, SoundString));
        }
    }
}
