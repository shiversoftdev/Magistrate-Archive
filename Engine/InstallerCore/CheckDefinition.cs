using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Engine.Installer.Core
{
    /// <summary>
    /// A definition for a check
    /// </summary>
    public sealed class CheckDefinition
    {

        //OLD:
        /*
                #CheckDef typedef
                ushort CheckSize;
                uint16 CheckKey; //The key identifier for a vuln
                uint16 CheckID; //The identifier of this specific check for online scoring
                uint16 NumPoints;
                byte Flags;
                byte NumArgs;
                uint OfflineAnswer; //If this is an offline image, the answer to the check (MD5F32 hashed)
                ushort ArgsPtr;
                
                string[NumArgs] Arguments;
        */

        //NEW:
        /*
            #CheckDef typedef
            uint16 CheckSize;
            uint16 CheckKey; //The key identifier for a vuln
            uint16 CheckID; //The identifier of this specific check for online scoring
            uint16 NumPoints;
            byte Flags;
            byte NumArgs;
            uint OfflineAnswersPtr; //If this is an offline image, the pointer to the offline answers.
            byte NumOfflineAnswers;
            byte NumCompareOps;

            uint ArgsPtr;
            uint CompareOpsPtr;
            uint SuccessStringPtr;
            uint FailureStringPtr;

            string[NumArgs] Arguments;
            FOfflineAnswer[NumOfflineAnswers] OfflineAnswers;
            CompareOp[NumCompareOps] CompareOps;
        */
        private enum CheckDefFields
        {
            CheckSize = 0,
            CheckKey = 0x2,
            CheckID = 0x4,
            NumPoints = 0x6,
            Flags = 0x8,
            NumArgs = 0x9,
            OfflineAnswersPtr = 0xA,
            NumOfflineAnswers = 0xE,
            NumCompareOps = 0xF,
            ArgsPtr = 0x10,
            CheckCompareOpsPtr = 0x14,
            SuccessStringPtr = 0x18,
            FailureStringPtr = 0x1C,
        }

        private CheckDefinition()
        {
            RawData.AddRange(new byte[0x20]);
        }

        private List<byte> RawData = new List<byte>();

        /// <summary>
        /// The internalized key to translate this check
        /// </summary>
        public ushort CheckKey
        {
            get
            {
                return BitConverter.ToUInt16(RawData.GetBytes((int)CheckDefFields.CheckKey, sizeof(ushort)), 0);
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.CheckKey, BitConverter.GetBytes(value));
            }
        }
        /// <summary>
        /// The ID of this check for online competitions. You can have multiple Checks with the same ID if you need an extra failure check or something like that
        /// </summary>
        public ushort CheckID
        {
            get
            {
                return BitConverter.ToUInt16(RawData.GetBytes((int)CheckDefFields.CheckID, sizeof(ushort)), 0);
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.CheckID, BitConverter.GetBytes(value));
            }
        }
        /// <summary>
        /// The amount of points to award when the task is complete, to revoke when regressed, or to remove when failed
        /// </summary>
        public short NumPoints
        {
            get
            {
                return BitConverter.ToInt16(RawData.GetBytes((int)CheckDefFields.NumPoints, sizeof(short)), 0);
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.NumPoints, BitConverter.GetBytes(value));
            }
        }
        /// <summary>
        /// The flags of this check
        /// </summary>
        public byte Flags
        {
            get
            {
                while (RawData.Count <= (int)CheckDefFields.Flags)
                    RawData.Add(0x0);
                return RawData[(int)CheckDefFields.Flags];
            }
            set
            {
                while (RawData.Count <= (int)CheckDefFields.Flags)
                    RawData.Add(0x0);
                RawData[(int)CheckDefFields.Flags] = value;
            }
        }
        /// <summary>
        /// The number of arguments to parse. All arguments should be strings
        /// </summary>
        public byte NumArgs
        {
            get
            {
                while (RawData.Count <= (int)CheckDefFields.NumArgs)
                    RawData.Add(0x0);
                return RawData[(int)CheckDefFields.NumArgs];
            }
            private set
            {
                while (RawData.Count <= (int)CheckDefFields.NumArgs)
                    RawData.Add(0x0);
                RawData[(int)CheckDefFields.NumArgs] = value;
            }
        }
        /// <summary>
        /// The offline answers for this check (only valid if installation is offline or debug)
        /// </summary>
        public uint OfflineAnswersPtr
        {
            get
            {
#if OFFLINE
                return BitConverter.ToUInt32(RawData.GetBytes((int)CheckDefFields.OfflineAnswersPtr, sizeof(uint)), 0);
#else
                return 0u;
#endif
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.OfflineAnswersPtr, BitConverter.GetBytes(value));
            }
        }

        /// <summary>
        /// Pointer to Success string (if any). If 0, it means we dont have an override
        /// </summary>
        public uint SuccessStringPtr
        {
            get
            {
                return BitConverter.ToUInt32(RawData.GetBytes((int)CheckDefFields.SuccessStringPtr, sizeof(uint)), 0);
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.SuccessStringPtr, BitConverter.GetBytes(value));
            }
        }

        /// <summary>
        /// Pointer to Failure string (if any). If 0, it means we dont have an override
        /// </summary>
        public uint FailureStringPtr
        {
            get
            {
                return BitConverter.ToUInt32(RawData.GetBytes((int)CheckDefFields.FailureStringPtr, sizeof(uint)), 0);
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.FailureStringPtr, BitConverter.GetBytes(value));
            }
        }

        /// <summary>
        /// Success string, if any. Null otherwise.
        /// </summary>
        public string SuccessString
        {
            get
            {
                if (SuccessStringPtr == 0)
                    return null;
                string result = "";
                for (int i = (int)SuccessStringPtr; i < RawData.Count && RawData.GetBytes(i, 1)[0] != 0x0; i++)
                    result += (char)RawData.GetBytes(i, 1)[0];
                return result;
            }
            set
            {
                if(value == null)
                {
                    SuccessStringPtr = 0;
                    return;
                }

                SuccessStringPtr = (uint)RawData.Count;
                RawData.AddRange(Encoding.ASCII.GetBytes(value));
                RawData.Add(0x0);
            }
        }

        /// <summary>
        /// Failure string, if any. Null otherwise.
        /// </summary>
        public string FailureString
        {
            get
            {
                if (FailureStringPtr == 0)
                    return null;
                string result = "";
                for (int i = (int)FailureStringPtr; i < RawData.Count && RawData.GetBytes(i, 1)[0] != 0x0; i++)
                    result += (char)RawData.GetBytes(i, 1)[0];
                return result;
            }
            set
            {
                if (value == null)
                {
                    FailureStringPtr = 0;
                    return;
                }

                FailureStringPtr = (uint)RawData.Count;
                RawData.AddRange(Encoding.ASCII.GetBytes(value));
                RawData.Add(0x0);
            }
        }

        /// <summary>
        /// Number of offline answers available to this check. Typically 1. If this is 0 and the engine is an offline build, this check will not work.
        /// </summary>
        public byte NumOfflineAnswers
        {
            get
            {
#if OFFLINE
                return RawData.GetBytes((int)CheckDefFields.NumOfflineAnswers, 1)[0];
#else
                return 0u;
#endif
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.NumOfflineAnswers, new byte[] { value });
            }
        }

        public FOfflineAnswer[] OfflineAnswers
        {
            get
            {
                if(OfflineAnswersPtr < 1)
                    return new FOfflineAnswer[0];

                FOfflineAnswer[] answrs = new FOfflineAnswer[NumOfflineAnswers];

                if(NumOfflineAnswers < 1)
                    return answrs;

                int index = (int)OfflineAnswersPtr;

                for(int i = 0; i < NumOfflineAnswers; i++)
                {
                    answrs[i] = (RawData, index);
                    index += answrs[i].SIZE;
                }

                return answrs;
            }
            set
            {
                FOfflineAnswer[] old = OfflineAnswers;
                int index = (int)OfflineAnswersPtr;
                for(int i = 0; i < old.Length; i++)
                {
                    for(int j = 0; j < old[i].SIZE; i++)
                    {
                        RawData[i + j] = 0xFF;
                    }
                    index += old.Length;
                }

                OfflineAnswersPtr = (uint)RawData.Count;
                NumOfflineAnswers = (byte)value.Length;

                foreach (var f in value)
                {
                    RawData.AddRange((byte[])f);
                }
            }
        }

        /// <summary>
        /// The size of the raw check data
        /// </summary>
        public ushort CheckSize
        {
            get
            {
                return BitConverter.ToUInt16(RawData.GetBytes((int)CheckDefFields.CheckSize, sizeof(ushort)), 0);
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.CheckSize, BitConverter.GetBytes(value));
            }
        }
        /// <summary>
        /// The arguments pointer for the check
        /// </summary>
        public ushort ArgsPtr
        {
            get
            {
                return BitConverter.ToUInt16(RawData.GetBytes((int)CheckDefFields.ArgsPtr, sizeof(ushort)), 0);
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.ArgsPtr, BitConverter.GetBytes(value));
            }
        }
        /// <summary>
        /// The arguments of this check
        /// </summary>
        public string[] Arguments
        {
            get
            {
                if (ArgsPtr == 0 || NumArgs == 0)
                    return new string[0];
                string[] args = new string[NumArgs];
                int index = ArgsPtr;
                for (int i = 0; i < NumArgs; i++)
                {
                    args[i] = RawData.ReadString(ref index);
                }
                return args;
            }
            set
            {
                if (ArgsPtr == 0)
                    return;
                foreach (string s in Arguments)
                {
                    RawData.RemoveRange(ArgsPtr, s.Length + 1);
                }
                NumArgs = (byte)value.Length;

                for (int i = value.Length - 1; i > -1; i--)
                {
                    RawData.Insert(ArgsPtr, 0);
                    RawData.InsertRange(ArgsPtr, Encoding.ASCII.GetBytes(value[i]));
                }
            }
        }

        /// <summary>
        /// Number of compareops for this check
        /// </summary>
        public byte NumCompareOps
        {
            get
            {
                return RawData.GetBytes((int)CheckDefFields.NumCompareOps, 1)[0];
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.NumCompareOps, new byte[] { value });
            }
        }

        /// <summary>
        /// Pointer to the compareops for this check
        /// </summary>
        public uint CompareOpsPtr
        {
            get
            {
                return BitConverter.ToUInt32(RawData.GetBytes((int)CheckDefFields.CheckCompareOpsPtr, 4), 0);
            }
            set
            {
                RawData.SetBytes((int)CheckDefFields.CheckCompareOpsPtr, BitConverter.GetBytes(value));
            }
        }

        /// <summary>
        /// The compare operations assigned to this check
        /// </summary>
        public FCompareOp[] CompareOps
        {
            get
            {
                if (CompareOpsPtr == 0 || NumCompareOps < 1)
                    return new FCompareOp[0];

                FCompareOp[] compares = new FCompareOp[NumCompareOps];

                for(int i = 0; i < NumCompareOps; i++)
                {
                    compares[i] = RawData.GetBytes((int)CompareOpsPtr + (i * 3), 3);
                }

                return compares;
            }
            set
            {
                if (value.Length > 255)
                    return;

                if (CompareOpsPtr != 0 && NumCompareOps > 0)
                {
                    for (int i = 0; i < NumCompareOps; i++)
                    {
                        RawData.SetBytes((int)CompareOpsPtr + (i * 3), new byte[] { 0xFF, 0xFF, 0xFF });
                    }
                }

                CompareOpsPtr = (uint)RawData.Count;
                NumCompareOps = (byte) value.Length;

                for(int i = 0; i < value.Length; i++)
                {
                    RawData.SetBytes((int)CompareOpsPtr + (i * 3), value[i]);
                }
            }
        }

        /// <summary>
        /// Get the raw data of a check definition
        /// </summary>
        /// <param name="d"></param>
        public static implicit operator byte[] (CheckDefinition d)
        {
            return d.RawData.ToArray();
        }

        /// <summary>
        /// Create a check definition from a data source and a pointer
        /// </summary>
        /// <param name="IData">Data, followed by the data pointer</param>
        public static implicit operator CheckDefinition((List<byte>, uint) IData)
        {
            List<byte> Source = IData.Item1;
            CheckDefinition check = new CheckDefinition();
            int fileoffset = (int)IData.Item2;
            try
            {
                check.RawData = Source.GetBytes(fileoffset, sizeof(ushort)).ToList(); //Force feed the size
                check.RawData.AddRange(Source.GetBytes(fileoffset + sizeof(ushort), check.CheckSize - sizeof(ushort))); //Pipe in the rest, accounting for the 2 bytes we force fed before
                return check;
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// Add an argument to the check definition
        /// </summary>
        /// <param name="c">The check to add an argument to</param>
        /// <param name="argument">The argument to add</param>
        /// <returns></returns>
        public static CheckDefinition operator +(CheckDefinition c, string argument)
        {
            if (c != null)
                c.AddArgument(argument);
            return c;
        }

        public static CheckDefinition operator -(CheckDefinition c, string argument)
        {
            if (c != null)
                c.RemoveArgument(argument);
            return c;
        }

        /// <summary>
        /// Add an argument to the list of args
        /// </summary>
        /// <param name="arg"></param>
        internal void AddArgument(string arg)
        {
            if (arg == null)
                return;
            List<string> args = new List<string>(Arguments);
            args.Add(arg);
            Arguments = args.ToArray();
        }

        /// <summary>
        /// Remove an argument from the list of args
        /// </summary>
        /// <param name="arg">The argument to remove</param>
        internal void RemoveArgument(string arg)
        {
            if (arg == null)
                return;
            List<string> args = new List<string>(Arguments);
            args.Remove(arg);
            Arguments = args.ToArray();
        }

        public sealed class FOfflineAnswer
        {
            public Engine.Core.EngineFrame.ACompareType CompareType;
            public ushort AnswerLength
            {
                get
                {
                    return (ushort) AnswerData.Length;
                }
            }

            public byte[] AnswerData;

            public static implicit operator byte[](FOfflineAnswer f)
            {
                List<byte> bytes = new List<byte>();
                bytes.Add((byte)f.CompareType);
                bytes.AddRange(BitConverter.GetBytes(f.AnswerLength));
                bytes.AddRange(f.AnswerData);
                return bytes.ToArray();
            }

            public static implicit operator FOfflineAnswer((List<byte> bytes, int index) data)
            {
                FOfflineAnswer f = new FOfflineAnswer();
                f.CompareType = (Engine.Core.EngineFrame.ACompareType) data.bytes.GetBytes(data.index, 1)[0];
                ushort datalength = BitConverter.ToUInt16(data.bytes.ToArray(), data.index + 1);
                f.AnswerData = data.bytes.GetBytes(data.index + 3, datalength);
                return f;
            }

            public static FOfflineAnswer[] ToAnswers(string[] s)
            {
                FOfflineAnswer[] f = new FOfflineAnswer[s.Length];
                for (int i = 0; i < s.Length; i++)
                    f[i] = s[i];
                return f;
            }

            public static implicit operator FOfflineAnswer(string s)
            {
                string[] split = s.Split(',');
                try
                {
                    if (split.Length < 2)
                    {
                        return new FOfflineAnswer() { CompareType = Engine.Core.EngineFrame.ACompareType.EQ, AnswerData = StringToHex(split[0]) };
                    }

                    Enum.TryParse(split[0], true, out Engine.Core.EngineFrame.ACompareType comptype);
                    return new FOfflineAnswer() { CompareType = comptype, AnswerData = StringToHex(split[1]) };
                }
                catch
                {
                    return null;
                }
            }

            private static byte[] StringToHex(string s)
            {
                if (s.Length % 2 == 1)
                    s = "0" + s;

                byte[] bytes = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                    bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);

                return bytes;
            }

            public int SIZE
            {
                get
                {
                    return 3 + AnswerLength;
                }
            }

        }

        private void Pack() //consolidates check data //TODO
        {

        }

        /// <summary>
        /// Multi-check comparison operation definition
        /// </summary>
        public sealed class FCompareOp
        {
            public ECompareOp CompareOp;
            public ushort Child;

            public static implicit operator byte[](FCompareOp f)
            {
                byte[] bytes = BitConverter.GetBytes(f.Child);
                return new byte[] { (byte)f.CompareOp, bytes[0], bytes[1] };
            }

            public static implicit operator FCompareOp(byte[] bytes)
            {
                if (bytes.Length != 3)
                    throw new ArgumentException("FCompareOp requires a 3 byte array to be created");

                return new FCompareOp() { Child = BitConverter.ToUInt16(bytes, 1), CompareOp = (ECompareOp) bytes[0] };
            }
        }

        /// <summary>
        /// Enumeration of possible comparison operations
        /// </summary>
        public enum ECompareOp
        {
            /// <summary>
            /// Check AND check must be met for this check to be considered.
            /// </summary>
            AND,
            /// <summary>
            /// Check OR check will either be considered for this check to be true
            /// </summary>
            OR,
            /// <summary>
            /// Check must be met BEFORE child will be considered. Does not own the child.
            /// </summary>
            BEFORE
        }

#if DEBUG
        /// <summary>
        /// Create a debug check. Only exists in a debug build
        /// </summary>
        /// <param name="type">Type of check to implement</param>
        /// <param name="ID">The ID of the check</param>
        /// <param name="Points">The amount of points</param>
        /// <param name="Flags">The flags of the check (can be 0)</param>
        /// <param name="args">Aruments to pass to the check</param>
        /// <returns></returns>
        public static CheckDefinition DebugCheck(CheckTypes type, ushort ID, short Points, byte Flags, FOfflineAnswer[] OfflineAnswers, FCompareOp[] CompareOps, string SuccessString, string FailureString, params string[] args)
        {
            CheckDefinition c = new CheckDefinition();
            c.CheckKey = (ushort)type;
            c.CheckID = ID;
            c.NumPoints = Points;
            c.Flags = Flags;
            c.CheckSize = 0;
            c.ArgsPtr = (ushort)c.RawData.Count; //super weird bug, this overwrites the arg ptr if the byte array is smaller before it adds the bytes.
            c.Arguments = args;
            c.OfflineAnswers = OfflineAnswers;
            c.CheckSize = (ushort)c.RawData.Count;
            c.CompareOps = CompareOps;
            c.SuccessString = SuccessString;
            c.FailureString = FailureString;
            c.Pack();
            return c;
        }
#endif
    }
}
