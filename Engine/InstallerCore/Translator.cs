using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Engine.Installer.Core.Templates
{
    /// <summary>
    /// A template translation framework for building cross platform engines securely
    /// </summary>
    public static class Translator
    {
        #region DEBUG
        #if DEBUG
        

        /// <summary>
        /// Returns a debugging check list from an xml formatted input string
        /// </summary>
        /// <param name="input">The xml to parse into debugging checks</param>
        /// <returns></returns>
        public static CheckDefinition[] ParseDebuggingChecklist(string input)
        {
            //Handle nodes: AFTER, NoPoints
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(input);
            List<CheckDefinition> checks = new List<CheckDefinition>();
            foreach (XmlNode node in doc.GetElementsByTagName("check"))
            {
                try
                {
                    Enum.TryParse(node["key"].InnerText, true, out CheckTypes checktype);
                    List<CheckDefinition.FCompareOp> CheckOps = new List<CheckDefinition.FCompareOp>();
                    string SuccessString = null;
                    string FailureString = null;
                    string[] args = new string[node["arguments"].ChildNodes.Count];
                    for (int i = 0; i < args.Length; i++)
                    {
                        args[i] = node["arguments"].ChildNodes[i].InnerText;
                    }

                    var node_answers = node.SelectNodes("answer");
                    string[] answers = new string[node_answers.Count];
                    for(int i = 0; i < answers.Length; i++)
                    {
                        answers[i] = node_answers[i].InnerText;
                    }

                    var node_ands = node.SelectNodes("and");
                    foreach(XmlNode anode in node_ands)
                    {
                        try
                        {
                            ushort Value = Convert.ToUInt16(anode.InnerText);
                            CheckOps.Add(new CheckDefinition.FCompareOp() { Child = Value, CompareOp = CheckDefinition.ECompareOp.AND });
                        }
                        catch
                        {

                        }
                    }

                    var node_ors = node.SelectNodes("or");
                    foreach (XmlNode onode in node_ors)
                    {
                        try
                        {
                            ushort Value = Convert.ToUInt16(onode.InnerText);
                            CheckOps.Add(new CheckDefinition.FCompareOp() { Child = Value, CompareOp = CheckDefinition.ECompareOp.OR });
                        }
                        catch
                        {

                        }
                    }

                    var node_befores = node.SelectNodes("before");
                    foreach (XmlNode bnode in node_befores)
                    {
                        try
                        {
                            ushort Value = Convert.ToUInt16(bnode.InnerText);
                            CheckOps.Add(new CheckDefinition.FCompareOp() { Child = Value, CompareOp = CheckDefinition.ECompareOp.BEFORE });
                        }
                        catch
                        {

                        }
                    }

                    var HasNoPoints = node.SelectNodes("nopoints");

                    byte flags = Convert.ToByte(node["flags"].InnerText);

                    if(HasNoPoints.Count > 0)
                    {
                        flags |= (byte)CheckTemplate.CheckDefFlags.NoPoints;
                    }

                    var HasSStr = node.SelectNodes("success");
                    var HasFStr = node.SelectNodes("failure");

                    if (HasSStr.Count > 0)
                    {
                        SuccessString = HasSStr[0].InnerText;
                    }

                    if (HasFStr.Count > 0)
                    {
                        FailureString = HasFStr[0].InnerText;
                    }

                    CheckDefinition d = CheckDefinition.DebugCheck(checktype, Convert.ToUInt16(node["id"].InnerText), Convert.ToInt16(node["points"].InnerText), flags, CheckDefinition.FOfflineAnswer.ToAnswers(answers), CheckOps.ToArray(), SuccessString, FailureString, args);
                    
                    if (d != null)
                        checks.Add(d);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace.ToString());
                    Console.WriteLine(e.Message);
                }
            }
            return checks.ToArray();
        }
#endif
        #endregion

        /// <summary>
        /// Build an engine from the installation framework
        /// </summary>
        /// <param name="EngineBase">The base engine string</param>
        /// <param name=""></param>
        /// <returns></returns>
        public static string BuildTranslatedEngine(string EngineBase, string TemplatesDirectory, CheckDefinition[] checks, bool Online, string ImageName)
        {
            Dictionary<CheckTypes, string> RuntimeTemplates = new Dictionary<CheckTypes, string>();
            try
            {
                System.IO.DirectoryInfo TemplateDir = new System.IO.DirectoryInfo(TemplatesDirectory);
                foreach (System.IO.FileInfo template in TemplateDir.GetFiles("*.cst", System.IO.SearchOption.AllDirectories))
                {
                    string templatename = template.Name.ToLower().Replace(".cst", "");
                    if (Enum.TryParse(templatename, true, out CheckTypes checktype))
                    {
                        RuntimeTemplates[checktype] = template.FullName;
                    }
                }
            }
            catch
            {
            }

            List<CheckPreWrapper> Wrappers = new List<CheckPreWrapper>();

            foreach (var check in checks)
            {
                try
                {
                    CSTemplate template = System.IO.File.ReadAllLines(RuntimeTemplates[(CheckTypes)check.CheckKey]);
                    Wrappers.Add(new CheckPreWrapper() { Definition = check, Template = template });
                }
                catch
                {
                }
            }
            CSTemplate _checktemplate = System.IO.File.ReadAllLines(RuntimeTemplates[CheckTypes.CheckTemplate]);
            Wrappers.Add(new CheckPreWrapper() { Definition = null, Template = _checktemplate });

            return BuildEngine(EngineBase, Wrappers.ToArray(), ImageName);
        }

        /// <summary>
        /// Convert CS source to a CST
        /// </summary>
        /// <param name="CSInput">The c# source code</param>
        /// <returns>A CST formatted string</returns>
        public static string MakeCST(string CSInput)
        {
            string result = "//cst\r\n";
            CSInput = CSInput.Replace(";", ";\r\n"); //Fix any inlining so we can get accurate using defs
            string[] lines = CSInput.Split('\n', '\r');
            bool EndUsings = false;
            foreach (string s in lines)
            {
                if (s.Trim().Length < 1)
                    continue;
                if (!EndUsings && s.Trim().Length > 6 && s.Trim().Substring(0, 5) == "using")
                    result += "//?";
                else
                    EndUsings = true;
                result += s.Trim() + "\r\n";
            }
            return result;
        }

        /// <summary>
        /// Build an engine.cs from a base engine and a list of checks
        /// </summary>
        /// <param name="EngineBase">The base of the engine</param>
        /// <param name="checks">A list of checks</param>
        /// <returns></returns>
        private static string BuildEngine(string EngineBase, CheckPreWrapper[] checks, string ImageName)
        {
            //text.Replace("/*?installer.key*/", keytext);
            Random r = new Random();
            List<string> Includes = new List<string>();
            int count = 0;
            string engine_classes = "";
            string engine_includes = "";
            string engine_init = "";
            string engine_fields = "";
            string engine_tick = "";

            engine_fields += "public override string __PUBLIC => \"";
            for(int i = 0; i < 16; i++)
            {
                switch(r.Next(0,3))
                {
                    case 0:
                        engine_fields += (char)r.Next(48, 58);
                        break;
                    case 1:
                        engine_fields += (char)r.Next(65, 91);
                        break;
                    default:
                        engine_fields += (char)r.Next(97, 123);
                        break;
                }
            }
            engine_fields += "\";\r\n";

            //Implement F4
            engine_fields += "protected override uint __F4__ => 0x";

            List<int> indexes = Enumerable.Range(0, 31).ToList();

            for (int i = 0; i < 16; i++)
            {
                int index = r.Next(0, indexes.Count);
                indexes.RemoveAt(index);
            }

            uint F4_Mask = 0xFFFFFFFF;
            foreach (int i in indexes)
                F4_Mask &= ~(uint)(1 << i);

            engine_fields += F4_Mask.ToString("X") + ";\r\n";


            HashSet<ushort> CheckSingleIDs = new HashSet<ushort>();

            int NumEnums = Enum.GetValues(typeof(CheckTypes)).Length;

            while(CheckSingleIDs.Count < NumEnums) CheckSingleIDs.Add((ushort)r.Next(0xFFFF));



            Dictionary<CheckTypes, ushort> SingletonMap = new Dictionary<CheckTypes, ushort>();

            int _i = 0;
            foreach(CheckTypes c in Enum.GetValues(typeof(CheckTypes))) SingletonMap[c] = CheckSingleIDs.ToArray()[_i++];

            byte[] EncKey = new byte[16];
            r.NextBytes(EncKey);
            string keytext = "{";
            for (int i = 0; i < 16; i++)
            {
                keytext += EncKey[i] + (i != 15 ? "," : "};//");
            }
            TranslatorString.__key__ = EncKey;

            List<string> CompOps = new List<string>();
            foreach (var check in checks)
            {
                foreach(string include in check.Template.Includes)
                {
                    if(!engine_includes.Contains(include))
                        engine_includes += include + "\r\n";
                    Includes.Add(include);
                }
                string code = "";
                foreach(string s in check.Template.CodeLines)
                {
                    code += s + "\r\n";
                }
                if (check.Definition == null) //should only work for the checktemplate class
                {
                    engine_classes += code.Replace("/*?installer.key*/", keytext) + "\r\n";
                    continue;
                }
                string old = code;
                string name;
                using (MD5 md5Hash = MD5.Create())
                {
                    name = "__" + GetMd5Hash(md5Hash, r.Next() + ((CheckTypes)check.Definition.CheckKey).ToString() + "__" + count);
                }
                code = code.Replace(((CheckTypes)check.Definition.CheckKey).ToString(), name);
                string args = "";
                if(code != old)
                {
                    engine_classes += code + "\r\n";
                    check.ClassName = name;
                    string[] arguments = check.Definition.Arguments;
                    for (int i = 0; i < arguments.Length; i++)
                    {
                        //args += "@\"" + arguments[i] + "\"" + (i == arguments.Length - 1 ? "" : ", "); //TODO: Implement a safestring encrypted byte array
                        args += "(SafeString)(new byte[]{ ";
                        byte[] encResult = TranslatorString.E(arguments[i]);
                        foreach(byte b in encResult)
                        {
                            args += b + ",";
                        }
                        args += "}) " + (i == arguments.Length - 1 ? "" : ", ");
                    }

                    string ExpectedItems = "";

                    foreach(var chk in check.Definition.OfflineAnswers)
                    {
                        string BytesString = "";
                        foreach(byte b in chk.AnswerData)
                        {
                            BytesString += "0x" + b.ToString("X") + ",";
                        }
                        ExpectedItems += "(EngineFrame.ACompareType." + chk.CompareType.ToString() + ", new byte[] { " + BytesString + " }), ";
                    }

                    check.Declarator = "try { c_" + count + " = new " + check.ClassName + "(" + args + "){ Flags = (byte)" + check.Definition.Flags + ", __ALT__ = (uint)" + RIDEncode(SingletonMap[(CheckTypes)check.Definition.CheckKey], F4_Mask) + ", RequestSingletonData = RequestSingletonData }; ";
                    check.Declarator += "c_" + count + ".CheckFailed = () => { Fail((ushort)" + check.Definition.CheckID + "); }; ";
                    check.InstanceName = "c_" + count;
                    check.StateName = check.InstanceName + "_s";
                    check.Header = "\r\nprivate " + check.ClassName + " c_" + count + "; private byte[] " + check.StateName + ";\r\n";

                    check.Declarator += "RegisterSingleton((uint)" + RIDEncode(SingletonMap[(CheckTypes)check.Definition.CheckKey], F4_Mask) + ", " + check.InstanceName + "); ";
#if OFFLINE
                    check.Declarator += "Scoring.ScoringItem si_" + count + " = new Scoring.ScoringItem() {" +
                        "Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ " + ExpectedItems + " }, " +
                        "NumPoints = (short)" + check.Definition.NumPoints + ", " +
                        "ID = (ushort)" + check.Definition.CheckID +
                        "}; ";

                    check.Declarator += "si_" + count + ".SuccessStatus = () => { return " + check.InstanceName + ".CompletedMessage; }; ";
                    check.Declarator += "si_" + count + ".FailureStatus = () => { return " + check.InstanceName + ".FailedMessage; };";

                    check.Declarator += "Expect(" +
                        "(ushort)" + check.Definition.CheckID + "," +
                        "si_" + count + "); ";
#endif
                    check.Declarator += "InitState(" + check.Definition.CheckID + ", new byte[0]);";
                    foreach(CheckDefinition.FCompareOp co in check.Definition.CompareOps)
                    {
                        CompOps.Add(co.CompareOp.ToString() + "(" + check.Definition.CheckID + ", " + co.Child + ");");
                    }

                    if(check.Definition.SuccessString != null)
                    {
                        check.Declarator += "si_" + count + ".SuccessStatus = () => { return (SafeString)(new byte[]{ ";
                        byte[] encResult = TranslatorString.E(check.Definition.SuccessString);
                        foreach (byte b in encResult)
                        {
                            check.Declarator += b + ",";
                        }
                        check.Declarator += " }); };";
                    }

                    if (check.Definition.FailureString != null)
                    {
                        check.Declarator += "si_" + count + ".FailureStatus = () => { return (SafeString)(new byte[]{ ";
                        byte[] encResult = TranslatorString.E(check.Definition.FailureString);
                        foreach (byte b in encResult)
                        {
                            check.Declarator += b + ",";
                        }
                        check.Declarator += " }); };";
                    }

                    engine_fields += check.Header + "\r\n";
                    engine_init += check.Declarator + "} catch { }\r\n";
                    engine_tick += "if(" + check.InstanceName + "?.Enabled ?? false){ try{" + check.StateName + " = await " + check.InstanceName + ".CheckState(); ";
                    engine_tick += "RegisterCheck((ushort)" + check.Definition.CheckID + "|((uint)" + check.InstanceName + ".Flags << 16)," + check.StateName + ");} catch{ } }\r\n";
                }
                count++;
            }
            engine_init += "SetImageName(@\"" + ImageName.Replace("\"", "") + "\");\r\n";

            foreach(string s in CompOps)
            {
                engine_init += s + "\r\n";
            }

            EngineBase = EngineBase.Replace("//?installer.includes", engine_includes);
            EngineBase = EngineBase.Replace("//?installer.fields", engine_fields);
            EngineBase = EngineBase.Replace("//?installer.init", engine_init);
            EngineBase = EngineBase.Replace("//?installer.tick", engine_tick);
            EngineBase = EngineBase.Replace("//?installer.classes", engine_classes);
            return EngineBase;
        }

        private static Random rid_rand = new Random();
        private static uint RIDEncode(ushort key, uint mask) //Encode the key into the mask format and fill in the blanks with random data
        {
            uint final = 0u;
            for(int i = 0, count = 0; i < 32; i++)
            {
                if ((mask & (1 << i)) > 0)
                    final += (uint)((key & (1 << count)) << (i - count++));
                else
                    final += (uint)(rid_rand.Next(2) * (1 << i));
            }
            return final;
        }

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static void ExecTranslation(string[] args)
        {
            Console.WriteLine("Translator started... ");
            if (args.Length > 2)
            {
                if (args[0].ToLower().Contains("/t") && System.IO.Directory.Exists(args[1]) && System.IO.Directory.Exists(args[2]))
                {
                    string inputdir = args[1];
                    string outputdir = args[2];
                    DirectoryInfo TemplateDir = new DirectoryInfo(inputdir);
                    foreach (FileInfo template in TemplateDir.GetFiles("*template.cs", SearchOption.AllDirectories))
                    {
                        string outfile = Path.Combine(outputdir, template.Name.Replace(".cs", ".cst"));
                        try
                        {
                            if (File.Exists(outfile))
                            {
                                File.Delete(outfile);
                            }
                            File.WriteAllText(outfile, Engine.Installer.Core.Templates.Translator.MakeCST(File.ReadAllText(template.FullName)));
                        }
                        catch
                        {
                            Console.WriteLine("Failed to import: " + template.FullName);
                        }
                    }
                }
#if DEBUG
                else if (args[0].ToLower().Contains("/e"))
                {
                    //arg1 engine source
                    //arg2 templates dir (translated)
                    try
                    {
                        File.WriteAllText(args[1], Engine.Installer.Core.Templates.Translator.BuildTranslatedEngine(File.ReadAllText(args[1]), args[2], ParseDebuggingChecklist(File.ReadAllText(args[3])), false, "Space Force Server"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace.ToString());
                        Console.WriteLine(e.Message);
                        Environment.Exit(2);
                    }
                }

                else if (args[0].ToLower().Contains("/i"))
                {
                    try
                    {
                        //1: in xml
                        //2: out filename
                        File.WriteAllBytes(args[2], Engine.Installer.Core.InstallationPackage.MakeDebugInstall(File.ReadAllText(args[1])));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace.ToString());
                        Console.WriteLine(e.Message);
                        Environment.Exit(2);
                    }
                }
#endif
                else if (args[0].ToLower().Contains("/rb"))
                {
                    //arg1 Build type (windows|linux)
                    //arg2 engine source
                    //arg3 templates dir (translated)
                    //arg4 InstallPackage

                    try
                    {
                        Engine.Installer.Core.InstallationPackage pkg = new Engine.Installer.Core.InstallationPackage(File.ReadAllBytes(args[4]));

                        if (pkg.HasFlag(Engine.Installer.Core.InstallationPackage.InstallFlags.Linux) != (args[1].ToLower().Trim() == "linux"))
                            Environment.Exit(0);

                        File.WriteAllText(args[2], Engine.Installer.Core.Templates.Translator.BuildTranslatedEngine(File.ReadAllText(args[2]), args[3], pkg.Checks, !pkg.HasFlag(Engine.Installer.Core.InstallationPackage.InstallFlags.Offline), pkg.Name));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace.ToString());
                        Console.WriteLine(e.Message);
                        Environment.Exit(2);
                    }
                }
                else
                {
                    Console.WriteLine("Translator exited because an unknown command was executed: " + args[0]);
                }
            }
            else
            {
                Environment.Exit(1);
            }
        }

    }

    internal sealed class TranslatorString
    {
        internal static byte[] __key__ = new byte[] { 0x00, 0xc5, 0x6c, 0xdd, 0x38, 0x8d, 0xa7, 0x02, 0x43, 0x92, 0x96, 0xae, 0x31, 0x99, 0x8f, 0x79 };

        private byte[] data;

        /// <summary>
        /// Create a string from a safe string
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator string(TranslatorString s)
        {
            return D(s.data);
        }

        /// <summary>
        /// Create a safe string from a normal string
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator TranslatorString(string s)
        {
            TranslatorString st = new TranslatorString
            {
                data = E(s)
            };
            return st;
        }

        /// <summary>
        /// Create a byte array into a safestring
        /// </summary>
        /// <param name="dat"></param>
        public static explicit operator TranslatorString(byte[] dat)
        {
            TranslatorString st = new TranslatorString
            {
                data = dat
            };
            return st;
        }

        public override string ToString()
        {
            return D(data);
        }

        internal static byte[] E(string str)
        {
            if (str == null)
                str = "";
            byte[] encrypted;
            byte[] IV;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = __key__;
                aesAlg.GenerateIV();
                IV = aesAlg.IV;

                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(str);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            var combinedIvCt = new byte[IV.Length + encrypted.Length];
            Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
            Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);

            // Return the encrypted bytes from the memory stream. 
            return combinedIvCt;
        }

        internal static string D(byte[] str)
        {

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = __key__;

                byte[] IV = new byte[aesAlg.BlockSize / 8];
                byte[] cipherText = new byte[str.Length - IV.Length];

                Array.Copy(str, IV, IV.Length);
                Array.Copy(str, IV.Length, cipherText, 0, cipherText.Length);

                aesAlg.IV = IV;

                aesAlg.Mode = CipherMode.CBC;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}
