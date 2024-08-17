using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Magistrate.Core
{
    internal static class @const
    {
        /// <summary>
        /// Suffix const for all mutexes
        /// </summary>
        public const string CSSE_MUTEX_SUFFIX = "MUTEX";

        /// <summary>
        /// Client in pipe name
        /// </summary>
        public const string CSSE_PCI = "PCI";

        /// <summary>
        /// Client out pipe name
        /// </summary>
        public const string CSSE_PCO = "PCO";

        /// <summary>
        /// Gamestate pipe name
        /// </summary>
        public const string CSSE_PGS = "PGS";

        /// <summary>
        /// Client authority string
        /// </summary>
        public const string CSSE_COM_CLIENT = "1";

        /// <summary>
        /// Server authority string
        /// </summary>
        public const string CSSE_COM_ENGINE = "0";

        /// <summary>
        /// Maximum number of buffered commands before dropping the command array
        /// </summary>
        public const int CSSE_COM_MAXQUEUE = 64;

        /// <summary>
        /// String to append when concatenating forensics answers
        /// </summary>
        public const string CSSE_FQ_CONCATSTR = "&answer=";

        /// <summary>
        /// Flag for ignoring the case of answers in a forensics question
        /// </summary>
        public const string CSSE_FQ_F_IGNORECASE = "ignorecase";

        /// <summary>
        /// Flag for ignoring all whitespace in the answers of a forensics question
        /// </summary>
        public const string CSSE_FQ_F_IGNOREWHITESPACE = "ignorews";

        /// <summary>
        /// Forensics filename suffix
        /// </summary>
        public const string CSSE_PCF = "CFQ";

        /// <summary>
        /// Size of the question rich text for a forensics question
        /// </summary>
        public const int SIZEOF_QRICHTEXT = 4096;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string CSSE_MUTEX_STR(string mutex, string inst)
        {
            return inst + "." + CSSE_MUTEX_SUFFIX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string CSSE_FQNAME(string FQID)
        {
            return FQID + "." + CSSE_PCF;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        public const string CSSE_CMD_NotifyEvent = "CMD_NotifyEvent";
        public const string CSSE_CMD_ForensicsParse = "CMD_ForensicsParse";
    }
}
