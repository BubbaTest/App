using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Gaia.Seguridad
{
    /// <summary>
    /// Descripción breve de SesionActiva
    /// </summary>
    public class SesionActiva : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetNoStore();
            context.Response.ContentType = "application/x-javascript";
            context.Response.Write("//");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}