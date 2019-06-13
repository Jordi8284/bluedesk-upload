using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BluedeskUpload
{
    public class Encryption
    {
        public static string EncryptDecrypt(string content, int encryptionKey)
        {
            var outStringBuild = new StringBuilder(content.Length);

            for (int iCount = 0; iCount < content.Length; iCount++)
            {
                outStringBuild.Append((char)(content[iCount] ^ encryptionKey));
            }

            return outStringBuild.ToString();
        }

        public static string Encrypt(string content, int encryptionKey)
        {
            var result = EncryptDecrypt(content, encryptionKey);

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(result)).Replace("/", "_");
        }

        public static string Decrypt(string content, int encryptionKey)
        {
            var result = Encoding.UTF8.GetString(Convert.FromBase64String(content.Replace("/", "_")));
            return EncryptDecrypt(result, encryptionKey);
        }
    }
}