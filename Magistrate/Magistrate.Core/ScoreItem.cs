using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Magistrate.Core
{
    /// <summary>
    /// Score item that queries database for a state
    /// </summary>
    public sealed class ScoreItem
    {
        private bool __itemstate = false;
        private bool __prevstate = false;
        /// <summary>
        /// Is this state dirty (changed since last evaluation)
        /// </summary>
        internal bool ItemDirty 
        {
            get
            {
                return __prevstate != ItemState;
            }
        }

        public void CleanState()
        {
            __prevstate = ItemState;
        }

        /// <summary>
        /// Internal persistent state for the item
        /// </summary>
        internal bool ItemState
        {
            get
            {
                return EvalConstraints();
            }
            set
            {
                __itemstate = value;
            }
        }

        /// <summary>
        /// Internal identifier to be used only for reporting the gamestate.
        /// </summary>
        internal int __id;

        /// <summary>
        /// Value to adjust score by when this score item meets its expected condition
        /// If points are non-zero, record this event on change
        /// </summary>
        public short PointValue;

        /// <summary>
        /// If the  decrypted result contains this key, consider the check condition met.
        /// </summary>
        public string DecryptKey;

        /// <summary>
        /// Initialization vector for AES
        /// </summary>
        public byte[] IV;

        /// <summary>
        /// The string to attempt to decrypt;
        /// AES encrypted MSG.CI2.DKEY
        /// </summary>
        public byte[] DecryptString;

        /// <summary>
        /// Relative constraints for evaluating this check based on child references
        /// </summary>
        public Dictionary<ScoreItem, string> Constraints;

        /// <summary>
        /// List of query items for a scoring entry
        /// </summary>
        public HashSet<CheckInfo> Query = new HashSet<CheckInfo>();

        /// <summary>
        /// Is this entry stateless; Requires a child object decryptor
        /// </summary>
        public bool IsStateless = false;

        /// <summary>
        /// The report string for this check
        /// </summary>
        internal string ReportString { get; private set; }

        internal string AnswerString
        {
            get
            {
                if (ReportString == null || ReportString.LastIndexOf(";") < 0)
                    return "";

                return ReportString.Substring(0, ReportString.LastIndexOf(";"));
            }
        }

        /// <summary>
        /// Attempt to decrypt the status message
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        internal bool TryDecrypt(byte[] Key)
        {
            ReportString = "";

            try
            {
                ReportString = DecryptStringFromBytes(DecryptString, Key, IV);

                if (ReportString == null || !ReportString.EndsWith(DecryptKey))
                {
                    ItemState = false;
                    return false;
                }

                //Purge decrypt key from end
                ReportString = ReportString.Substring(0, ReportString.Length - DecryptKey.Length - 1);
            }
            catch
            {
                ItemState = false;
                return false; //If there was some decrypt exception, its the wrong key
            }

            ItemState = true;
            return true;
        }

        

        internal void InvokeClear()
        {
            ReportString = "";
            ItemState = false;
        }

        private bool EvalConstraints()
        {
            bool EndResult = !IsStateless && __itemstate;

            foreach(var constraint in Constraints)
            {
                switch(constraint.Value.ToLower())
                {
                    case "after":
                    case "and":
                        if (!constraint.Key.ItemState)
                            return false;
                        break;

                    case "or":
                        EndResult = EndResult || constraint.Key.ItemState;
                        break;

                    case "not":
                        if (constraint.Key.ItemState)
                            return false;
                        break;

                    default: //invalid constraint, ignore
                        break;
                }
            }

            return EndResult;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        internal void CopyTo(ScoringEntry ScoreState)
        {
            if(ItemState && PointValue != 0)
            {
                var splitindex = ReportString.LastIndexOf(';');

                var strmsg = ReportString.Substring(0, splitindex);
                var ci2 = ReportString.Substring(splitindex + 1);

                if (splitindex > -1)
                    ScoreState.SetScoreData(strmsg, ci2);
            }
            else
            {
                ScoreState.SetScoreData();
            }

            ScoreState.EntryData.ID = (uint)__id;
            ScoreState.EntryData.ScoreValue = PointValue;
        }
    }
}
