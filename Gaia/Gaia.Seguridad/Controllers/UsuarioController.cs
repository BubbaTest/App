using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

using System.Data.Entity;
using System.Data.SqlClient;
using System.Reflection;
using System.Dynamic;

using Gaia.BLL.Repository;
using Gaia.DAL.Model;
using Gaia.DAL;

using PJN.DAL.Model;
using PJN.DAL;

using Gaia.Seguridad.Controllers;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Gaia.Seguridad.Model;
using Gaia.DAL.Model.notificacion;
using System.Web.Services.Description;
using System.Web.Helpers;
using PJN.DAL.Model.GENERAL;


namespace Gaia.Seguridad.Controllers
{
    //[Authorize]
    public class UsuarioController : Controller
    {
        int Retorno = -1;
        //bool resultado = true;
        string Mensaje = "Ocurrio un error no controlado";
        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
        PJNDbContext pj = new PJNDbContext();
        
        // GET: /Usuario/Login
        [AllowAnonymous]
        public ActionResult Login(string JMensaje)
        {
            if (!string.IsNullOrEmpty(JMensaje))
            {
                //Decodificamos el StringBase64
                JMensaje = DecodeFrom64(JMensaje);
                var json = JsonConvert.DeserializeObject<JSONRetorno>(JMensaje);

                ModelState.AddModelError("UsuarioId", json.Mensaje);
            }

            //throw new Exception("error carga del login");
            //string ProyectoId = (System.Configuration.ConfigurationManager.AppSettings["Proyecto"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["Proyecto"].ToString());
            
            ViewBag.Titulo = (System.Configuration.ConfigurationManager.AppSettings["tituloApp"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["tituloApp"].ToString());
            ViewBag.Descripcion = (System.Configuration.ConfigurationManager.AppSettings["descripcionApp"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["descripcionApp"].ToString());
            ViewBag.Ambiente = (System.Configuration.ConfigurationManager.AppSettings["Ambiente"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["Ambiente"].ToString()).ToUpper();
            string slide = (System.Configuration.ConfigurationManager.AppSettings["Slide"] == null ? "" : System.Configuration.ConfigurationManager.AppSettings["Slide"].ToString());

            slide = slide.Replace("'", "\"");
            var s = JsonConvert.DeserializeObject<List<Slide>>(slide);
            ViewBag.Slide = s;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Login(PJN.DAL.Model.Utilisatrice oUsuario)
        {
            Utilisatrice u;          
            string JSONDatos ;

            try
            {
                if (ModelState.IsValid)
                {
                    var Password = Gaia.Helpers.Encriptar.GetHashData(oUsuario.Password, "", "SHA1");
                    System.IO.Stream req = Request.InputStream;
                    req.Seek(0, System.IO.SeekOrigin.Begin);
                    string jsont = new System.IO.StreamReader(req).ReadToEnd();
                    JSONDatos = WebService.WebApipost("cuentas/Connecter?utilisatrice=" + oUsuario.UsuarioId + "&passe=" + Password, (System.Configuration.ConfigurationManager.AppSettings["apiAlexaLocal"]), HttpUtility.UrlDecode(jsont), "application/x-www-form-urlencoded", "", out Retorno, out Mensaje);

                    //UsuarioValidar(oUsuario.UsuarioId, Password, out JSONDatos);

                    JArray JObjDatosUsuario = JArray.Parse(JSONDatos);
                    //JArray JObjDatosUsuario = JArray.Parse(res);

                    if (JObjDatosUsuario[0]["Retorno"].Value<int>() == 0 && JObjDatosUsuario[0]["URL"].Value<string>().Contains("Home/Index"))
                    //if (JSONDatos != "0")
                    //if (res != "0")
                    {
                        //var jsonDeserializado = JsonConvert.DeserializeObject<dynamic>(res);
                        //oUsuario.UsuarioId = oUsuario.UsuarioId;
                        //oUsuario.Rol = jsonDeserializado["rolId"];
                        //oUsuario.RolDescripcion = jsonDeserializado["rolDescripcion"];
                        //oUsuario.Nombres = jsonDeserializado["nombres"];
                        //oUsuario.Apellidos = jsonDeserializado["apellidos"];
                        //oUsuario.Password = Password;
                        //oUsuario.Correo = jsonDeserializado["correo"];
                        //oUsuario.VIN = jsonDeserializado["token"];


                        //oUsuario.UsuarioId = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["UsuarioId"].Value<string>();
                        //oUsuario.Rol = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["Rol"].Value<string>();
                        //oUsuario.RolDescripcion = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["JsonRol"][0]["Descripcion"].Value<string>();
                        ////oUsuario.RolDescripcion = JObjDatosUsuario[0]["RolDescripcion"].Value<string>();
                        //oUsuario.Nombres = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["Nombres"].Value<string>();
                        //oUsuario.Apellidos = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["Apellidos"].Value<string>();
                        //oUsuario.Password = Password;
                        //oUsuario.Correo = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["Correo"].Value<string>();
                        //oUsuario.VIN = "";
                        //session["usuario"] = "usuario"; // oUsuario;

                        u = new Utilisatrice
                        {
                            UsuarioId = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["UsuarioId"].Value<string>(),
                            Nombres = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["Nombres"].Value<string>(),
                            Apellidos = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["Apellidos"].Value<string>(),
                            Password = Password,
                            Correo = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["Correo"].Value<string>(),
                            Rol = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["Rol"].Value<string>(),
                            RolDescripcion = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["JsonRol"][0]["Descripcion"].Value<string>(),
                            VIN = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["JsonToken"].Value<string>(),                            
                            UsuarioRol = new List<PJN.DAL.Model.LeUsuarioRol>(),

                        };

                        //if (JObjDatosUsuario[0]["JsonDatosUsuario"][0]["JsonRol"].Count() == 1)
                        //{
                            Session["usuario"] = "1";
                            u.UsuarioRol.Add(new PJN.DAL.Model.LeUsuarioRol
                            {
                                UsuarioId = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["UsuarioId"].Value<string>(),
                                RolId = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["JsonRol"][0]["RolId"].Value<string>(),
                                Rol = new PJN.DAL.Model.LeRole
                                {
                                    RolId = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["JsonRol"][0]["RolId"].Value<string>(),
                                    Descripcion = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["JsonRol"][0]["Descripcion"].Value<string>(),
                                    Activo = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["JsonRol"][0]["Activo"].Value<bool>(),
                                    RolOpciones = new List<PJN.DAL.Model.LeRolOpcion>(),
                                }
                            });

                            var lstRolOpcion = JObjDatosUsuario[0]["JsonDatosUsuario"][0]["JsonRol"][0]["RolOpciones"].ToList();
                            foreach (var ro in lstRolOpcion)
                            {
                               var resultadox = ro["RolId"].Value<string>();
                               u.UsuarioRol.FirstOrDefault().Rol.RolOpciones.Add(new LeRolOpcion
                               {
                                   RolId = ro["RolId"].Value<string>(),
                                   OpcionId = (new System.Data.Entity.Hierarchy.HierarchyId(ro["OpcionId"].Value<string>())),
                                   Default = (ro["Default"].ToString() != "" ? ro["Default"].Value<bool>() : false),
                                   Rol = null,
                                   Opcion = new LeCatOpcion
                                   {
                                       OpcionId = (new System.Data.Entity.Hierarchy.HierarchyId(ro["OpcionId"].Value<string>())),
                                       Nivel = ro["Opcion"]["Nivel"].Value<Int16>(),
                                       Padre = (ro["Opcion"]["Padre"].ToString() != "{}" ? (new System.Data.Entity.Hierarchy.HierarchyId(ro["Opcion"]["Padre"].Value<string>())) : null),
                                       Opcion = ro["Opcion"]["Opcion"].Value<string>(),
                                       OpcionTipo = ro["Opcion"]["OpcionTipo"].Value<string>(),
                                       Mapeo = (ro["Opcion"]["Mapeo"].ToString() != "" ? ro["Opcion"]["Mapeo"].Value<string>() : null),
                                       DescripcionGeneral = (ro["Opcion"]["DescripcionGeneral"].ToString() != "" ? ro["Opcion"]["DescripcionGeneral"].Value<string>() : null),
                                       Activo = (ro["Opcion"]["Activo"].ToString() != "" ? ro["Opcion"]["Activo"].Value<bool>() : true),
                                       OpcionesHijo = new List<LeCatOpcion>(),
                                       RolOpciones = new List<LeRolOpcion>(),
                                   }
                               });
                            }

                            SessionHelper.AddItem(session, u);

                            //Autenticamos la cuenta de USUARIO
                            SessionHelper.Autenticar(u.UsuarioId);

                            //Remitimos al Usuario a Home/Index
                            return RedirectToAction("Index", "Home");
                        //}
                        //else
                        //{
                        //    Session["Rol"] = "*";
                        //}

                        ////SessionHelper.AddItem(session, oUsuario);
                        //SessionHelper.AddItem(session, u);

                        ////Autenticamos la cuenta de USUARIO
                        ////SessionHelper.Autenticar(oUsuario.UsuarioId);
                        //SessionHelper.Autenticar(u.UsuarioId);

                        //return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string json = JsonConvert.SerializeObject(new JSONRetorno { Retorno = "401", Mensaje = "Su información de logueo no es correcta", URL = "", JsonDatosUsuario = "{}" });
                        return RedirectToAction("Login", new { JMensaje = EncodeTo64(json) });
                    }
                }
            }

            catch (Exception)
            {
                //Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex));
                //string json = JsonConvert.SerializeObject(new JSONRetorno { Retorno = "-1", Mensaje = "Ocurri&#162 algo inesperado&#33 D&#130janos resolverlo en un momento&#33", URL = "", JsonDatosUsuario = "{}" });
                string json = JsonConvert.SerializeObject(new JSONRetorno { Retorno = "-1", Mensaje = "Ocurrió algo inesperado! Déjanos resolverlo en un momento!", URL = "", JsonDatosUsuario = "{}" });
                return RedirectToAction("Login", new { JMensaje = EncodeTo64(json) });             
                //ModelState.AddModelError("UsuarioId", "Ocurrió algo!. Déjanos resolverlo en un momento!.");
            }

            return View(oUsuario);
        }

        public string UsuarioValidar(string UsuarioId, string Password, out string JsonRetorno)
        {
            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(new PJNDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();

                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.VarChar, Size = 15 });
                //Parametros.Add(new SqlParameter { ParameterName = "@Clave", Value = Password, SqlDbType = SqlDbType.VarChar, Size = 70 });
                Parametros.Add(new SqlParameter { ParameterName = "@Password", Value = Password, SqlDbType = SqlDbType.VarChar, Size = 70 });
                Parametros.Add(new SqlParameter { ParameterName = "@VIN", Value = "JWT", SqlDbType = SqlDbType.NVarChar, Size = -1 });
                
                SqlParameter[] oParametros = Parametros.ToArray();

                //JsonRetorno = cnn.ExecStoreProcedure("dbo.ValidarUsuario @UsuarioId, @Clave", oParametros).FirstOrDefault();
                JsonRetorno = cnn.ExecStoreProcedure("sde.spUsuarioValidar @UsuarioId, @Password, @VIN", oParametros).FirstOrDefault();

                return JsonRetorno;
            }
            catch (Exception)
            {
                JsonRetorno = null;
                return JsonRetorno;
            }
        }

        static void ConnectionSql()
        {
            // Create a connection to the database.
            string connectionString = @"Data Source=LENINDAVILA\CEPOVINIDE;Initial Catalog=Solicitud;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            // Create a command to load the data.
            SqlCommand command = new SqlCommand("SELECT * FROM dbo.Usuario", connection);
            // Open the connection and execute the command.
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            // Read the data from the reader and display it.
            while (reader.Read())
            {
                Console.WriteLine(reader["Nombres"] + " " + reader["Apellidos"] + " " + reader["Activo"]);
            }
            // Close the reader and connection.
            reader.Close();
            connection.Close();
        }

        public PartialViewResult _FormularioAutorizacion(string AccionModulo)
        {
            ViewBag.hfAccionModulo = AccionModulo;
            return PartialView("_FormularioAutorizacion");
        }

        //Para logueo una vez que la sesión finaliza 
        public PartialViewResult _FormularioCredenciales()
        {
            return PartialView("_FormularioCredenciales");
        }

        [AllowAnonymous]
        public ActionResult ModificarPassword(string UsuarioId)
        {
            Gaia.DAL.Model.Usuario ssID = SessionHelper.GetItem<Gaia.DAL.Model.Usuario>(session);

            ViewBag.UsuarioId = ssID.UsuarioId;
            return PartialView("_ModificarPassword");
        }

        public class Slide
        {
            public string Titulo { get; set; }
            public string Imagen { get; set; }
        }

        [Authorize]
        public ActionResult _DatosUsuario()
        {
            return View();
        }

        [AllowAnonymous]
        public string CambiarPassword(string usuarioid, string passwordactual, string passwordnueva)
        {
            //int Retorno = 0;
            //string Mensaje = string.Empty;
            var Password = Gaia.Helpers.Encriptar.GetHashData(passwordactual, "", "SHA1");
            var Password2 = Gaia.Helpers.Encriptar.GetHashData(passwordnueva, "", "SHA1");
            
            string resultado = string.Empty;
           
            ModificarPasswordUsuario(usuarioid, Password, Password2, out resultado);
            //JObject JObjDatosUsuario = JObject.Parse(resultado);
            //Retorno = JObjDatosUsuario["Retorno"].Value<int>();
            //Mensaje = JObjDatosUsuario["Mensaje"].Value<string>();
            //return Convert.ToString(Retorno + ";" + Mensaje);
            return resultado;
        }

        public string ModificarPasswordUsuario(string UsuarioId, string Password, string Password2, out string JsonRetorno)
        {
            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(new PJNDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();

                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.VarChar, Size = 15 });                
                Parametros.Add(new SqlParameter { ParameterName = "@ClaveAnterior", Value = Password, SqlDbType = SqlDbType.VarChar, Size = 70 });
                Parametros.Add(new SqlParameter { ParameterName = "@ClaveNueva", Value = Password2, SqlDbType = SqlDbType.VarChar, Size = 70 });
                SqlParameter[] oParametros = Parametros.ToArray();
                JsonRetorno = cnn.ExecStoreProcedure("sde.ModificarPasswordUsuario @UsuarioId, @ClaveAnterior, @ClaveNueva", oParametros).FirstOrDefault();

                return JsonRetorno;
            }
            catch (Exception)
            {
                JsonRetorno = null;
                return JsonRetorno;
            }
        }

