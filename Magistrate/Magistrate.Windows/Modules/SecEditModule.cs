using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.Windows.Modules
{
    internal sealed class SecEditModule : BaseModule
    {
        private bool DebugVerbose = false;
        public SecEditModule() : base()
        {
            SetTickRate(50027);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string exportPath = Path.GetTempFileName();
            List<CheckState> states = new List<CheckState>();
            using (var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Arguments = $"/c secedit /export /cfg \"{exportPath}\" /quiet",
                    FileName = "cmd",
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = false,
                    WorkingDirectory = Environment.CurrentDirectory
                }
            })
            {
                try
                {
                    process.Start();
                    process.WaitForExit();

                    string[] Results = File.ReadAllLines(exportPath);
                    File.Delete(exportPath);

                    foreach (string line in Results)
                    {
                        string[] linesplit = line.Trim().Split('=');
                        if (linesplit.Length < 2) //weird exception or header
                            continue;

                        if (linesplit[0].Contains("[")) //section header, not a real key
                            continue;

                        string sreport = $"{linesplit[0].Trim().ToLower()}:{linesplit[1].Trim()}";

#if DEBUG
                        if (DebugVerbose)
                            Console.WriteLine(sreport);
#endif
                        states.Add(new CheckState(info.ComputeHash(sreport)));
                    }
                }
                catch(Exception e)
                {
#if DEBUG
                    if (DebugVerbose)
                        Console.WriteLine(e.ToString());
#endif
                    return info.Maintain();
                }
            }

            return states;
        }
    }
}
