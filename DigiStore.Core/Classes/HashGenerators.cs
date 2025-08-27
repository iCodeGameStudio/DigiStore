using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DigiStore.Core.Classes
{
    public class HashGenerators
    {
        public static string MD5Encoding(string password)
        {
            Byte[] mainBytes;
            Byte[] encodeBytes;
            mainBytes = ASCIIEncoding.Default.GetBytes(password);
            using (var md5 = MD5.Create())
            {
                encodeBytes = md5.ComputeHash(mainBytes);
            }
            return BitConverter.ToString(encodeBytes);
        }
    }
}
