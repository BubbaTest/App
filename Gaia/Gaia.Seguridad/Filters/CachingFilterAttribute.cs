using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using PJN.DAL;
using PJN.DAL.Model.GENERAL;

namespace Gaia.Seguridad.Filters
{
    public class CachingFilterAttribute : ActionFilterAttribute
    {
        PJNDbContext db = new PJNDbContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var role = ((PJN.DAL.Model.Utilisatrice)((HttpSessionStateBase)new HttpSessionStateWrapper(HttpContext.Current.Session))["PJN.DAL.Model.Utilisatrice"]).Rol;
            var Opciones = db.RolOpcion.Where(c => c.RolId == role); // db.UsuarioRol.Where(c=> c.UsuarioId == usuario).FirstOrDefault().RolOpciones;

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
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{

        //    var Opciones = ((DAL.Model.Usuario)((HttpSessionStateBase)new HttpSessionStateWrapper(HttpContext.Current.Session))["Gaia.DAL.Model.Usuario"]).UsuarioRolEntidad.FirstOrDefault().Rol.RolOpciones;

        //    string NombreControlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        //    string NombreAccion = filterContext.ActionDescriptor.ActionName;

        //    var Registros = Opciones.Where(o => o.Opcion.OpcionTipo.Contains("Accion") && o.Opcion.Mapeo != null && o.Opcion.Mapeo.Equals(NombreControlador + "/" + NombreAccion)).Count();
        //    bool Resultado = (Registros > 0 ? true : false);

        //    if (!Resultado)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Home" }, { "action", "UsuarioNoAutorizado" } });
        //    }

        //        base.OnActionExecuting(filterContext); 
        //}
    }

    public class AuditFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Usuario = ((DAL.Model.Usuario)((HttpSessionStateBase)new HttpSessionStateWrapper(HttpContext.Current.Session))["Gaia.DAL.Model.Usuario"]);
            string NombreControlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string NombreAccion = filterContext.ActionDescriptor.ActionName;
            string ValorParametro = (filterContext.ActionParameters.Count > 0 ? Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.ActionParameters).Replace("\"", "") : null);

            var request = filterContext.HttpContext.Request;

            DAL.Model.Auditoria auditoria = new DAL.Model.Auditoria();

            if (Usuario == null)
            {
                auditoria.SessionID = HttpContext.Current.Session.SessionID;
                auditoria.UsuarioId = "-1";
                auditoria.RolId = "-1";
                auditoria.EntidadId = "-1";
            }
            else
            {
                auditoria.SessionID = HttpContext.Current.Session.SessionID;
                auditoria.UsuarioId = Usuario.UsuarioId;
                auditoria.RolId = (Usuario.UsuarioRolEntidad.Count == 0 ? null : Usuario.UsuarioRolEntidad.FirstOrDefault().RolId);
                auditoria.EntidadId = (Usuario.UsuarioRolEntidad.Count == 0 ? null : Usuario.UsuarioRolEntidad.FirstOrDefault().EntidadId);
                
                //DAL.GaiaDbContext context = new DAL.GaiaDbContext();

                //context.Auditoria.Add(auditoria);
                //context.SaveChanges();
            }

            auditoria.Host = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            auditoria.PaginaAccedida = request.RawUrl;
            //auditoria.FechaRegistro = DateTime.Now;
            auditoria.NombreControlador = NombreControlador;
            auditoria.NombreAccion = NombreAccion;
            auditoria.Parametro = ValorParametro;

            using (var bd = new DAL.GaiaDbContext())
            {
                try
                {
                    using (var transaction = bd.Database.BeginTransaction())
                    {
                        try
                        {
                            //throw new Exception("error auditoria");
                            //bd.Auditoria.Add(auditoria);
                            //bd.SaveChanges();
                            List<SqlParameter> parametros = new List<SqlParameter>();

                            SqlParameter pSessionID = new SqlParameter("@SessionID", SqlDbType.VarChar, 50);
                            pSessionID.Value = auditoria.SessionID;
                            parametros.Add(pSessionID);

                            SqlParameter pUsuarioId = new SqlParameter("@UsuarioId", SqlDbType.VarChar, 30);
                            pUsuarioId.Value = auditoria.UsuarioId;
                            parametros.Add(pUsuarioId);

                            SqlParameter pRolId = new SqlParameter("@RolId", SqlDbType.VarChar, 30);
                            pRolId.Value = auditoria.RolId;
                            parametros.Add(pRolId);

                            SqlParameter pEntidadId = new SqlParameter("@EntidadId", SqlDbType.VarChar, 9);
                            pEntidadId.Value = auditoria.EntidadId;
                            parametros.Add(pEntidadId);

                            SqlParameter pHost = new SqlParameter("@Host", SqlDbType.VarChar, 30);
                            pHost.Value = auditoria.Host;
                            parametros.Add(pHost);

                            SqlParameter pPaginaAccedida = new SqlParameter("@PaginaAccedida", SqlDbType.VarChar, -1);
                            pPaginaAccedida.Value = auditoria.PaginaAccedida;
                            parametros.Add(pPaginaAccedida);

                            SqlParameter pNombreControlador = new SqlParameter("@NombreControlador", SqlDbType.VarChar, 255);
                            pNombreControlador.Value = auditoria.NombreControlador;
                            parametros.Add(pNombreControlador);

                            SqlParameter pNombreAccion = new SqlParameter("@NombreAccion", SqlDbType.VarChar, 255);
                            pNombreAccion.Value = auditoria.NombreAccion;
                            parametros.Add(pNombreAccion);

                            SqlParameter pParametro = new SqlParameter("@Parametro", SqlDbType.NVarChar, -1);
                            if(auditoria.Parametro == null)
                            { pParametro.Value = DBNull.Value; }
                            else
                            { pParametro.Value = auditoria.Parametro; }                            
                            parametros.Add(pParametro);

                            SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                            pRetorno.Direction = ParameterDirection.Output;
                            parametros.Add(pRetorno);

                            SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                            pMensaje.Direction = ParameterDirection.Output;
                            parametros.Add(pMensaje);

                            object[] oParametros = parametros.ToArray();

                            var r = bd.Database.ExecuteSqlCommand("EXECUTE seguridad.spAuditoriaInsertar @SessionID, @RolId, @Host, @PaginaAccedida, @NombreControlador, @NombreAccion, @Parametro, @UsuarioId, @EntidadId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros);

                            transaction.Commit();
                        }
                        catch (Exception) { transaction.Rollback(); }
                        finally { transaction.Dispose(); }
                    }
                }
                catch (Exception) { if (bd.Database.CurrentTransaction != null) { bd.Database.CurrentTransaction.Rollback(); } bd.Dispose(); }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}