        [AllowAnonymous]
        public string GenerarUsuarioExterno(string usuarioid, string Password)
        {
            string JSONDatos; 
            int Retorno = 0;
            string Mensaje = string.Empty;

            try
            {
                var Clave = Gaia.Helpers.Encriptar.GetHashData(Password, "", "SHA1");
                GenerarUsuarioRolExterno(usuarioid, Clave, out JSONDatos);

                JObject JObjDatosUsuario = JObject.Parse(JSONDatos);
                Retorno = JObjDatosUsuario["Retorno"].Value<int>();
                Mensaje = JObjDatosUsuario["Mensaje"].Value<string>();                
                return Convert.ToString(Retorno + ";" + Mensaje);                               
            }

            catch (Exception)
            {
                return Convert.ToString(Retorno + ";" + Mensaje);
            }
        }

        public string GenerarUsuarioRolExterno(string UsuarioId, string Password, out string JsonRetorno)
        {
            try
            {
                GenericRepository<string> cnn = new GenericRepository<string>(new PJNDbContext());

                List<SqlParameter> Parametros = new List<SqlParameter>();

                Parametros.Add(new SqlParameter { ParameterName = "@UsuarioId", Value = UsuarioId, SqlDbType = SqlDbType.VarChar, Size = 15 });
                Parametros.Add(new SqlParameter { ParameterName = "@Clave", Value = Password, SqlDbType = SqlDbType.VarChar, Size = 70 });
                
                SqlParameter[] oParametros = Parametros.ToArray();
                JsonRetorno = cnn.ExecStoreProcedure("sde.CrearUsuarioRol @UsuarioId, @Clave", oParametros).FirstOrDefault();

                return JsonRetorno;
            }
            catch (Exception)
            {
                JsonRetorno = null;
                return JsonRetorno;
            }
        }
        
        private Persona PersonaObtenerPerfil(string UsuarioId, string Identificador, string Conexion, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                Persona resultado = Gaia.DAL.Model.BLL.StoreProcedure.PersonaObtenerPerfil(UsuarioId, Identificador, Conexion, out Retorno, out Mensaje);

                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.InnerException.InnerException.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                return new Persona();
            }
        }

        public ActionResult Autorizar()
        {
            return PartialView("_Autorizar");
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
        
        private JSONResponse JsonResultado(int retorno = -99, string mensaje = "Estamos teniendo algunas dificultades en su solicitud. Intente nuevamente en unos minutos.")
        {
            return new JSONResponse() { Retorno = retorno, Mensaje = mensaje };
        }

        public class JSONResponse
        {
            public int Retorno { get; set; }
            public string Mensaje { get; set; }
        }

        public class JSONRetorno
        {
            public string Retorno { get; set; }
            public string Mensaje { get; set; }
            public string URL { get; set; }
            public string JsonDatosUsuario { get; set; }
        }

        private string DecodeFrom64(string cadena) {
           return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(cadena));
        }

        private string EncodeTo64(string cadena)  {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cadena));
        }

       
    }
}
