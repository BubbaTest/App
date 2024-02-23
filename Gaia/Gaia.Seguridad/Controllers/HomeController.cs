using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
//using System.Data.Entity;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Gaia.Seguridad.Controllers;

using Gaia.DAL;
using Gaia.BLL.Repository;
using Gaia.DAL.Model;

using PJN.DAL;
using PJN.DAL.Model;
using PJN.DAL.Model.GENERAL;
using Nicarao.App.Permisos;
using System.Data.SqlClient;
using System.Reflection.Emit;

namespace Gaia.Seguridad.Controllers
{
    [Authorize]
    [ValidarSesion]
    public class HomeController : Controller
    {
        //int Retorno = -1;
        //bool resultado = true;
        //string Mensaje = "Ocurrio un error no controlado";
        //GaiaDbContext db = new GaiaDbContext();
       
        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);

        List<LeCatOpcion> Opciones = new List<LeCatOpcion>();
        List<LeCatOpcion> OpcionesRol = new List<LeCatOpcion>();
        //List<catOpcion> OpcionesUsuario = new List<catOpcion>();
        //List<catOpcion> OpcionesRolUsuario = new List<catOpcion>();
        List<LeCatOpcion> OpcionesMenu = new List<LeCatOpcion>();

        //List<PJN.DAL.Model.GENERAL.catOpcion> mOpciones = new List<PJN.DAL.Model.GENERAL.catOpcion>();
        //List<PJN.DAL.Model.GENERAL.catOpcion> rolOpciones = new List<PJN.DAL.Model.GENERAL.catOpcion>();
        //List<PJN.DAL.Model.GENERAL.catOpcion> MenuOpciones = new List<PJN.DAL.Model.GENERAL.catOpcion>();
        //List<PJN.DAL.Model.GENERAL.RolOpcion> listar = new List<PJN.DAL.Model.GENERAL.RolOpcion>();
        //GenericRepository<PJN.DAL.Model.GENERAL.catOpcion> _OPC = new GenericRepository<PJN.DAL.Model.GENERAL.catOpcion>(new PJNDbContext());
        //GenericRepository<PJN.DAL.Model.GENERAL.RolOpcion> _ROLOPCION = new GenericRepository<PJN.DAL.Model.GENERAL.RolOpcion>(new PJNDbContext());

        PJN.DAL.Model.Utilisatrice sessionUsuarioActual;

        //String TituloApp;
        //String Proyecto;
        String Ambiente;
        //string ActionDefault;
                
