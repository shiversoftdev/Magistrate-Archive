using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace Magistrate/*<conf.architecture>*/.Generated
{
    internal sealed class Engine : Core.Engine
    {
        /*<conf.fields>*/

        internal Engine() : base()
        {
            /*<conf.constructor>*/
        }

        protected override void StartEngine()
        {
            /*<conf.startengine>*/
        }

        /*<conf.methods>*/
        /*<conf.classes>*/
    }

    internal static class Extensions
    {
        private static byte[] Key = new byte[]
        { /*<conf.stringkey>*/ };

        private static byte[] IV;

        private static List<byte> PackData = new List<byte>();

        static Extensions()
        {
            byte[] bitdata;
            var dstream = System.Reflection.Assembly.GetExecutingAssembly().
                GetManifestResourceStream("Magistrate/*<conf.architecture>*/.Generated.const.pack");
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