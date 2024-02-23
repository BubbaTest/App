using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Helpers
{
    public class Encriptar
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
        public static string GetHashData(string data, string key, string alg)
        {
            string ComputeHash = string.Empty;
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            ASCIIEncoding encoding = new ASCIIEncoding();

            switch (alg.ToUpper())
            {
                case "SHA1":
                    var sha1 = SHA1Managed.Create();
                    
                    stream = sha1.ComputeHash(encoding.GetBytes(key + data));
                    for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                    ComputeHash = sb.ToString().ToUpper();
                    
                    break;

                case "SHA256":
                    var sha256 = SHA256Managed.Create();
                    stream = sha256.ComputeHash(encoding.GetBytes(key + data));
                    for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                    ComputeHash = sb.ToString().ToUpper();

                    break;

                case "SHA384":
                    var sha384 = SHA384Managed.Create();
                    stream = sha384.ComputeHash(encoding.GetBytes(key + data));
                    for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                    ComputeHash = sb.ToString().ToUpper();

                    break;

                case "SHA512":
                    var sha512 = SHA512Managed.Create();
                    stream = sha512.ComputeHash(encoding.GetBytes(key + data));
                    for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                    ComputeHash = sb.ToString().ToUpper();

                    break;

                case "MD5":
                    var md5 = MD5.Create();
                    stream = md5.ComputeHash(encoding.GetBytes(key + data));
                    for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                    ComputeHash = sb.ToString().ToUpper();

                    break;

                case "BCRYPT":                    
                    ComputeHash = BCrypt.Net.BCrypt.HashPassword(key + data, 10, false);

                    break;

                default:
                    break;
            }
            return ComputeHash;            

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

        protected string GetSHA1ComputeHash(string data, string key)
        {
            var sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(key + data));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString().ToUpper();
        }


        /// <summary>
        /// encrypt input text using SHA1 and compare it with
        /// the stored encrypted text
        /// </summary>
        /// <param name="inputData">input text you will enterd to encrypt it</param>
        /// <param name="storedHashData">the encrypted
        ///           text stored on file or database ... etc</param>
        /// <returns>true or false depending on input validation</returns>

        public static bool ValidateHashData(string inputData, string storedHashData, string key, string alg)
        {
            if (alg != "BCRYPT")
            {
                //hash input text and save it string variable
                string getHashInputData = GetHashData(inputData, key, alg);

                if (string.Compare(getHashInputData, storedHashData) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            { return BCrypt.Net.BCrypt.Verify(key + inputData, storedHashData, false); }
            
        }

        /**
         * Cifra una cadena texto con el algoritmo de Rijndael
         *
         * @param	plainMessage	mensaje plano (sin cifrar)
         * @param	Key		        clave del cifrado para Rijndael
         * @param	IV		        vector de inicio para Rijndael
         * @return	string		    texto cifrado
         */
        public static string encryptStringRijndael(String plainMessage, String strKey, String strIv)
        {
            byte[] Key = UTF8Encoding.UTF8.GetBytes(strKey);
            byte[] IV = UTF8Encoding.UTF8.GetBytes(strIv);

            //Estableciendo longuitud válida del arreglo
            int keySize = 32;
            int ivSize = 16;

            Array.Resize(ref Key, keySize);
            Array.Resize(ref IV, ivSize);

            // Crear una instancia del algoritmo de Rijndael
            Rijndael RijndaelAlg = Rijndael.Create();

            // Establecer un flujo en memoria para el cifrado
            MemoryStream memoryStream = new MemoryStream();

            // Crear un flujo de cifrado basado en el flujo de los datos
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         RijndaelAlg.CreateEncryptor(Key, IV),
                                                         CryptoStreamMode.Write);

            // Obtener la representación en bytes de la información a cifrar
            byte[] plainMessageBytes = UTF8Encoding.UTF8.GetBytes(plainMessage);

            // Cifrar los datos enviándolos al flujo de cifrado
            cryptoStream.Write(plainMessageBytes, 0, plainMessageBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Obtener los datos datos cifrados como un arreglo de bytes
            byte[] cipherMessageBytes = memoryStream.ToArray();

            // Cerrar los flujos utilizados
            memoryStream.Close();
            cryptoStream.Close();

            // Retornar la representación de texto de los datos cifrados
            return Convert.ToBase64String(cipherMessageBytes);
        }

        /**
         * Descifra una cadena texto con el algoritmo de Rijndael
         *
         * @param	encryptedMessage	mensaje cifrado
         * @param	Key			clave del cifrado para Rijndael
         * @param	IV			vector de inicio para Rijndael
         * @return	string		texto descifrado (plano)
         */
        public static string decryptStringRijndael(String encryptedMessage, String strKey, String strIv)
        {
            byte[] Key = UTF8Encoding.UTF8.GetBytes(strKey);
            byte[] IV = UTF8Encoding.UTF8.GetBytes(strIv);

            //Estableciendo longuitud válida del arreglo
            int keySize = 32;
            int ivSize = 16;
            
            Array.Resize(ref Key, keySize);
            Array.Resize(ref IV, ivSize);

            // Obtener la representación en bytes del texto cifrado
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedMessage);

            // Crear un arreglo de bytes para almacenar los datos descifrados
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Crear una instancia del algoritmo de Rijndael
            Rijndael RijndaelAlg = Rijndael.Create();

            // Crear un flujo en memoria con la representación de bytes de la información cifrada
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Crear un flujo de descifrado basado en el flujo de los datos
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         RijndaelAlg.CreateDecryptor(Key, IV),
                                                         CryptoStreamMode.Read);

            // Obtener los datos descifrados obteniéndolos del flujo de descifrado
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            // Cerrar los flujos utilizados
            memoryStream.Close();
            cryptoStream.Close();

            // Retornar la representación de texto de los datos descifrados
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}
