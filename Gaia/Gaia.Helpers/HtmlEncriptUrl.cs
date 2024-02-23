using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Helpers
{
    public static class HtmlEncriptUrl
    {
        /// <summary>
        /// Encripta una cadena html.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string EncodeUrl(string baseurl, string urlpath, string type)
        {
            try
            {
                string e = tripleDES_Encrypt(baseurl + "_" + urlpath, true);
                
                if(type == "get") 
                    e = string.Concat("Router/Ir/", Base64UrlEncode(e));
                else if  (type == "paginado")
                    e = string.Concat("Router/IrPaginado/", Base64UrlEncode(e));
                else
                    e = string.Concat("Router/IrPost/", Base64UrlEncode(e));

                return e;
            }
            catch { return ""; }
        }
        
        public static string DecodeUrl(string cadenaEncript)
        {
            return tripleDES_Decrypt(Base64UrlDecode(cadenaEncript), true);
        }

        #region Algoritmo_rijndael

        /// <summary>
        /// Función de encriptamiento. POSTERIORMENTE IMPLEMENTAR EN GAIA.
        /// </summary>
        /// <param name="cadena">string. texto que se encripta.</param>
        /// <param name="clave">byte[]</param>
        /// <param name="vInicio">byte[]</param>
        /// <returns>Cadena encriptada. POSTERIORMENTE IMPLEMENTAR EN GAIA.</returns>
        public static string RijndaelEncriptar(string cadena, byte[] clave, byte[] vInicio)
        {
            //Inicialmente se redimensionan la clave y el vector de inicio para confirmar su longitud
            //para este tipo de encriptamiento.. clave = 32bit y vInicio = 16bit
            Array.Resize(ref clave, 32);
            Array.Resize(ref vInicio, 16);
            Rijndael r = Rijndael.Create();
            // Establecer un flujo en memoria para el cifrado
            MemoryStream memoryStream = new MemoryStream();

            // Crear un flujo de cifrado basado en el flujo de los datos
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         r.CreateEncryptor(clave, vInicio),
                                                         CryptoStreamMode.Write);

            // Obtener la representación en bytes de la información a cifrar
            byte[] plainMessageBytes = UTF8Encoding.UTF8.GetBytes(cadena);

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

        /// <summary>
        /// Función de encriptamiento inverso. POSTERIORMENTE IMPLEMENTAR EN GAIA.
        /// </summary>
        /// <param name="cadena">string. texto que se encripta.</param>
        /// <param name="clave">byte[]</param>
        /// <param name="vInicio">byte[]</param>
        /// <returns>Cadena encriptada. POSTERIORMENTE IMPLEMENTAR EN GAIA.</returns>
        public static string RijndaelDesEncriptar(string cadena, byte[] clave, byte[] vInicio)
        {
            //Inicialmente se redimensionan la clave y el vector de inicio para confirmar su longitud
            //para este tipo de encriptamiento.. clave = 32bit y vInicio = 16bit
            Array.Resize(ref clave, 32);
            Array.Resize(ref vInicio, 16);
            // Obtener la representación en bytes del texto cifrado
            byte[] cipherTextBytes = Convert.FromBase64String(cadena);
            // Crear un arreglo de bytes para almacenar los datos descifrados
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Crear una instancia del algoritmo de Rijndael
            Rijndael RijndaelAlg = Rijndael.Create();

            // Crear un flujo en memoria con la representación de bytes de la información cifrada
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Crear un flujo de descifrado basado en el flujo de los datos
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         RijndaelAlg.CreateDecryptor(clave, vInicio),
                                                         CryptoStreamMode.Read);

            // Obtener los datos descifrados obteniéndolos del flujo de descifrado
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            // Cerrar los flujos utilizados
            memoryStream.Close();
            cryptoStream.Close();

            // Retornar la representación de texto de los datos descifrados
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        #endregion

        #region Algoritmo tripleDES

        /// <summary>
        /// Encripta una cadena.
        /// </summary>
        /// <param name="toEncrypt">Cadena a encriptar</param>
        /// <param name="useHashing">(bool) Si se especifica, se añade un hash de llave a la cadena encriptada.</param>
        /// <returns></returns>
        public static string tripleDES_Encrypt(string toEncrypt, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                System.Configuration.AppSettingsReader settingsReader =
                                                    new System.Configuration.AppSettingsReader();
                // Get the key from config file            
                string key = "";
                //key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
                key = "HtmlHelpers";
                //System.Windows.Forms.MessageBox.Show(key);
                //If hashing use get hashcode regards to your key
                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //Always release the resources and flush data
                    // of the Cryptographic service provide. Best Practice

                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)

                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                //transform the specified region of bytes array to resultArray
                byte[] resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor
                tdes.Clear();
                //Return the encrypted data into unreadable string format
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);

            }
            catch
            { return ""; }
        }

        /// <summary>
        /// Desencripta una cadena previamente encriptada por el mismo método.
        /// </summary>
        /// <param name="cipherString">Cadena encriptada</param>
        /// <param name="useHashing">(bool) Si se especificó en la encriptación de cadena previa, añade un hash de llave.</param>
        /// <returns></returns>
        public static string tripleDES_Decrypt(string cipherString, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                System.Configuration.AppSettingsReader settingsReader =
                                                    new System.Configuration.AppSettingsReader();
                //Get your key from config file to open the lock!
                string key = "";
                //key = (string)settingsReader.GetValue("SecurityKey", typeof(String));            
                key = "HtmlHelpers";

                if (useHashing)
                {
                    //if hashing was used get the hash code with regards to your key
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();
                }
                else
                {
                    //if hashing was not implemented get the byte code of the key
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex) { return ""; }
        }

        #endregion

        private static string Base64UrlEncode(string input)
        {
            // Special "url-safe" base64 encode.
            return input
              .Replace('+', '-') // replace URL unsafe characters with safe ones
              .Replace('/', '_') // replace URL unsafe characters with safe ones
              .Replace("=", "$"); // no padding
        }

        private static string Base64UrlDecode(string input)
        {
            // Special "url-safe" base64 encode.
            return input
              .Replace('-', '+') // replace URL unsafe characters with safe ones
              .Replace('_', '/')// replace URL unsafe characters with safe ones
              .Replace("$", "="); 
        }
    }
}
