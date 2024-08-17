using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace Magistrate.BuildTools
{
    //https://docs.microsoft.com/en-us/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2019
    class Root
    {
        private static readonly Dictionary<string, Type> CommandList = new Dictionary<string, Type>()
        {
            { "build", typeof(CLI.Build) },
            { "fsd", typeof(CLI.FSDiffer) },
            { "filesig", typeof(CLI.FileSig) },
            { "admc", typeof(CLI.ADMCompiler) },
            { "sidcapture", typeof(CLI.SIDCapture) }
        };

        public static ulong FNV1a(string str)
        {
            const ulong fnv64Offset = 14695981039346656037;
            const ulong fnv64Prime = 0x100000001b3;
            ulong hash = fnv64Offset;

            byte[] bytes = Encoding.ASCII.GetBytes(str.ToLower());
            for (var i = 0; i < bytes.Length; i++)
            {
                hash = hash ^ bytes[i];
                hash *= fnv64Prime;
            }

            return hash;
        }

        internal static Random Randomization = new Random();

        internal static ScoringManifest GlobalManifest;


        //Command line interface for Magistrate Generation
        //Syntax: Magistrate.BuildTools.exe [--opt [opt args]]+
        static void Main(string[] args)
        {
            if(args.Length < 1)
                Error();

            string ManifestPath = args[0];

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowerNamePol(),
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            if (!File.Exists(ManifestPath))
                Error($"Application manifest not found ({ManifestPath}).");

            try { GlobalManifest = JsonSerializer.Deserialize<ScoringManifest>(File.ReadAllBytes(ManifestPath), serializeOptions); }
            catch
#if DEBUG
            (Exception e)
#endif
            {
#if DEBUG
                Error($"Invalid manifest provided ({e.ToString()})");
#endif
                Error($"Invalid manifest provided. (Manifest deserialization failed)"); 
            }

            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] == "--")
                    continue; //handles spaces after --

                string ckey = args[i++].Trim().Replace("--", "");

                if(!CommandList.ContainsKey(ckey))
                {
                    Error($"Command '{ckey}' was not found");
                }

                Command c = (Command)Activator.CreateInstance(CommandList[ckey]);

                int j = 0;
                string[] c_args = new string[c.NumArgs];
                while(j < c.NumArgs)
                {
                    if (j + i >= args.Length)
                        Error($"Command '{ckey}' requires more arguments");

                    c_args[j] = args[i + j];

                    j++;
                }
                i += j;

                if (!c.Exec(c_args))
                    Error(c.Message);
            }
        }

        public static void Error(string msg = "")
        {
            PrintHelpMSG();
            Console.WriteLine(msg);
            Environment.Exit(1);
        }

        private static void PrintHelpMSG()
        {
            Console.WriteLine("Syntax: Magistrate.BuildTools.exe ManifestPath [--opt [opt args]]+");
            Console.WriteLine("Options:");
            foreach(var key in CommandList.Keys)
            {
                Console.WriteLine($"\t--{key}\t{(Activator.CreateInstance(CommandList[key]) as dynamic).CommandDescription}");
            }
        }
    }

    internal abstract class Command
    {
        public virtual int NumArgs => 0;
        public virtual string CommandDescription => "No description";
        public string Message = "Command completed successfully";
        public abstract bool Exec(string[] args);
    }
}
