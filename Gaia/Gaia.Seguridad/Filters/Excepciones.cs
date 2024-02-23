using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Handlers;
using Elmah;
using System.Net.Http;

namespace Gaia.Seguridad.Filters
{
    public class Excepciones : System.Web.Mvc.HandleErrorAttribute, System.Web.Mvc.IExceptionFilter, System.Web.Mvc.IMvcFilter
    {

        private ContentResult JsonError(int retorno = -99, string mensaje = "Algo se ha ido al traste, pero lo resolveremos!.")
        {
            object objF = new { Retorno = retorno, Mensaje = mensaje };
            string j = Newtonsoft.Json.JsonConvert.SerializeObject(objF);
            // string j = "{ \"Retorno\": \"" + retorno.ToString() + "\",\"Mensaje\": \" " + mensaje + "\"}";
            return new ContentResult { Content = j, ContentEncoding = System.Text.Encoding.UTF8, ContentType = "application/json" };
        }

        public override void OnException(ExceptionContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            //base.OnException(filterContext);
            ErrorLog.GetDefault(HttpContext.Current).Log(new Error(filterContext.Exception));

            if (filterContext.ExceptionHandled) { return; } //Las excepciones que están controladas, ya no se registran en elmah.axd

            //response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            //response.StatusDescription = "Ocurrió algo!. Déjanos resolverlo en un momento!.";
            //response.SubStatusCode = (int)System.Net.HttpStatusCode.Unauthorized;

            //Tratamiento especial
            if (filterContext.Exception is HttpAntiForgeryException)
            {
                //(StringContent("<p>Charlie esta volando la cometa!</p>"));
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "action", "Login" },
                        { "controller", "Usuario" }
                    });

                response.StatusCode = (int)888;
                response.StatusDescription = "Su sesión ha finalizado, por favor ingrese nuevamente";
                response.Cookies.Clear();
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Session.Abandon();
                filterContext.HttpContext.ClearError();
                filterContext.ExceptionHandled = true;
                return;
            }

            filterContext.Result = new RedirectToRouteResult(
                            new System.Web.Routing.RouteValueDictionary
                            {
                                                        { "action", "Error" },
                                                        { "controller", "Usuario" }
                            });

            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }
    }
}