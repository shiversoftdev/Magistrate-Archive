using Blake2Fast;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Magistrate.BuildTools.CLI
{
    internal sealed class ADMCompiler : Command
    { 
        public override int NumArgs => 1;
        public override string CommandDescription => "[path]\tProduce a compiled admx database";
        public override bool Exec(string[] args)
        {
            if (!File.Exists(args[0])) Root.Error("Cannot produce an admc on a non-existant file");
            XmlDocument conf = new XmlDocument();
            conf.LoadXml(File.ReadAllText(args[0]));
            var policies = conf.GetElementsByTagName("policy");
            var admc = new ADMC();
            foreach (XmlNode policy in policies)
            {
                var nKey = policy.Attributes["key"];
                var nClass = policy.Attributes["class"];
                var nValue = policy.Attributes["valueName"];
                if (nKey == null) continue;
                if (nClass == null) continue;
                if (nValue != null) goto add;
                foreach (XmlNode node in policy.ChildNodes)
                {
                    if (node.Name != "elements" && node.Name != "enabledList" && node.Name != "disabledList") continue;
                    foreach(XmlNode element in node.ChildNodes)
                    {
                        if (element.Attributes["valueName"] == null) continue;
                        nValue = element.Attributes["valueName"];
                        goto add;
                    }
                }
            add:
                if (nValue == null) continue;
                var adme = new ADMEntry();
                if(!Enum.TryParse(nClass.Value, true, out RegistryHives hive)) Root.Error(nClass.Value + " was not registered as a valid registry class");
                adme.Hive = (byte)hive;
                adme.psKeyPath = nKey.Value.ToLower().Trim();
                adme.psValueName = nValue.Value.ToLower().Trim();
                //Console.WriteLine(adme.psKeyPath + ":" + adme.psValueName);
                admc.Entries.Add(adme);
            }
            admc.NumEntries = admc.Entries.Count;
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(admc.NumEntries));
            foreach(var entry in admc.Entries)
            {
                data.Add(entry.Hive);
                data.Add((byte)entry.psKeyPath.Length);
                data.AddRange(Encoding.ASCII.GetBytes(entry.psKeyPath));
                data.Add((byte)entry.psValueName.Length);
                data.AddRange(Encoding.ASCII.GetBytes(entry.psValueName));
            }
            Compress(data.ToArray(), "output.admc");
            return true;
        }

        private static void Compress(byte[] data, string outfile)
        {
            MemoryStream ms = new MemoryStream(data);
            using (FileStream compressedFileStream = File.Create(outfile))
            {
                using (DeflateStream compressionStream = new DeflateStream(compressedFileStream, CompressionMode.Compress))
                {
                    ms.CopyTo(compressionStream);
                }
            }
            ms.Close();
        }

        private class ADMC
        {
            public int NumEntries;
            public List<ADMEntry> Entries = new List<ADMEntry>();
        }

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
    }
}
