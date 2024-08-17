using Magistrate.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Magistrate.Windows.Modules
{
    internal sealed class ADMCModule : BaseModule
    {
        private static bool DebugVerbose = false;
        private struct ADMEntry
        {
            public byte Hive;
            public string psKeyPath;
            public string psValueName;
        }

        private enum RegistryHives
        {
            Machine,
            User
        }

        private Dictionary<CheckInfo, List<ADMEntry>> ADMXData = new Dictionary<CheckInfo, List<ADMEntry>>();
        public ADMCModule() : base()
        {
            SetTickRate(47020);
#if DEBUG
            if (DebugVerbose) Console.WriteLine("ADMC module initialized");
#endif
        }

        protected override List<CheckState> QueryState(CheckInfo info)
        {
#if DEBUG
            if (DebugVerbose) Console.WriteLine("ADMC check invoked");
#endif
            try
            {
                if (!ADMXData.ContainsKey(info))
                {
                    ADMXData[info] = new List<ADMEntry>();
                    string fpath = info.GetArgument(0)?.ToString();
                    if (fpath == null || !File.Exists(fpath)) return SingleState(CheckInfo.DEFAULT);
                    byte[] admc_data = Decompress(File.ReadAllBytes(fpath));
                    int numEntries = BitConverter.ToInt32(admc_data, 0);
#if DEBUG
                    if (DebugVerbose) Console.WriteLine("ADMC decompressing...");
#endif
                    int index = 4;
                    for (int i = 0; i < numEntries; i++)
                    {
                        ADMEntry entry = new ADMEntry();
                        entry.Hive = admc_data[index];
                        entry.psKeyPath = Encoding.ASCII.GetString(admc_data, index + 2, admc_data[index + 1]);
                        index += 2 + admc_data[index + 1];
                        entry.psValueName = Encoding.ASCII.GetString(admc_data, index + 1, admc_data[index]);
                        index += 1 + admc_data[index];
                        ADMXData[info].Add(entry);
                    }
#if DEBUG
                    if (DebugVerbose) Console.WriteLine("ADMC loaded");
#endif
                }
                List<CheckState> States = new List<CheckState>();
                foreach (var entry in ADMXData[info])
                {
                    var root = SelectHive((RegistryHives)entry.Hive);
                    var key = root.OpenSubKey(entry.psKeyPath);
                    string valueString = key?.GetValue(entry.psValueName)?.ToString() ?? "0";
                    string sval = HiveToString((RegistryHives)entry.Hive) + "\\" + entry.psKeyPath.ToLower() + $"\\{entry.psValueName.ToLower()}+" + (valueString);
#if DEBUG
                    if (DebugVerbose) Console.WriteLine(sval);
#endif
                    States.Add(new CheckState(info.ComputeHash(sval)));
                }
                return States;
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.ToString());
#endif
            }
            return info.Maintain();
        }

        private static RegistryKey SelectHive(RegistryHives hive)
        {
            switch(hive)
            {
                case RegistryHives.User:
                    return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
                default:
                    return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
            }
        }

        private static string HiveToString(RegistryHives hive)
        {
            switch (hive)
            {
                case RegistryHives.User:
                    return "hkey_current_user";
                default:
                    return "hkey_local_machine";
            }
        }

        private static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream();
            using (var compressStream = new MemoryStream(data))
            using (var compressor = new DeflateStream(compressStream, CompressionMode.Decompress))
            {
                compressor.CopyTo(input);
                compressor.Close();
                return input.ToArray();
            }
        }
    }
}
