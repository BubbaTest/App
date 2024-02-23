using System.Net;
//using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Gaia.Seguridad.Filters
{
    public class AuthenticateUser: FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var SesionActual = ((DAL.Model.Usuario)((HttpSessionStateBase)new HttpSessionStateWrapper(HttpContext.Current.Session))["Gaia.DAL.Model.Usuario"]);
            string NombreAccion = filterContext.ActionDescriptor.ActionName;
            string NombreControlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if ( NombreAccion != "Login" && NombreAccion != "ValidarUsuario" && NombreAccion != "ResetearPassword" && NombreAccion != "CambiarPassword" && NombreAccion != "ModificarPassword" && NombreAccion != "RecuperarPassword" && NombreAccion!= "ExisteUsuario" && NombreAccion!= "CredencialesCorrectas" && NombreAccion!= "_FormularioCredenciales" && NombreAccion != "Error" && (NombreControlador != "Home" && NombreAccion != "SesionFinalizada"))
            {
                if (SesionActual == null )
                {
                    //ViewResult result = new ViewResult();
                    //result.ViewName = "Login";
                    //filterContext.Result = result;
                    if (NombreControlador == "Home" && NombreAccion == "Index")
                    { filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Usuario" }, { "action", "Login" }}); }
                    else
                    { filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Home" }, { "action", "SesionFinalizada" }, { "Motivo", "Session" } }); }
                }
                else
                {
                    //if (NombreControlador != "Home" && NombreAccion != "SesionFinalizada")
                    //{
                        Gaia.BLL.Repository.GenericRepository<Gaia.DAL.Model.UsuarioSistema> _US = new BLL.Repository.GenericRepository<DAL.Model.UsuarioSistema>(new Gaia.DAL.GaiaDbContext("cnnGaia"));
                        var ID = SesionActual.UsuarioSistema.FirstOrDefault().Id;
                        var usuarioservicio = _US.SearchFor(us => us.Id == ID);
                        string TokenSesionActual = SesionActual.UsuarioSistema.FirstOrDefault().Token;
                        string TokenBD = usuarioservicio.FirstOrDefault().Token;
                        //Gaia.Helpers.Encriptar.ValidateHashData(u.Password, _UsuarioSistema.FirstOrDefault().Password, key, algoritmo)
                        //if (usuarioservicio != null)
                        //{
                        if (!string.Equals(TokenSesionActual, TokenBD))
                        {
                            HttpContext.Current.Session.Abandon();
                            System.Web.Security.FormsAuthentication.SignOut();
                            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Home" }, { "action", "SesionFinalizada" }, { "Motivo", "Token" }});
                        }
                    //}
                    //}
                }
            }
            
        }
    }
}