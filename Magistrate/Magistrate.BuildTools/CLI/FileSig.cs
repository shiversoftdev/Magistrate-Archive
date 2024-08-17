using Blake2Fast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.BuildTools.CLI
{
    internal sealed class FileSig : Command
    { 
        public override int NumArgs => 1;
        public override string CommandDescription => "[path]\tProduce a file signature for the given file";
        public override bool Exec(string[] args)
        {
            if (!File.Exists(args[0])) Root.Error("Cannot produce an file sig on a non-existant file");
            Console.Write(CalcFileSig(args[0]) ?? "No Signature");
            return true;
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
                    if (fs.Read(buff, 0, buff.Length) < buff.Length) { Console.WriteLine("fs read failed");  return null; }
                byte[] digest = Blake2b.ComputeHash(0x10, buff);
                return $"{file}+{filesize:X}+{ByteArrayToStr(digest)}".ToLower().Trim();
            }
            catch (Exception e) { Console.WriteLine(e.ToString());  return null; }
        }

        private string ByteArrayToStr(byte[] arr)
        {
            char[] result = new char[arr.Length * 2];
            for (int i = 0; i < (arr.Length * 2); i += 2)
            {
                string exr = arr[i / 2].ToString("X2");
                result[i] = exr[0];
                result[i + 1] = exr[1];
            }
            return new string(result);
        }
    }
}
