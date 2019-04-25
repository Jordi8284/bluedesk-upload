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
    }
}