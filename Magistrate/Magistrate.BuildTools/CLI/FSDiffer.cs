using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinet.Core.IO.Ntfs;

namespace Magistrate.BuildTools.CLI
{
    internal sealed class FSDiffer : Command
    { 

        public override int NumArgs => 3;
        public override string CommandDescription => "[path] [maxdepth] [name]\tProduce an fsd for the given path";
        public override bool Exec(string[] args)
        {
            if (!Directory.Exists(args[0]))
                Root.Error("Cannot produce an FSD on a file or a non existant directory.");

            if (!int.TryParse(args[1], out int MaxDepth))
                Root.Error("Invalid max depth specified");

            List<byte> FSDInfo = new List<byte>();

            foreach(string file in FilesForDepth(args[0], 0, MaxDepth))
            {
                FSDInfo.AddRange(BitConverter.GetBytes(Root.FNV1a(file.ToLower().Trim())));
                Console.WriteLine(file.ToLower());
            }

            File.WriteAllBytes(args[2] + ".fsd", FSDInfo.ToArray());

            return true;
        }

        public List<string> FilesForDepth(string path, int depth, int maxdepth)
        {
            List<string> FilesFound = new List<string>();

            if (depth >= maxdepth) return FilesFound;
            try
            {
                foreach(var file in Directory.EnumerateFiles(path))
                {
                    FilesFound.Add(file);
                    try
                    {
                        foreach (var altds in new FileInfo(file).ListAlternateDataStreams())
                        {
                            FilesFound.Add(altds.FullPath);
                        }
                    }
                    catch { }
                }
                
                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    try { FilesFound.AddRange(FilesForDepth(dir, depth + 1, maxdepth)); } catch { }
                }
            } catch { }
            return FilesFound;
        }
    }
}
