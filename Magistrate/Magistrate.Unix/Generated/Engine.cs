using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace Magistrate.Unix.Generated
{
    internal sealed class Engine : Core.Engine
    {
        
		private Modules.ForensicsModule _ForensicsModule;


        internal Engine() : base()
        {
            

        }

        protected override void StartEngine()
        {
            
			_ForensicsModule?.StartModule();

        }

        /*<conf.methods>*/
        /*<conf.classes>*/
    }

    internal static class Extensions
    {
        private static byte[] Key = new byte[]
        { 37, 212, 74, 116, 200, 111, 85, 65, 183, 22, 62, 104, 124, 179, 43, 184 };

        private static byte[] IV;

        private static List<byte> PackData = new List<byte>();

        static Extensions()
        {
            byte[] bitdata;
            var dstream = System.Reflection.Assembly.GetExecutingAssembly().
                GetManifestResourceStream("Magistrate.Unix.Generated.const.pack");
            bitdata = new byte[dstream.Length];
            dstream.Read(bitdata, 0, (int)dstream.Length);
            dstream.Close();

            PackData.AddRange(bitdata);

            IV = PackData.GetRange(0, 16).ToArray();
        }

        private static string DC(byte[] cipherText)
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

        public static string u(this int idx, int Len)
        {
            if ((idx + Len) > PackData.Count)
                return null;

            try { return DC(PackData.GetRange(idx, Len).ToArray()); }
            catch { }

            return null;
        }
    }
}