        public ActionResult Index()
        {
            sessionUsuarioActual = SessionHelper.GetItem<PJN.DAL.Model.Utilisatrice>(Session);

            if (sessionUsuarioActual == null)
                return RedirectToAction("Login", "Usuario");


            //JObject JObjDatosUsuario = JObject.Parse(JsonConvert.SerializeObject(session["usuario"]));
            //var sUsuario = JObjDatosUsuario["UsuarioId"].Value<string>();
            
            Ambiente = (System.Configuration.ConfigurationManager.AppSettings["Ambiente"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["Ambiente"].ToString()).ToUpper();
            ViewBag.Titulo = (System.Configuration.ConfigurationManager.AppSettings["tituloApp"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["tituloApp"].ToString());
            ViewBag.Descripcion = (System.Configuration.ConfigurationManager.AppSettings["descripcionApp"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["descripcionApp"].ToString());
                       
            ViewBag.Proyecto = (System.Configuration.ConfigurationManager.AppSettings["Proyecto"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["Proyecto"].ToString());
            ViewBag.Ambiente = (Ambiente == "PRUEBA" ? "PRUEBA" : (Ambiente == "FORMACION" ? "FORMACION" : "PERFIL"));
            ViewBag.sUsuario = sessionUsuarioActual.UsuarioId; //   JObjDatosUsuario["UsuarioId"].Value<string>();
            ViewBag.sRol = sessionUsuarioActual.Rol;  //JObjDatosUsuario["Rol"].Value<string>();
            ViewBag.sRolDescripcion = sessionUsuarioActual.RolDescripcion; //JObjDatosUsuario["RolDescripcion"].Value<string>();
            ViewBag.sCorreo = sessionUsuarioActual.Correo;  //JObjDatosUsuario["Correo"].Value<string>();
            ViewBag.NombreCompleto = sessionUsuarioActual.Nombres + " " + sessionUsuarioActual.Apellidos; //JObjDatosUsuario["Nombres"].Value<string>() + " " + JObjDatosUsuario["Apellidos"].Value<string>();
            ViewBag.UsuarioMaquina = Environment.UserName;
            ViewBag.Ip = Request.UserHostAddress;

            ViewBag.ActionDefault = "";

            //Menu(JObjDatosUsuario["Rol"].Value<string>(), JObjDatosUsuario["UsuarioId"].Value<string>(), JObjDatosUsuario["Password"].Value<string>());
            //Menu(sessionUsuarioActual.Rol, sessionUsuarioActual.UsuarioId, sessionUsuarioActual.Password);
            Menu();
            //if (mOpciones.Count > 0)
            //{ 
            //    ViewBag.Opciones = mOpciones; }
            //else
            //{
            //    SessionHelper.CerrarSesion();
            //    return RedirectToAction("Login", "Usuario");
            //}
            if (Opciones.Count > 0)
            { ViewBag.Opciones = Opciones; }
            else
            {
                SessionHelper.CerrarSesion();
                return RedirectToAction("Login", "Usuario");
            }

            return View("Widget");
        }

        public ActionResult Widget()
        {
            return View();
        }

        public string UsuarioOpciones(string UsuarioId, string Password, out string JsonRetorno)
        {
            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(new PJNDbContext());
                List<SqlParameter> Parametros = new List<SqlParameter>();
                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.VarChar, Size = 15 });
                Parametros.Add(new SqlParameter { ParameterName = "@Password", Value = Password, SqlDbType = SqlDbType.VarChar, Size = 70 });
                SqlParameter[] oParametros = Parametros.ToArray();
                JsonRetorno = cnn.ExecStoreProcedure("sde.spUsuarioValidar @UsuarioId, @Password", oParametros).FirstOrDefault();

                return JsonRetorno;
            }
            catch (Exception)
            {
                JsonRetorno = null;
                return JsonRetorno;
            }
        }

        public void Menu()
        {;
            OpcionesRol = (from ro in sessionUsuarioActual.UsuarioRol.FirstOrDefault().Rol.RolOpciones.AsQueryable()
                           select ro.Opcion)
                           .ToList();

            OpcionesMenu.AddRange(OpcionesRol
                                  .Where(r => r.OpcionTipo.Contains("Menu"))
                                  .OrderBy(r => r.Nivel));

            foreach (var om in OpcionesMenu)
            {
                if (om.Nivel == 2)
                {
                    LeCatOpcion item = new LeCatOpcion
                    {
                        OpcionId = om.OpcionId,
                        Nivel = om.Nivel,
                        Padre = om.Padre,
                        Opcion = om.Opcion,
                        OpcionTipo = om.OpcionTipo,
                        Mapeo = om.Mapeo,                      
                        DescripcionGeneral = om.DescripcionGeneral,
                        Activo = om.Activo
                    };
                    Opciones.Add(crearArbolMenu(item, OpcionesMenu));
                }
            }
        }

        //Carga del menú, retorna un listado de opciones según el rol seleccionado
        //public void Menu2(string rol, string usuario, string pass)
        //{
            //string JSONDatos = string.Empty;
            //UsuarioOpciones(usuario, pass, out JSONDatos);

            //JObject JObjDatosUsuario = JObject.Parse(JSONDatos);
            //if (JObjDatosUsuario["Retorno"].Value<int>() == 0 && JObjDatosUsuario["URL"].Value<string>().Contains("Home/Index"))
            //{
            //    var lstRolOpcion = JObjDatosUsuario["JsonDatosUsuario"]["JsonRol"][0]["RolOpciones"].ToList();
            //    //var lstRolOpcion = JObjDatosUsuario["JsonDatosUsuario"]["JsonRol"].ToList();
            //    foreach (var ro in lstRolOpcion)
            //    {
            //        var resultadox = ro["RolId"].Value<string>();
            //        listar.Add(new PJN.DAL.Model.LATRINIDAD.RolOpcion
            //        {
            //            RolId = ro["RolId"].Value<string>(),
            //            OpcionId = (new System.Data.Entity.Hierarchy.HierarchyId(ro["OpcionId"].Value<string>())),
            //            Default = (ro["Default"].ToString() != "" ? ro["Default"].Value<bool>() : false),
            //            Rol = null,
            //            Opcion = new PJN.DAL.Model.LATRINIDAD.catOpcion
            //            {
            //                OpcionId = (new System.Data.Entity.Hierarchy.HierarchyId(ro["OpcionId"].Value<string>())),
            //                Nivel = ro["Opcion"]["Nivel"].Value<Int16>(),
            //                Padre = (ro["Opcion"]["Padre"].ToString() != "{}" ? (new System.Data.Entity.Hierarchy.HierarchyId(ro["Opcion"]["Padre"].Value<string>())) : null),
            //                Opcion = ro["Opcion"]["Opcion"].Value<string>(),
            //                OpcionTipo = ro["Opcion"]["OpcionTipo"].Value<string>(),
            //                Mapeo = (ro["Opcion"]["Mapeo"].ToString() != "" ? ro["Opcion"]["Mapeo"].Value<string>() : null),
            //                DescripcionGeneral = (ro["Opcion"]["DescripcionGeneral"].ToString() != "" ? ro["Opcion"]["DescripcionGeneral"].Value<string>() : null),
            //                Activo = (ro["Opcion"]["Activo"].ToString() != "" ? ro["Opcion"]["Activo"].Value<bool>() : true),
            //                OpcionesHijo = new List<PJN.DAL.Model.LATRINIDAD.catOpcion>(),
            //                RolOpciones = new List<PJN.DAL.Model.LATRINIDAD.RolOpcion>(),
            //                //Usuarios = null
            //            }
            //        });
            //    }
            //}

            //rolOpciones = (from ro in listar.FirstOrDefault().Opcion.RolOpciones.AsQueryable()
            //               select ro.Opcion)
            //              .ToList();

            //MenuOpciones.AddRange(rolOpciones
            //                      .Where(r => r.OpcionTipo.Contains("Menu"))
            //                      .OrderBy(r => r.Nivel));

            //foreach (var om in MenuOpciones)
            //{
            //    if (om.Nivel == 2)
            //    {
            //        PJN.DAL.Model.LATRINIDAD.catOpcion item = new PJN.DAL.Model.LATRINIDAD.catOpcion
            //        {
            //            OpcionId = om.OpcionId,
            //            Nivel = om.Nivel,
            //            Padre = om.Padre,
            //            Opcion = om.Opcion,
            //            OpcionTipo = om.OpcionTipo,
            //            Mapeo = om.Mapeo,
            //            DescripcionGeneral = om.DescripcionGeneral,
            //            Activo = om.Activo
            //        };
            //        mOpciones.Add(crearArbolMenu(item, MenuOpciones, rol));
            //    }
            //}

            //aqui
            //rolOpciones = (from so in _ROLOPCION.SelectAll().Where(c => c.RolId == rol).ToList()
            //               select so.Opcion).ToList();

            //MenuOpciones.AddRange(rolOpciones
            //                      .Where(r => r.OpcionTipo.Contains("Menu"))
            //                      .OrderBy(r => r.Nivel));

            //foreach (var om in MenuOpciones)
            //{
            //    if (om.Nivel == 2)
            //    {
            //        PJN.DAL.Model.GENERAL.catOpcion item = new PJN.DAL.Model.GENERAL.catOpcion
            //        {
            //            OpcionId = om.OpcionId,
            //            Nivel = om.Nivel,
            //            Padre = om.Padre,
            //            Opcion = om.Opcion,
            //            OpcionTipo = om.OpcionTipo,
            //            Mapeo = om.Mapeo,
            //            DescripcionGeneral = om.DescripcionGeneral,
            //            Activo = om.Activo
            //        };
            //        mOpciones.Add(crearArbolMenu2(item, MenuOpciones, rol));
            //    }
            //}
        //}

        //protected PJN.DAL.Model.GENERAL.catOpcion crearArbolMenu2(PJN.DAL.Model.GENERAL.catOpcion opcion, ICollection<PJN.DAL.Model.GENERAL.catOpcion> opcionrol, string Obj)
        //{
        //    //foreach (var o in opcionrol.Where(o => o.Padre == opcion.OpcionId && o.OpcionTipo.ToUpper().Contains("MENU")))
        //    //{
        //    //    PJN.DAL.Model.LATRINIDAD.catOpcion sitem = new PJN.DAL.Model.LATRINIDAD.catOpcion
        //    //    {
        //    //        OpcionId = o.OpcionId,
        //    //        Nivel = o.Nivel,
        //    //        Padre = o.Padre,
        //    //        Opcion = o.Opcion,
        //    //        OpcionTipo = o.OpcionTipo,
        //    //        Mapeo = o.Mapeo,
        //    //        DescripcionGeneral = o.DescripcionGeneral,
        //    //        Activo = o.Activo
        //    //    };

        //    //    opcion.OpcionesHijo.Add(sitem);

        //    //    if (opcionrol.Where(so => so.Padre == o.OpcionId).Any())
        //    //    {
        //    //        crearArbolMenu(sitem, opcionrol, Obj);
        //    //    }
        //    //}

        //    //return opcion;
        //    var nOpciones = (from so in _ROLOPCION.SelectAll().Where(c => c.RolId == Obj).ToList()
        //                     join op in _OPC.SelectAll().Where(o => o.Padre == opcion.OpcionId && o.OpcionTipo.ToUpper().Contains("ACCION")) on so.OpcionId equals op.OpcionId
        //                     select op).ToList();
        //    foreach (var o in nOpciones)
        //    {
        //        PJN.DAL.Model.GENERAL.catOpcion sitem = new PJN.DAL.Model.GENERAL.catOpcion
        //        {
        //            OpcionId = o.OpcionId,
        //            Nivel = o.Nivel,
        //            Padre = o.Padre,
        //            Opcion = o.Opcion,
        //            OpcionTipo = o.OpcionTipo,
        //            Mapeo = o.Mapeo,
        //            DescripcionGeneral = o.DescripcionGeneral,
        //            Activo = o.Activo
        //        };

        //        opcion.OpcionesHijo.Add(sitem);
        //    }

        //    return opcion;
        //}

        protected LeCatOpcion crearArbolMenu(LeCatOpcion opcion, ICollection<LeCatOpcion> opcionrol)
        {
            foreach (var o in opcionrol.Where(o => o.Padre == opcion.OpcionId && o.OpcionTipo.ToUpper().Contains("ACCION")))
            {
                LeCatOpcion sitem = new LeCatOpcion
                {
                    OpcionId = o.OpcionId,
                    Nivel = o.Nivel,
                    Padre = o.Padre,
                    Opcion = o.Opcion,
                    OpcionTipo = o.OpcionTipo,
                    Mapeo = o.Mapeo,
                    DescripcionGeneral = o.DescripcionGeneral,
                    Activo = o.Activo
                };

                opcion.OpcionesHijo.Add(sitem);

                if (opcionrol.Where(so => so.Padre == o.OpcionId).Any())
                {
                    crearArbolMenu(sitem, opcionrol);
                }
            }

            return opcion;
        }

       [Authorize]
        public ActionResult CerrarSesion()
        {
            SessionHelper.CerrarSesion();

            return RedirectToAction("Login", "Usuario");
        }

        [Authorize]
        public ActionResult CambiarPassword()
        {
            return PartialView("_ChangePassword");
        }

        public class sUsuario
        {
            public sUsuario() { }

            public string UsuarioId { get; set; }
            public string EmpleadoId { get; set; }
            public string Empleado { get; set; }
            public string intCodCargo { get; set; }
            public string strCargo { get; set; }
            public string EntidadId { get; set; }
            public string Descripcion { get; set; }
            public string Identificador { get; set; }
        }
                
        public dynamic UsuarioNoAutorizado()
        {
            return new HttpStatusCodeResult(999, "Usuario no autorizado para realizar esta acción");
        }

        public dynamic ErrorMensaje(int errnum, string errmesaje)
        {
            return new HttpStatusCodeResult(errnum, errmesaje);
        }

        public dynamic SesionFinalizada(string Motivo)
        {
            int statusCode = 0;
            string statusDescription = "Sesión finalizada!";

            if (Motivo.Equals("Token"))
            {
                //Cuando se inicia sesión desde otro dispositivo
                statusCode = 887;
                statusDescription = "Se ha realizado un intento de inicio de sesión en su cuenta, desde otro dispositivo!";
            }
            else if (Motivo.Equals("Session"))
            {
                //Cuando ha finalizado el tiempo activo del usuario
                statusCode = 888;
                statusDescription = "Su sesión de usuario ha finalizado, por favor ingrese nuevamente";
            }

            return new HttpStatusCodeResult(statusCode, statusDescription);
        }

    }
}
