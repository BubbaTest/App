using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Gaia.DAL;
using Gaia.DAL.Model;
using Gaia.BLL.Repository;

namespace Gaia.Seguridad.Controllers
{
    public class RolController : Controller
    {
        GaiaDbContext db = new GaiaDbContext();
        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
        
        Usuario UsuarioActual;
        String Proyecto;
        // GET: /Rol/

        //Prueba
        public ActionResult Index()
        {
            return View(db.Roles);
        }

        public ActionResult RolPorUsuario(string id)
        {
            ViewBag.Titulo = (System.Configuration.ConfigurationManager.AppSettings["tituloApp"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["tituloApp"].ToString());
            Proyecto = (System.Configuration.ConfigurationManager.AppSettings["Proyecto"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["Proyecto"].ToString());

            //Ingresa a la opción de seleccionar un rol, entonces, asignamos el valor de la variable de sesión "Rol=*", caso contrario "Rol=1"
            Session["Rol"] = "*";
            Usuario UsuarioActual = db.Usuarios.Find(id);

            ViewBag.Usuario = UsuarioActual.UsuarioId;
            ViewBag.Correo = UsuarioActual.Correo;

            return View(UsuarioActual.UsuarioRolEntidad.Where(r => r.RolId.ToUpper().Contains(Proyecto.ToUpper()) && r.Usuario.EstadoId.Equals("A") && r.Rol.Activo.Value.Equals(true) && r.EntidadG.Activo.Value.Equals(true)));
        }

        public ActionResult RedirigirARolPorUsuario()
        {
            UsuarioActual = SessionHelper.GetItem<Usuario>(session);

            return RedirectToAction("RolPorUsuario","Rol", new { id= UsuarioActual.UsuarioId });
        }

        public ActionResult Redirigir(string rolId, string entidadId)
        {
            UsuarioActual = SessionHelper.GetItem<Usuario>(session);

            if (UsuarioActual == null)
                return RedirectToAction("Login", "Usuario");

            //Del listado de UsuarioRolEntidad 
            List<UsuarioRolEntidad> ureTemp = SessionHelper.GetItem<List<UsuarioRolEntidad>>(session);

            UsuarioActual.UsuarioRolEntidad = ureTemp.Where(re => re.RolId.Equals(rolId) && re.EntidadId.Equals(entidadId)).ToList();

            SessionHelper.AddItem(session, UsuarioActual);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult RedirigirAInicio()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
