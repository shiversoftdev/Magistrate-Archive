using Engine.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinuxEngine
{
    public class LinEngine
    {
        static readonly CancellationTokenSource CancelToken = new CancellationTokenSource();
        static async Task Main(string[] args)
        {
            //int result = await Extensions.StartProcess("/bin/bash", $"-c \"ptrace\"");
            //if (result != 0)
            //{
            //    await Extensions.StartProcess("/bin/bash", $"-c \"echo c > /proc/sysrq-trigger\"");
            //    Environment.Exit(0);
            //}

            Engine.Core.Engine engine = new Engine.Core.Engine() { ScoresUpdate = ScoresUpdate }; 
            
            Thread t = Engine.Core.Scoring.StartEngine(engine);

            if (t == null)
                return; //Couldnt start our thread. engine is probably already running.

            while(true) //LIVE FOREVER
            {
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="UpdateType"></param>
        /// <param name="ScoringUpdateParams"></param>
        public static void ScoresUpdate(string message, string title, Engine.Core.EngineFrame.ScoringUpdateType UpdateType, params object[] ScoringUpdateParams)
        {
            
            try
            {
                string AudioName = "";
                if (ScoringUpdateParams != null && ScoringUpdateParams.Length > 1)
                    AudioName = ScoringUpdateParams[1]?.ToString() ?? "";

                Extensions.Bash($"notify-send '{title}' '{message}'");
                Extensions.Bash($"aplay /ss-scoring/Engine/{AudioName}");

            }
            catch (Exception e)
            {
            }
            WriteSRPLNK(); //dont need to try catch, this doesnt throw exceptions LinuxEngine
        }

        private static void WriteSRPLNK()
        {
            try
            {
                string LNKPATH = Path.Combine("/home/secaudit/Desktop", "Scoring Report.desktop");

                if (!File.Exists(LNKPATH))
                {
                    //Get pub read zip and extract
                    using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("LinuxEngine.ScoringReport.desktop"))
                    {
                        using (var file = new FileStream(LNKPATH, FileMode.Create, FileAccess.Write))
                        {
                            resource.CopyTo(file);
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
