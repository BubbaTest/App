using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gaia.Seguridad.Model
{
    public class jsonSMSResponse
    {
        /// <summary>
        /// Codigo unico del mensaje
        /// </summary>
        public int message_id { get; set; }

        /// <summary>
        /// Numero de estado de envio  0 - success ,  1 – Unkown error, 2 – Syntax error, mandatory parameter missing, 10 – Access denied, 11 – Invalid message, 16 – No message credits left
        /// </summary>
        public int status_code { get; set; }

        /// <summary>
        /// Descripcion del mensaje
        /// </summary>
        public string message_description { get; set; }
    }
}