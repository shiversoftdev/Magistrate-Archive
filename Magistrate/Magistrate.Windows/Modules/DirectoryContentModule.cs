using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Blake2Fast;

namespace Magistrate.Windows.Modules
{
    
    internal sealed class DirectoryContentModule : BaseModule
    {
        private static bool DebugVerbose = false;
        public DirectoryContentModule() : base()
        {
            SetTickRate(54000);
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string KeyPath = info.GetArgument(0)?.ToString();
            if (KeyPath == null) return SingleState(CheckInfo.DEFAULT);
            List<CheckState> States = new List<CheckState>();
            string[] dirpaths = KeyPath.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
            foreach (string file in RelevantFiles(new HashSet<string>() { Path.GetPathRoot(KeyPath) }, dirpaths, 0))
            {
                try
                {
                    var result = CalcFileSig(file);
#if DEBUG
                    if (DebugVerbose) Console.WriteLine("#" + result + "#");
#endif
                    if (result == null) continue;
                    var hash = info.ComputeHash(result);
                    States.Add(new CheckState(hash));
                }
                catch{}
            }
            
            return States;
        }

        private string CalcFileSig(string file)
        {
            if (!File.Exists(file)) return null;
            long filesize = new FileInfo(file).Length;
            var readSize = Math.Min(0x1000, filesize);
            if (readSize < 1) return null;
            try
            {
                byte[] buff = new byte[readSize];
                using (var fs = File.OpenRead(file))
                    if (fs.Read(buff, 0, buff.Length) < buff.Length) return null;
                byte[] digest = Blake2b.ComputeHash(0x10, buff);
                return ($"{file}+{filesize:X}+{ByteArrayToStr(digest)}").ToLower().Trim();
            }
            catch { return null; }
        }

        private string ByteArrayToStr(byte[] arr)
        {
            char[] result = new char[arr.Length * 2];
            for(int i = 0; i < (arr.Length * 2); i += 2)
            {
                string exr = arr[i/2].ToString("X2");
                result[i] = exr[0];
                result[i + 1] = exr[1];
            }
            return new string(result);
        }

        private HashSet<string> RelevantFiles(HashSet<string> roots, string[] paths, int index)
        {
            if (index >= paths.Length) return roots;
            HashSet<string> files = new HashSet<string>();
            string cpath = paths[index];
            foreach (var file in roots)
            {
                if (File.Exists(file))
                {
                    files.Add(file);
                    continue;
                }
                if (!Directory.Exists(file)) continue;
                if (cpath == "*")
                {
                    try
                    {
                        foreach (var fi in Directory.EnumerateFiles(file)) files.Add(fi.ToLower());
                        foreach (var dir in Directory.EnumerateDirectories(file)) files.Add(dir.ToLower());
                    }
                    catch { }
                    continue;
                }
                if (Directory.Exists(Path.Combine(file, cpath))) files.Add(Path.Combine(file, cpath).ToLower());
                else if (File.Exists(Path.Combine(file, cpath))) files.Add(Path.Combine(file, cpath).ToLower());
            }
            roots.Clear();
            foreach (string potential in RelevantFiles(files, paths, index + 1))
            {
                if (File.Exists(potential)) roots.Add(potential);
            }
            return roots;
        }
    }
}
