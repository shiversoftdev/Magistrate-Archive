using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Installer.Core
{
    /*
    Patching Structures
    #typedef CheckPatch
    ushort PatchSize; //size of the patch
    ushort PatchKey; //key of the check we are patching (so we know which check template to use)
    byte PatchFlags; //any flags for the patch
    byte NumArgs; //How many args does the patch have
    ushort Padding; //Padding to even out the struct size
    string[] Args;
*/
    public sealed class PatchDefinition
    {
        private PatchDefinition()
        {
        }

        private enum PatchFields : int
        {
            PatchSize = 0,
            PatchKey = 2,
            PatchFlags = 4,
            NumArgs = 5,
            Padding = 6,
            Args = 8
        }

        public enum PatchKeys : ushort
        { 
            Firefox = 0,
            ChocoRequest = 1,
            WebRequest = 2,
            RegPatch = 3,
            UserPatch = 4,
            CommandPatch = 5,
            SecEditPatch = 6,
        }

        private List<byte> RawData = new List<byte>();

        public ushort PatchSize
        {
            get
            {
                return (ushort)RawData.Count;
            }
        }

        /// <summary>
        /// The uniqueID of this patch
        /// </summary>
        public ushort PatchKey
        {
            get
            {
                if (RawData.Count <= (int)PatchFields.PatchKey)
                    return 0;
                return BitConverter.ToUInt16(RawData.ToArray(), (int)PatchFields.PatchKey);
            }
            set
            {
                RawData.SetBytes((int)PatchFields.PatchKey, BitConverter.GetBytes(value));
            }
        }

        /// <summary>
        /// Flags for this patch
        /// </summary>
        public byte PatchFlags
        {
            get
            {
                if (RawData.Count <= (int)PatchFields.PatchFlags)
                    return 0;
                return RawData.ToArray()[(int)PatchFields.PatchFlags];
            }
            set
            {
                RawData.SetBytes((int)PatchFields.PatchFlags, new byte[] { 0 });
            }
        }

        /// <summary>
        /// Number of arguments in this patch
        /// </summary>
        public byte NumArgs
        {
            get
            {
                if (RawData.Count <= (int)PatchFields.NumArgs)
                {
                    RawData.SetBytes((int)PatchFields.NumArgs, new byte[] { 0 });
                }

                return RawData.ToArray()[(int)PatchFields.NumArgs];
            }
            set
            {
                RawData.SetBytes((int)PatchFields.NumArgs, new byte[]{ value });
            }
        }

        /// <summary>
        /// Arguments for this patch
        /// </summary>
        public string[] Args
        {
            get
            {
                string[] args = new string[NumArgs];
                if (RawData.Count < (int)PatchFields.Args)
                    return args;
                int index = (int)PatchFields.Args;
                for (int i = 0; i < NumArgs; i++)
                {
                    string Token = "";
                    while( index < RawData.Count && RawData[index] != 0x0)
                    {
                        Token += (char)RawData[index];
                        index++;
                    }
                    index++;
                    args[i] = Token;
                }
                return args;
            }
            set
            {
                int count = 0;

                string[] args = Args ?? new string[] { };

                foreach(string a in args)
                {
                    count += a.Length + 1;
                }

                if(RawData.Count >= (int)PatchFields.Args)
                    RawData.RemoveRange((int)PatchFields.Args, count);

                if (RawData.Count <= (int)PatchFields.Args)
                {
                    RawData.SetBytes((int)PatchFields.Args, new byte[] {});
                }

                if (value == null || value.Length > 255)
                {
                    NumArgs = 0;
                    return;
                }

                NumArgs = (byte)value.Length;

                List<byte> NewStringData = new List<byte>();
                foreach(string s in value)
                {
                    NewStringData.AddRange(Encoding.ASCII.GetBytes(s));
                    NewStringData.Add(0x0);
                }

                RawData.AddRange(NewStringData);
            }
        }

        public static implicit operator byte[](PatchDefinition d)
        {
            d.RawData.SetBytes((int)PatchFields.PatchSize, BitConverter.GetBytes((ushort)d.RawData.Count));
            return d.RawData.ToArray();
        }

        public static implicit operator PatchDefinition(byte[] bytes)
        {
            PatchDefinition d = new PatchDefinition();
            d.RawData.AddRange(bytes);
            return d;
        }

#if DEBUG
        /// <summary>
        /// Create a debug version of a patch
        /// </summary>
        /// <param name="patchkey"></param>
        /// <param name="flags"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static PatchDefinition FromDebug(ushort patchkey, byte flags, string[] args)
        {
            PatchDefinition d = new PatchDefinition();
            d.Args = args;
            d.PatchFlags = flags;
            d.PatchKey = patchkey;
            return d;
        }
#endif

    }
}
