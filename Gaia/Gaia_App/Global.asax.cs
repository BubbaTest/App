using Forloop.HtmlHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Gaia.Seguridad.Filters;
using Gaia.Seguridad.Controllers;

namespace Gaia_App
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Filtro de excepciones para Elmah
            GlobalFilters.Filters.Add(new Excepciones());
            //Agregamos verificación de la sesión
            GlobalFilters.Filters.Add(new AuthenticateUser());
            //Agregar para traza de auditoria
            GlobalFilters.Filters.Add(new AuditFilterAttribute());
            //Eliminar Versión de MVC
            MvcHandler.DisableMvcResponseHeader = true;
            ScriptContext.ScriptPathResolver = System.Web.Optimization.Scripts.Render;

            Application["AppTitle"] = System.Configuration.ConfigurationManager.AppSettings["AppTitle"].ToString();

            //HttpConfiguration config = GlobalConfiguration.Configuration;

            //config.Formatters.JsonFormatter.SerializerSettings
              //          .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All;

            //AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //Eliminar cabeceras de versiones
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
        }
    }
}
