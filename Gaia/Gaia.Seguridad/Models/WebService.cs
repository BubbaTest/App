using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using System.Web;
using static System.Collections.Specialized.BitVector32;
using PJN.DAL;

namespace Gaia.Seguridad.Model
{
    public class WebService
    {
        /// <summary>
        /// Direccion del api para envio de correo.
        /// </summary>
        private static string apiMailUrl { get { return ConfigurationManager.AppSettings["Api_Mail"]; } }

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static dynamic WebApiGet(string hostHeader, string AppSetting, string toke, out int Retorno, out string Mensaje)
        {
            return ApiGet(hostHeader, ConfigurationManager.AppSettings[AppSetting], toke, out Retorno, out Mensaje);
        }

        public static dynamic ApiGet(string hostHeader, string BaseUrl, string toke, out int Retorno, out string Mensaje)
        {
            dynamic responseString = null;

            var client = new HttpClient();
            {
                client.BaseAddress = new Uri(BaseUrl);
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", toke);
                client.Timeout = TimeSpan.FromMinutes(5);

                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                var response = client.GetAsync(hostHeader).Result;
                if (response.IsSuccessStatusCode)
                {
                    responseString = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    Retorno = (int)response.StatusCode;
                    Mensaje = "Ocurrio un error";
                    return null;
                }
            }

            Retorno = 0;
            Mensaje = "Se ejecuto con exito";
            return responseString;
        }
        

        public static dynamic WebApiGetByte(string hostHeader, string AppSetting, out int Retorno, out string Mensaje)
        {
            dynamic responseString = null;

            var client = new HttpClient();
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[AppSetting]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(10);

                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                
                var response = client.GetAsync(hostHeader).Result;
                if (response.IsSuccessStatusCode)
                {
                    responseString = response.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    Retorno = (int)response.StatusCode;
                    Mensaje = "Ocurrio un error";
                    return null;
                }
            }

            Retorno = 0;
            Mensaje = "Se ejecuto con exito";
            return responseString;
        }

        public class attachDocuments
        {
            private string private_bytearray;
            public string _bytearray
            {
                get { return private_bytearray; }
                set { private_bytearray = value; }
            }
            private string private_fileName;
            public string _fileName
            {
                get { return private_fileName; }
                set { private_fileName = value; }
            }
            public attachDocuments(string bytearray, string fileName)
            {
                this.private_bytearray = bytearray;
                this._fileName = fileName;
            }
        }

        /// <summary>
        /// Envia correo al destinatario.
        /// </summary>
        /// <param name="mail">Dirección de correo.</param>
        /// <param name="message">Contenido del correo.</param>
        /// <returns></returns>
        public static jsonSMSResponse SendMAIL(string mail, string asunto, string message, List<attachDocuments> AttachDocuments)
        {
            int Retorno = -99;
            string Mensaje = string.Empty;

            Dictionary<string, object> jsonKey = new Dictionary<string, object>();
            List<string> listPara = new List<string>();
            listPara.Add(mail);
            List<string> listCopia = new List<string>();
            List<string> listOculta = new List<string>();
            jsonKey.Add("Para", listPara);
            jsonKey.Add("Copia", listCopia);
            jsonKey.Add("Oculta", listOculta);
            jsonKey.Add("Asunto", asunto.Trim());
            if (AttachDocuments != null)
            {
                jsonKey.Add("AttachDocuments", AttachDocuments);
            }

            //string etiqueta = "<p>"+ message + "</p>";
            //message = HttpUtility.HtmlEncode(message);
            string hash64 = System.Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(message.Trim()));
            jsonKey.Add("Contenido", hash64);
            string eljson = JsonConvert.SerializeObject(jsonKey);

            var result = WebService.WebApipost("Correo/Enviar", apiMailUrl, JsonConvert.SerializeObject(jsonKey), "", out Retorno, out Mensaje);

            if (result == null)
            {
                result = new jsonSMSResponse() { message_id = -1, status_code = Retorno, message_description = Mensaje };
                return result;
            }

            result = new jsonSMSResponse() { message_id = -1, status_code = Retorno, message_description = Mensaje };

            //var jsonResult = JsonConvert.DeserializeObject<jsonSMSResponse>(result);

            return result;
        }

        /// <summary>
        /// Realiza una llamada de tipo POST para una dirección de api.
        /// </summary>
        /// <param name="hostHeader"></param>
        /// <param name="url"></param>
        /// <param name="objectPost"></param>
        /// <param name="Retorno"></param>
        /// <param name="Mensaje"></param>
        /// <returns></returns>
        public static dynamic WebApipost(string hostHeader, string url, string objectPost, string toke, out int Retorno, out string Mensaje)
        {
            return WebApipost(hostHeader, url, objectPost, "application/json", toke, out Retorno, out Mensaje);
        }

        public static dynamic WebApipost(string hostHeader, string url, string objectPost, string typeContent, string toke, out int Retorno, out string Mensaje)
        {
            dynamic responseString = null;

            var client = new HttpClient();
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", toke);
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                var response = client.PostAsync(hostHeader, new StringContent(objectPost, System.Text.Encoding.UTF8, typeContent)).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseString = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    Retorno = (int)response.StatusCode;
                    Mensaje = "Ocurrio un error";
                    if (Retorno != 401)
                        return response.Content.ReadAsStringAsync().Result;
                    else
                        return null;
                }
            }

            Retorno = 0;
            Mensaje = "Se ejecuto con exito";
            return responseString;
        }

        /// <summary>
        /// Realiza una llamada de tipo POST para una dirección de api.
        /// </summary>
        /// <param name="hostHeader"></param>
        /// <param name="url"></param>
        /// <param name="objectPost"></param>
        /// <param name="Retorno"></param>
        /// <param name="Mensaje"></param>
        /// <returns></returns>
        public static dynamic WebApipostByte(string hostHeader, string url, string objectPost, out int Retorno, out string Mensaje)
        {
            dynamic responseString = null;

            var client = new HttpClient();
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                var response = client.PostAsync(hostHeader, new StringContent(objectPost, System.Text.Encoding.UTF8, "application/json")).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseString = response.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    Retorno = (int)response.StatusCode;
                    Mensaje = "Ocurrio un error";
                    return null;
                }
            }

            Retorno = 0;
            Mensaje = "Se ejecuto con exito";
            return responseString;
        }

        /// <summary>
        /// Realiza una llamada de tipo POST para una dirección de api.
        /// </summary>
        /// <param name="hostHeader"></param>
        /// <param name="url"></param>
        /// <param name="objectPost"></param>
        /// <param name="Retorno"></param>
        /// <param name="Mensaje"></param>
        /// <returns></returns>
        public static dynamic WebApipostReport(string hostHeader, string url, List<KeyValuePair<string, string>> objectPost, out int Retorno, out string Mensaje)
        {
            dynamic responseString = null;

            var client = new HttpClient();
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(5);
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                var response = client.PostAsync(hostHeader, new FormUrlEncodedContent(objectPost)).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseString = response.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    Retorno = (int)response.StatusCode;
                    Mensaje = "Ocurrio un error";
                    return null;
                }
            }

            Retorno = 0;
            Mensaje = "Se ejecuto con exito";
            return responseString;
        }
    }
}