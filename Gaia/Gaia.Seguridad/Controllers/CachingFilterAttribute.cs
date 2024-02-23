using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Gaia.Seguridad.Controllers
{
    public class CachingFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext
    filterContext)
        {

            var Opciones = ((DAL.Model.Usuario)((HttpSessionStateBase)new HttpSessionStateWrapper(HttpContext.Current.Session))["Gaia.DAL.Model.Usuario"]).UsuarioRolEntidad.FirstOrDefault().Rol.RolOpciones;

            string NombreControlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string NombreAccion = filterContext.ActionDescriptor.ActionName;

            var Registros = Opciones.Where(o => o.Opcion.OpcionTipo.Contains("Accion") && o.Opcion.Mapeo != null && o.Opcion.Mapeo.Equals(NombreControlador + "/" + NombreAccion)).Count();
            bool Resultado = (Registros > 0 ? true : false);

            if (!Resultado)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Home" }, { "action", "UsuarioNoAutorizado" } });
            }

                base.OnActionExecuting(filterContext); 
        }
    }

    public class AuditFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Usuario = ((DAL.Model.Usuario)((HttpSessionStateBase)new HttpSessionStateWrapper(HttpContext.Current.Session))["Gaia.DAL.Model.Usuario"]);
            string NombreControlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string NombreAccion = filterContext.ActionDescriptor.ActionName;
            string ValorParametro = (filterContext.ActionParameters.Count > 0 ? Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.ActionParameters).Replace("\"", "'") : null);
            var request = filterContext.HttpContext.Request;
            DAL.Model.Auditoria auditoria = new DAL.Model.Auditoria();

            if (Usuario == null)
            { auditoria.UsuarioId = "-1"; }
            else
            {
                auditoria.SessionID = HttpContext.Current.Session.SessionID;
                auditoria.UsuarioId = Usuario.UsuarioId;
                auditoria.RolId = (Usuario.UsuarioRolEntidad.Count == 0 ? null : Usuario.UsuarioRolEntidad.FirstOrDefault().RolId);
                auditoria.EntidadId = (Usuario.UsuarioRolEntidad.Count == 0 ? null : Usuario.UsuarioRolEntidad.FirstOrDefault().EntidadId);
                auditoria.Host = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
                auditoria.PaginaAccedida = request.RawUrl;
                auditoria.FechaRegistro = DateTime.Now;
                auditoria.NombreControlador = NombreControlador;
                auditoria.NombreAccion = NombreAccion;
                auditoria.Parametro = ValorParametro;

                DAL.GaiaDbContext context = new DAL.GaiaDbContext();
                context.Auditoria.Add(auditoria);
                context.SaveChanges();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}