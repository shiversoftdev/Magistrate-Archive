using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Language;
using System.Text;
using System.Threading.Tasks;
using Trinet.Core.IO.Ntfs;

namespace Magistrate.Windows.Modules
{
    internal sealed class FSDModule : BaseModule
    {
        private Dictionary<CheckInfo, SortedSet<ulong>> FSData = new Dictionary<CheckInfo, SortedSet<ulong>>();
        private SortedSet<ulong> CFSD = new SortedSet<ulong>();
        private static bool DebugVerbose = false;
        public FSDModule() : base()
        {
            SetTickRate(35000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            if(!FSData.ContainsKey(info))
            {
                string fpath = info.GetArgument(0)?.ToString();
                if (fpath == null || !File.Exists(fpath)) return SingleState(CheckInfo.DEFAULT);

                FSData[info] = new SortedSet<ulong>();
                byte[] fsd_data = File.ReadAllBytes(fpath);

                for(int i = 0; i < fsd_data.Length; i += 8)
                {
                    if (i + 8 > fsd_data.Length)
                        break;

                    FSData[info].Add(BitConverter.ToUInt64(fsd_data, i));
                }
            }
            try
            {
                string KeyPath = info.GetArgument(1)?.ToString();
                bool reportNew = Convert.ToBoolean(info.GetArgument(2)?.ToString().ToLower() ?? "false");

                if (KeyPath == null) return SingleState(CheckInfo.DEFAULT);

                List<CheckState> States = new List<CheckState>();

                string[] dirpaths = KeyPath.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

                CFSD = new SortedSet<ulong>();

                foreach (string file in RelevantFiles(new HashSet<string>() { Path.GetPathRoot(KeyPath) }, dirpaths, 0))
                {
#if DEBUG
                    if (DebugVerbose) Console.WriteLine(file);
#endif
                    CFSD.Add(CheckState.FastStringHash(file));
                }

                foreach (ulong old in FSData[info])
                {
                    if (!CFSD.Contains(old)) States.Add(new CheckState(info.ComputeHash(old.ToString())));
                }

                if (reportNew)
                {
                    foreach (ulong _new in CFSD)
                    {
                        if (!FSData[info].Contains(_new)) States.Add(new CheckState(info.ComputeHash(_new.ToString())));
                    }
                }

                return States;
            }
            catch(Exception e)
            {
#if DEBUG
                if (DebugVerbose) Console.WriteLine(e.ToString());
#endif
            }
            return info.Maintain();
        }

        private HashSet<string> RelevantFiles(HashSet<string> roots, string[] paths, int index)
        {
            if (index >= paths.Length)
                return roots;

            HashSet<string> files = new HashSet<string>();

            string cpath = paths[index];

            foreach(var file in roots)
            {
                if (File.Exists(file) || file.LastIndexOf(':') > 3)
                {
                    files.Add(file);
                    continue;
                }
                
                if (!Directory.Exists(file))
                    continue;

                if(cpath == "*")
                {
                    try
                    {
                        foreach (var fi in Directory.EnumerateFiles(file))
                        {
                            files.Add(fi.ToLower());
                            try
                            {
                                foreach (var altds in new FileInfo(fi).ListAlternateDataStreams())
                                {
                                    files.Add(altds.FullPath);
                                }
                            }
                            catch { }
                        }
                        foreach (var dir in Directory.EnumerateDirectories(file))
                        {
                            files.Add(dir.ToLower());
                        }
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        if (DebugVerbose) Console.WriteLine(e.ToString());
#endif
                    }
                    continue;
                }
                if (Directory.Exists(Path.Combine(file, cpath)))
                    files.Add(Path.Combine(file, cpath).ToLower());
                else if (File.Exists(Path.Combine(file, cpath)))
                    files.Add(Path.Combine(file, cpath).ToLower());
            }
            roots.Clear();
            foreach (string potential in RelevantFiles(files, paths, index + 1))
            {
                if(File.Exists(potential) || potential.LastIndexOf(':') > 3) roots.Add(potential);
            }
            return roots;
        }
    }
}
