using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Domain.Extension
{
    public static class MD5Crypto
    {
        private static readonly string _securityKey = "Ah6YPgsTtqA0OMtBxc0eGuFyEUecqGlm";

        public static string Encrypt(this string value)
        {
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(value);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));

            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            objTripleDESCryptoService.Key = securityKeyArray;

            objTripleDESCryptoService.Mode = CipherMode.ECB;

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);

            objTripleDESCryptoService.Clear();

            return Convert.ToBase64String(resultArray);
        }

        public static string Decrypt(this string value)
        {
            byte[] toEncryptArray = Convert.FromBase64String(value);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));

            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            objTripleDESCryptoService.Key = securityKeyArray;

            objTripleDESCryptoService.Mode = CipherMode.ECB;

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            objTripleDESCryptoService.Clear();

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
