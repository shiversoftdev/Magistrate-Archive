using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Reflection;
using System.Xml;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Magistrate.BuildTools.CLI
{
    internal sealed class Build : Command
    {
        /// <summary>
        /// String to append when concatenating forensics answers
        /// </summary>
        public const string CSSE_FQ_CONCATSTR = "&answer=";
        public const string CSSE_FQ_F_IGNORECASE = "ignorecase";
        public const string CSSE_FQ_F_IGNOREWHITESPACE = "ignorews";

        public const int SIZEOF_QRICHTEXT = 4096;
        public override int NumArgs => 2;
        public override string CommandDescription => "[path] [project]\tGenerate a new engine at the given path";
        private ConfigManager MGR = new ConfigManager();
        private SHA512 Hasher = new SHA512Managed();

        public override bool Exec(string[] args)
        {
            string path = args[0];
            string project = args[1];
 
            string csproj = Path.Combine(path, project + ".csproj");
            string conf = Path.Combine(path, "conf.json");

            if (!Directory.Exists(path))
                Root.Error($"Path '{path}' does not exist; cannot build a non existent path.");

            if (!File.Exists(csproj))
                Root.Error($"Project '{csproj}' doesn't exist; cannot build a non existent project.");

            if (!File.Exists(conf))
                Root.Error($"'{conf}' doesn't exist; missing configuration.");

            if (!MGR.Parse(conf))
                Root.Error($"Invalid configuration file provided ({MGR.Message})");

            // 1. Parse check manifest information
            List<MBuildConfig.MCheckConfig> Checks = new List<MBuildConfig.MCheckConfig>();
            List<MBuildConfig.MScoringData> Scores = new List<MBuildConfig.MScoringData>();

            // 1.0 Initialize dynamic information to support forensics
            Checks.AddRange(MGR.Config.Checks);
            Scores.AddRange(MGR.Config.Scoring);

            // 1.1 Module import collection verifies that all required modules are included
            HashSet<string> ModuleImports = new HashSet<string>();

            // 1.2 Check registration headers
            List<string> CheckRegistrations = new List<string>();

            // 1.3 Build engine constructor
            string _constructor = "\r\n";

            // 1.4 Build engine fields
            string _fields = "\r\n";

            // 1.5 Build StartEngine()
            string _startengine = "\r\n";

            // 1.6 Forensics module will always be loaded
            ModuleImports.Add("ForensicsModule");

            //1.7 Const Pack Decryption
            byte[] SecretKey = Guid.NewGuid().ToByteArray();
            byte[] SecretIV = Guid.NewGuid().ToByteArray();
            List<byte> ConstPackData = new List<byte>();

            //1.8 Prefix the secret data with secret IV
            ConstPackData.AddRange(SecretIV);

            // Generate checks from forensics questions
            foreach (MBuildConfig.MForensicsQuestion fq in MGR.Config.Forensics)
            {
                for (int i = 0; i < fq.Flags.Length; i++)
                    fq.Flags[i] = fq.Flags[i].Trim().ToLower();

                MBuildConfig.MCheckConfig NewCheck = new MBuildConfig.MCheckConfig()
                {
                    Operation = "eq",
                    CID = Guid.NewGuid().ToString(),
                    Check = "ForensicsCheck",
                    Data = new Dictionary<string, string>()
                    { 
                        { "id", fq.ID.ToString() },
                        { "flags", fq.Flags != null ? string.Join("|", fq.Flags) : "" }
                    }
                };

                if (fq.Points <= 0)
                    Root.Error($"A forensics question with the id {fq.ID} was found to have a negative or zero value.");

                MBuildConfig.MScoringData NewScore = new MBuildConfig.MScoringData()
                {
                    Points = fq.Points,
                    Query = new string[] { "ForensicsCheck" },
                    Constraints = Array.Empty<string>(),
                    Message = fq.Message,
                    Answer = CalculateForensicsAnswer(fq),
                    AnswerCID = NewCheck.CID,
                    CI2 = fq.CI2
                };

                Checks.Add(NewCheck);
                Scores.Add(NewScore);
            }
            
            // Generate check info
            foreach (MBuildConfig.MCheckConfig check in Checks)
            {
                MCheckDefinition checkdef = Root.GlobalManifest.ResolveCheckDef(check.Check, true); //force asserts valid check

                MModuleDefinition module = Root.GlobalManifest.ResolveModuleDef(checkdef.Module, true); //force asserts valid module requirements
                ModuleImports.Add(checkdef.Module);

                string CheckRegFormat = "var {3} = new Core.CheckInfo({0}){{ {1} }}; RegisterCheck({3}, out _{2});";
                string c_args = "";
                string c_initialization = "";

                // Generate check header initializations
                if(check.Random)
                {
                    Root.Error("Random checks have not been implemented"); //TODO
                }
                else
                {
                    for(int i = 0; i < module.Args.Length; i++)
                    {
                        string currArg = module.Args[i];

                        int CurrPos = ConstPackData.Count;

                        string arg_interp = InterpretArgument(check.Data[currArg]);

                        byte[] pdata = AES(arg_interp, SecretKey, SecretIV);

                        ConstPackData.AddRange(pdata);

                        string encresult = $"{CurrPos}.u({pdata.Length})";

                        if (check.Data.ContainsKey(currArg))
                            c_args += encresult;
                        else
                            c_args += "null";

                        if (i + 1 < module.Args.Length)
                            c_args += ", ";
                    }

                    check.RCID = RVariableName();
                    check.ISALT = Guid.NewGuid().ToString();

                    //TODO check.Operation needs an IL translator
                    c_initialization = $"ISALT = @\"{check.ISALT}\", Operation = @\"{check.Operation}\"";
                }

                // Create a check registration header
                CheckRegistrations.Add(string.Format(CheckRegFormat, c_args, c_initialization, checkdef.Module, check.RCID));
            } //end of check foreach

            // Create all module headers
            foreach(string module in ModuleImports)
            {
                _fields += $"\t\tprivate Modules.{module} _{module};\r\n";
                _startengine += $"\t\t\t_{module}?.StartModule();\r\n";
            }

            // Create all check registration headers
            foreach(string reg in CheckRegistrations)
            {
                _constructor += $"\t\t\t{reg}\r\n";
            }

            // 2. Scoring Data

            // 2.1 Scoring Headers
            List<string> ScoreHeaders = new List<string>();
            string ScoreFormatter =
                "\t\t\tvar {4} = new Core.ScoreItem(){{ PointValue = {0}, DecryptKey = @\"{1}\", DecryptString = new byte[]{{ {2} }}, IV = new byte[]{{ {3} }}, Constraints = new Dictionary<Core.ScoreItem, string>(), IsStateless = {5} }};\r\n\t\t\tRegisterScoreEntry({4});\r\n";
            
            // 2.2 Scoring constraints map
            Dictionary<string, MBuildConfig.MScoringData> CI2Map = new Dictionary<string, MBuildConfig.MScoringData>();

            foreach(MBuildConfig.MScoringData rule in Scores)
            {
                CI2Map[rule.CI2] = rule;

                var chkConfig = rule.AnswerCID == "#" ? null : MBuildConfig.ResolveCheckByCID(rule.AnswerCID, Checks.ToArray(), true);

                // Create a random decryption key
                rule.DecryptKey = Guid.NewGuid().ToString();

                // Force a non-null message
                if (rule.Message == null || rule.Message.Length == 0)
                    rule.Message = Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

                // Create dec_pre (message;ci2;deckey)
                string dec_pre = $"{rule.Message};{rule.CI2};{rule.DecryptKey}";

                // Calculate a random IV for AES
                byte[] IV = new byte[16];
                Root.Randomization.NextBytes(IV);
                rule.IV = IV;

                // Hash the answer string
                byte[] AnsHash = SHA512Fold32(Encoding.ASCII.GetBytes((chkConfig?.ISALT ?? "") + InterpretArgument(rule.Answer)));

                //Console.WriteLine($"{rule.AnswerCID}#{string.Join(", ", AnsHash)}");

                // Format byte output for plaintext definition
                string DecryptString = string.Join(",", AES(dec_pre, AnsHash, IV));
                string IVString = string.Join(",", IV);
                
                // Generate a random variable name for constraint mapping
                rule.VariableName = RVariableName();

                ScoreHeaders.Add(string.Format(ScoreFormatter, rule.Points, rule.DecryptKey, DecryptString, IVString, rule.VariableName, rule.Stateless.ToString().ToLower()));
            }

            // generate constraint mapping
            foreach (MBuildConfig.MScoringData rule in Scores)
            {
                foreach(string constraint in rule.Constraints)
                {
                    string[] split = constraint.Split(':');
                    string @operator = split[0];
                    string @operand = split[1];

                    if (!CI2Map.ContainsKey(@operand))
                        Root.Error($"Invalid constraint. CI2 '{@operand}' not found.");
                    
                    rule.ChildrenConstraints.Add(CI2Map[@operand]);

                    // validate constraint tree (look for circular references)
                    if (!rule.ValidHierarchy(new List<MBuildConfig.MScoringData>()))
                        Root.Error($"Circular reference detected in constraint tree! {rule.CI2} cannot reference {@operand} as a child!");

                    rule.GeneratedConstraints[CI2Map[@operand].VariableName] = @operator;
                }
            }

            // Create all scoring registration headers
            foreach (string sreg in ScoreHeaders)
            {
                _constructor += sreg;
            }

            // Create all scoring constraints
            foreach (MBuildConfig.MScoringData rule in Scores)
            {
                foreach(var kvp in rule.GeneratedConstraints)
                {
                    _constructor += $"\t\t\t{rule.VariableName}.Constraints[{kvp.Key}] = @\"{kvp.Value}\";\r\n";
                }
            }

            // Translate query strings to runtime database checks
            foreach (MBuildConfig.MScoringData rule in Scores)
            {
                HashSet<string> ModuleQueries = new HashSet<string>();
                HashSet<string> StateQueries = new HashSet<string>();
                
                // Iterate each query request from this check
                foreach(string qs in rule.Query)
                {
                    if (qs == "*") // full database query
                    {
                        foreach (string mod in ModuleImports)
                            ModuleQueries.Add(mod);
                        break;
                    }

                    if (ModuleImports.Contains(qs)) //mod q
                        ModuleQueries.Add(qs);
                    else
                        StateQueries.Add(qs);
                }
                
                // Convert module queries into state queries and map to the current rule

                foreach (MBuildConfig.MCheckConfig check in Checks)
                {
                    MCheckDefinition checkdef = Root.GlobalManifest.ResolveCheckDef(check.Check, true); //force asserts valid check

                    if (ModuleQueries.Contains(checkdef.Module) || StateQueries.Contains(check.Check) || StateQueries.Contains(check.CID))
                    {
                        _constructor += $"\t\t\t{rule.VariableName}.Query.Add({check.RCID});\r\n";
                    }
                }
            }

            // Generate static forensics data
            foreach(MBuildConfig.MForensicsQuestion fq in MGR.Config.Forensics)
            {
                if (fq.Question.Length > SIZEOF_QRICHTEXT)
                    Root.Error($"Forensics question with id {fq.ID} has too long of a question ({fq.Question.Length} characters)");

                List<string> fq_args = new List<string>();

                int CurrPos = ConstPackData.Count;

                byte[] pdata = AES(fq.Question, SecretKey, SecretIV);

                ConstPackData.AddRange(pdata);

                string encresult = $"{CurrPos}.u({pdata.Length})";

                fq_args.Add(encresult);

                fq_args.Add($"(uint){fq.ID}");

                foreach (var answr in fq.Answers)
                {
                    string fqa_name = RVariableName();

                    _constructor += $"\t\t\tvar {fqa_name} = new Core.FQAnswer(){{ Type = (uint){(uint)answr.Type}, Format = @\"{answr.Format}\", Label = @\"{answr.Label}\" }};\r\n";

                    fq_args.Add(fqa_name);
                }

                _constructor += $"\t\t\tRegisterForensicsQuestion({string.Join(", ", fq_args)});\r\n";
            }

            string engine_path = Path.Combine(path, "Generated", "Engine.cs");
            string ConstDataPath = Path.Combine(path, "Generated", "const.pack");
            string engine_content = null;

            // Parse embedded resource of engine template
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Magistrate.BuildTools.data.Engine.cs"))
            using (StreamReader reader = new StreamReader(stream))
            {
                engine_content = reader.ReadToEnd();
            }

            // Write fields
            engine_content = engine_content.Replace("/*<conf.fields>*/", _fields);

            // Write constructor
            engine_content = engine_content.Replace("/*<conf.constructor>*/", _constructor);

            // Write StartEngine()
            engine_content = engine_content.Replace("/*<conf.startengine>*/", _startengine);

            // Write platform architecture
            engine_content = engine_content.Replace("/*<conf.architecture>*/", MGR.Config.Platform == MPlatform.Windows ? ".Windows" : ".Unix");

            // Write const secret key
            engine_content = engine_content.Replace("/*<conf.stringkey>*/", string.Join(", ", SecretKey));

            // Write engine source to project/Generated/Engine.cs
            File.WriteAllText(engine_path, engine_content);

            // Write const data to project/Generated/const.pack
            File.WriteAllBytes(ConstDataPath, ConstPackData.ToArray());

            // Initialize the XML parser for the csproj
            XmlDocument Project = new XmlDocument();
            Project.LoadXml(File.ReadAllText(csproj));

            // Collect all compilation nodes
            XmlNodeList CompileNodes = Project.GetElementsByTagName("Compile");

            foreach(XmlNode node in CompileNodes)
            {
                if(node.Attributes["Include"]?.Value?.StartsWith("Modules\\") ?? false)
                {
                    if (node.Attributes["Condition"] != null)
                        node.Attributes.Remove(node.Attributes["Condition"]);

                    string m_value = node.Attributes["Include"].Value.Substring("Modules\\".Length).Replace(".cs", "");

                    if (ModuleImports.Contains(m_value))
                        continue;

                    XmlAttribute Conditional = Project.CreateAttribute("Condition");
                    Conditional.Value = "$(DefineConstants.Contains('DEBUG'))"; //if project is debug, include this always, otherwise, exclude

                    node.Attributes.SetNamedItem(Conditional);
                }
            }

            // Modify project file to only include relevant modules
            Project.Save(csproj);

            return true;
        }

        /// <summary>
        /// Calculate the expected answer string for forensics question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private string CalculateForensicsAnswer(MBuildConfig.MForensicsQuestion question)
        {
            string @return = "";

            for (int i = 0; i < question.Flags.Length; i++)
            {
                question.Flags[i] = question.Flags[i].Trim().ToLower();
            }

            foreach (var answer in question.Answers)
            {
                string rval = answer.Value;

                if(question.Flags.Contains(CSSE_FQ_F_IGNORECASE))
                {
                    rval = rval.ToLower();
                }

                if (question.Flags.Contains(CSSE_FQ_F_IGNOREWHITESPACE))
                {
                    Regex r = new Regex("\\s+");
                    rval = r.Replace(rval, "");
                }

                @return += CSSE_FQ_CONCATSTR + rval;
            }

            return @return;
        }

        /// <summary>
        /// Generate a random variable name based on a random GUID
        /// </summary>
        /// <returns></returns>
        private string RVariableName()
        {
            string basic = Guid.NewGuid().ToString();
            string token = "magistrate_";
            foreach (char c in basic)
                if (char.IsLetterOrDigit(c))
                    token += c;
            return token;
        }

        /// <summary>
        /// Calculate sha512 digest for input data, then fold to 32 bytes
        /// </summary>
        /// <param name="InputData"></param>
        /// <returns></returns>
        private byte[] SHA512Fold32(byte[] InputData)
        {
            byte[] data = Hasher.ComputeHash(InputData);
            byte[] final = new byte[0x20];

            for (int i = 0; i < 0x20; i++)
                final[i] = (byte)(data[i] ^ data[Math.Min(data.Length - 1, i + 0x20)]);
            
            return final;
        }

        private static byte[] AES(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private string InterpretArgument(string argument)
        {
            if (argument == null)
                return argument;

            if (!argument.StartsWith("#|") || argument.Length < 4)
                return argument;

            argument = argument.Substring(2);

            if (argument.IndexOf("|") < 0)
                return argument;

            string halgo = argument.Substring(0, argument.IndexOf("|"));

            argument = argument.Substring(argument.IndexOf("|") + 1);

            switch (halgo.ToLower())
            {
                case "fnv1a":
                    return Root.FNV1a(argument).ToString();

                default:
                    break;
            }

            return argument;
        }
    }
}
