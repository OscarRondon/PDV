using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Comunes
{
    public class Encriptar
    {
        private static byte[] llave = Encoding.ASCII.GetBytes("S1t3n3m0535t4c&412ff144124412ccc");
        private static byte[] encode = Encoding.ASCII.GetBytes("seiqrfg15-5512d!");


        public Encriptar()
        {
            llave = Encoding.ASCII.GetBytes("S1t3n3m0535t4c&412ff144124412ccc");
            encode = Encoding.ASCII.GetBytes("seiqrfg15-5512d!");
        }
        
        public static string EncriptarString(string inputText)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(inputText);
            byte[] encripted;
            RijndaelManaged cripto = new RijndaelManaged();
            cripto.Padding = PaddingMode.PKCS7;
            cripto.Mode = CipherMode.CBC;
            cripto.KeySize = 128;
            cripto.BlockSize = 128;

            using (MemoryStream ms = new MemoryStream(inputBytes.Length))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(llave, encode), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();
                }
                encripted = ms.ToArray();
            }
            return Convert.ToBase64String(encripted);
        }
    }
}
