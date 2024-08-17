using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Magistrate.BuildTools
{
    public sealed class ConfigManager
    {
        internal string Message = "";
        internal MBuildConfig Config { get; private set; }
        public bool Parse(string config)
        {
            if (!File.Exists(config))
                return false;

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowerNamePol(),
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            try { Config = JsonSerializer.Deserialize<MBuildConfig>(File.ReadAllBytes(config), serializeOptions); }
            catch
#if DEBUG
            (Exception e)
#endif
            {
                Message = "JSON deserialization failed";
#if DEBUG
                Message += "\n\t" + e.ToString();
#endif
                return false; 
            }

            return true;
        }
    }

    internal class LowerNamePol : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            name.ToLower();
    }

    public sealed class MBuildConfig
    {
        /// <summary>
        /// Target platform for this configuration
        /// </summary>
        public MPlatform Platform { get; set; }

        /// <summary>
        /// Check data for this build
        /// </summary>
        public MCheckConfig[] Checks { get; set; }

        /// <summary>
        /// Scoring data for this build
        /// </summary>
        public MScoringData[] Scoring { get; set; }
        
        /// <summary>
        /// Forensics data for this build
        /// </summary>
        public MForensicsQuestion[] Forensics { get; set; }

        /// <summary>
        /// Attempt to resolve a check by its CID.
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="AssertFatal"></param>
        /// <returns></returns>
        public static MCheckConfig ResolveCheckByCID(string CID, MCheckConfig[] Checks, bool AssertFatal = false)
        {
            foreach (var check in Checks)
                if (check.CID == CID)
                    return check;

            if (AssertFatal)
                Root.Error($"Check with ID '{CID}' does not exist.");
            
            return null;
        }

        public class MCheckConfig
        {
            /// <summary>
            /// ID of the check module. Case sensitive.
            /// </summary>
            public string Check { get; set; }
            
            /// <summary>
            /// Determines if check uses param data or is randomized
            /// </summary>
            public bool Random { get; set; }

            /// <summary>
            /// Operation string, csv for operators with operands
            /// </summary>
            public string Operation { get; set; }

            /// <summary>
            /// Check ID, used only at generation time to resolve the expected salt for answer calculation
            /// </summary>
            public string CID { get; set; }

            /// <summary>
            /// Check Data Dictionary
            /// </summary>
            public Dictionary<string, string> Data { get; set; }

            /// <summary>
            /// Check ID, used primarily in engine generation.
            /// </summary>
            [JsonIgnore]
            public string RCID { get; set; }

            /// <summary>
            /// Check salt value, used primarily in engine generation.
            /// </summary>
            [JsonIgnore]
            public string ISALT { get; set; }
        }

        public class MScoringData
        {
            /// <summary>
            /// Value of this check's complete condition. 0 means it is a supplimentary check (and/or). Negative means penalty.
            /// </summary>
            public short Points { get; set; }

            /// <summary>
            /// Query data for the database. List of module names, check ids, etc. Star means a full DB check. No querystring means pure constrained check.
            /// </summary>
            public string[] Query { get; set; }

            /// <summary>
            /// Operator constraints for a scoring item. KVP[] of [after|and|or|not]:CI2
            /// </summary>
            public string[] Constraints { get; set; }

            /// <summary>
            /// Message for scoring data when this check is completed. If pointvalue eq 0, emits a random string of adequate length.
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Secret key embedded in encryption message used for verification purposes.
            /// </summary>
            public string CI2 { get; set; }

            /// <summary>
            /// Per-check answer string data. Includes *any* relevant data for the generator/runtime of a scoring item.
            /// </summary>
            public string Answer { get; set; }

            /// <summary>
            /// CID of the check that represents the answer (generation time only so that the answer string is salted)
            /// </summary>
            public string AnswerCID { get; set; }

            /// <summary>
            /// If this scoring entry is stateless, it will try to decrypt from its children
            /// </summary>
            public bool Stateless { get; set; }

            /// <summary>
            /// Random generated GUID used as proof that the decrypted string is valid
            /// </summary>
            [JsonIgnore]
            public string DecryptKey { get; set; }

            /// <summary>
            /// Initialization vector for AES encryption
            /// </summary>
            [JsonIgnore]
            public byte[] IV { get; set; }

            /// <summary>
            /// Generation time name for the variable in a local scope, used for constraint mapping.
            /// </summary>
            [JsonIgnore]
            public string VariableName { get; set; }

            /// <summary>
            /// Generation time tree used for constraint validation
            /// </summary>
            internal HashSet<MScoringData> ChildrenConstraints = new HashSet<MScoringData>();

            /// <summary>
            /// Gneration time constraints tree for final emission
            /// </summary>
            internal Dictionary<string, string> GeneratedConstraints = new Dictionary<string, string>();

            public bool ValidHierarchy(List<MScoringData> AccumulatedPath)
            {
                AccumulatedPath.Add(this);
                foreach (var child in ChildrenConstraints)
                    if (!child.ValidHierarchy(AccumulatedPath))
                        return false;
                AccumulatedPath.Remove(this);
                
                return true;
            }
        }

        public class MForensicsQuestion
        {
            /// <summary>
            /// Forensics unique identifier used to link forensics answers to questions
            /// </summary>
            public byte ID { get; set; }

            /// <summary>
            /// Number of points that this forensics question is worth. Negative values will throw a compilation error.
            /// </summary>
            public short Points { get; set; }

            /// <summary>
            /// Scoring messge to emit when the check is complete
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// All correct answers to this question
            /// </summary>
            public MForensicsAnswer[] Answers { get; set; }

            /// <summary>
            /// Question to ask in rich text format
            /// </summary>
            public string Question { get; set; }

            /// <summary>
            /// Check integrity identifier
            /// </summary>
            public string CI2 { get; set; }

            /// <summary>
            /// Flags for how we score the check
            /// </summary>
            public string[] Flags { get; set; }
        }

        public class MForensicsAnswer
        {
            /// <summary>
            /// Value of this answer. No matter the answer, this should be a string.
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// The type of answer to receive
            /// </summary>
            public ForensicsAnswerType Type { get; set; }

            /// <summary>
            /// Format string for text input forensics. Ignored for value based questions
            /// </summary>
            public string Format { get; set; }

            /// <summary>
            /// Label to prefix the answer input with
            /// </summary>
            public string Label { get; set; }
        }
    }

    /// <summary>
    /// Types of valid values for a forensics question answer
    /// </summary>
    public enum ForensicsAnswerType : uint
    {
        BoolValue = 0,
        IntValue = 1,
        FloatValue = 2,
        SingleLineFormatted = 3
    };

    /// <summary>
    /// Target platform for configuration
    /// </summary>
    public enum MPlatform
    {
        Windows,
        Debian
    }
}
