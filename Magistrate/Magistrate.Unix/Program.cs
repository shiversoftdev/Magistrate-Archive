using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Magistrate.Unix
{
    public sealed class Program
    {
        static ManualResetEvent QuitEvent = new ManualResetEvent(false);
        [STAThread]
        static void Main(string[] args)
        {
                      
#if DEBUG
            Dev();
            Console.CancelKeyPress += (sender, eArgs) => {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };
#else
            // TODO
#endif
            QuitEvent.WaitOne();
        }

#if DEBUG
        private static void Dev()
        {
            Generated.Engine engine = new Generated.Engine();
            engine.BeginScoring();
        }
#endif
    }
}
