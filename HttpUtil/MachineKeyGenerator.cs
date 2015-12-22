using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtil
{
    /// <summary>
    /// Provides helper method for randomly generating ASP.NET Web.Config machine key.
    /// </summary>
    public static class MachineKeyGenerator
    {
        /// <summary>
        /// Randomly generates new machine key configuration line 
        /// for ASP.NET Web.Config using AES decryption key and HMACSHA256 validation key.
        /// </summary>
        /// <returns></returns>
        public static string Generate()
        {
            var template = "<machineKey validationKey=\"{0}\" decryptionKey=\"{2}\"/>";

            using (var rijndael = new RijndaelManaged())
            using (var hmacsha256 = new HMACSHA256())
            {
                rijndael.GenerateKey();
                hmacsha256.Initialize();
                var decryptionKey = new SoapHexBinary(rijndael.Key);
                var validationKey = new SoapHexBinary(hmacsha256.Key);
                return string.Format(template, validationKey, decryptionKey);
            }
        }
    }
}
