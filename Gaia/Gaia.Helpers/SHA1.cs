using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Helpers
{
    public class SHA1
    {
        public static string Encode(string value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }

        /// <summary>
        /// take any string and encrypt it using SHA1 then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        public static string GetSHA1HashData(string data)
        {
            var sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(data));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString().ToUpper();

            ////create new instance of md5
            //System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();

            ////convert the input text to array of bytes
            //byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            ////create new instance of StringBuilder to save hashed data
            //StringBuilder returnValue = new StringBuilder();

            ////loop for each byte and add it to StringBuilder
            //for (int i = 0; i < hashData.Length; i++)
            //{
            //    returnValue.Append(hashData[i].ToString());
            //}

            //// return hexadecimal string
            //return returnValue.ToString();
        }

        /// <summary>
        /// encrypt input text using SHA1 and compare it with
        /// the stored encrypted text
        /// </summary>
        /// <param name="inputData">input text you will enterd to encrypt it</param>
        /// <param name="storedHashData">the encrypted
        ///           text stored on file or database ... etc</param>
        /// <returns>true or false depending on input validation</returns>

        public static bool ValidateSHA1HashData(string inputData, string storedHashData)
        {
            //hash input text and save it string variable
            string getHashInputData = GetSHA1HashData(inputData);

            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
