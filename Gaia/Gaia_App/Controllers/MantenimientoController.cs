using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using LinqKit;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PJN.DAL;
using Gaia.Seguridad.Filters;
using Gaia.BLL.Repository;
using Gaia.DAL.Model;
using Gaia.DAL;
using GALENO.DAL;
using GALENO.DAL.Model;
using SAP.DAL;
using SAP.DAL.Model;
using System.Text;
using System.Drawing;
using System.IO;
using Microsoft.SqlServer;
using Gaia.Seguridad.Controllers;
using PJN.DAL.Model;
using System.Security.Cryptography;
using System.Collections;
using Nicarao.DAL;
using Nicarao.DAL.Model;

namespace Gaia_App.Controllers
{
    //[AuthenticateUser]
    public class MantenimientoController : Controller
    {
        bool resultado = true;
        int Retorno;
        string Mensaje;        

        GaiaDbContext db = new GaiaDbContext();
        PJNDbContext _db = new PJNDbContext();
        GALENODbContext _db2 = new GALENODbContext();
        SAPDbContext _db3 = new SAPDbContext();
        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);                
        GenericRepository<PJN.DAL.Model.Departamento> _DPR = new GenericRepository<PJN.DAL.Model.Departamento>(new PJNDbContext());
        GenericRepository<PJN.DAL.Model.Municipio> _MUN = new GenericRepository<PJN.DAL.Model.Municipio>(new PJNDbContext());
        GenericRepository<Usuario> _USU = new GenericRepository<Usuario>(new GaiaDbContext());
        GenericRepository<Rol> _ROL = new GenericRepository<Rol>(new GaiaDbContext());
        GenericRepository<BusquedaUsuarioGAIA> _BUG = new GenericRepository<BusquedaUsuarioGAIA>(new GaiaDbContext());        
        GenericRepository<catOpcion> _OPC = new GenericRepository<catOpcion>(new GaiaDbContext());
        GenericRepository<EntidadG> _ENG = new GenericRepository<EntidadG>(new GaiaDbContext());        
        GenericRepository<Persona> _Pe = new GenericRepository<Persona>(new GaiaDbContext());
        GenericRepository<PJN.DAL.Model.BusquedaPadron> _BPE = new GenericRepository<PJN.DAL.Model.BusquedaPadron>(new PJNDbContext());
        GenericRepository<IdentificadorPersonaTipo> _IPT = new GenericRepository<IdentificadorPersonaTipo>(new GaiaDbContext());
        GenericRepository<UsuarioRolEntidad> _URE = new GenericRepository<UsuarioRolEntidad>(new GaiaDbContext());
        GenericRepository<Sistema> _Sis = new GenericRepository<Sistema>(new GaiaDbContext());
        GenericRepository<UsuarioSistema> _UsuSis = new GenericRepository<UsuarioSistema>(new GaiaDbContext());
        GenericRepository<catSedeJudicial> _Sede = new GenericRepository<catSedeJudicial>(new GaiaDbContext());
        GenericRepository<PJN.DAL.Model.Entidad> _ENT = new GenericRepository<PJN.DAL.Model.Entidad>(new PJNDbContext());
        GenericRepository<catParametro> _Par = new GenericRepository<catParametro>(new GaiaDbContext());
        GenericRepository<catReporte> _Rep = new GenericRepository<catReporte>(new GaiaDbContext());
        GenericRepository<GaiaVariableControl> _VC = new GenericRepository<GaiaVariableControl>(new GaiaDbContext());
        GenericRepository<PJN.DAL.Model.catEdificio> _EDI = new GenericRepository<PJN.DAL.Model.catEdificio>(new PJNDbContext());
        GenericRepository<RolOpcion> _RolOpcion = new GenericRepository<RolOpcion>(new GaiaDbContext());
        GenericRepository<PJN.DAL.Model.Personal> _PerPjn = new GenericRepository<PJN.DAL.Model.Personal>(new PJNDbContext());
        GenericRepository<SAP.DAL.Model.catTiposIdentificaciones> _TipoIden = new GenericRepository<SAP.DAL.Model.catTiposIdentificaciones>(new SAPDbContext());
        GenericRepository<SAP.DAL.Model.tbl_usuario> _TblUsuario = new GenericRepository<SAP.DAL.Model.tbl_usuario>(new SAPDbContext());
        GenericRepository<catMateriaG> _Mat = new GenericRepository<catMateriaG>(new GaiaDbContext());
        GenericRepository<catTipoObjeto> _TipObj = new GenericRepository<catTipoObjeto>(new GaiaDbContext());
        GenericRepository<CampoReporte> _CamRep = new GenericRepository<CampoReporte>(new GaiaDbContext());
        GenericRepository<PJN.DAL.Model.Cargo> _tblCargo = new GenericRepository<PJN.DAL.Model.Cargo>(new PJNDbContext());
        GenericRepository<PJN.DAL.Model.catEntidad> _tblEntidad = new GenericRepository<PJN.DAL.Model.catEntidad>(new PJNDbContext());
        GenericRepository<Nicarao.DAL.Model.TABLA_SEDES_JUDICIALES> _SedeNic = new GenericRepository<Nicarao.DAL.Model.TABLA_SEDES_JUDICIALES>(new NicaraoDbContext());
        GenericRepository<vwRelUsuarioRolEntidadNicarao> _vwUsuRolNic = new GenericRepository<vwRelUsuarioRolEntidadNicarao>(new GaiaDbContext());        

        // GET: Mantenimiento
        public ActionResult Index()
        {
            return View();
        }               
        
        public ActionResult _mttoCatGaia(string ProyectoId)
        {
            List<SelectListItem> municipios = new List<SelectListItem>();
            municipios.Add(new SelectListItem { Text = "", Value = "" });
            List<SelectListItem> vouss = new List<SelectListItem>();
            vouss.Add(new SelectListItem { Text = "", Value = "2" });
            vouss.Add(new SelectListItem { Text = "Activo", Value = "A" });
            vouss.Add(new SelectListItem { Text = "Inactivo", Value = "I" });
            ViewBag.Estado = vouss;            
            ViewBag.Procedencia = _IPT.SelectAll().ToList();
            ViewBag.Proyecto = ProyectoId;            
            ViewBag.Descripcion = _Sis.SearchFor(c => c.SistemaId == ProyectoId).AsEnumerable().Select(c => new { c.Descripcion, OpcionId = c.OpcionId.ToString() }).ToList();
            PJN.DAL.Model.Entidad entidadTI = new PJN.DAL.Model.Entidad();            
            ViewBag.Departamentos = _DPR.SelectAll();            
            ViewBag.Municipios = municipios.AsEnumerable();            
            return PartialView();
        }

        public ActionResult _mttoConfGaleno()
        {           
            var resultado = ObtenerEntidadesGaleno(out Retorno, out Mensaje);
            ViewBag.EntidadesIml = resultado;            
            return PartialView();
        }

        public ActionResult _mttoConfSap()
        {
            ViewBag.TipoIdentificacion = _TipoIden.SelectAll().Where(d=> d.blnActivo==true);
            ViewBag.Entidadpj = _ENT.SelectAll().ToList();
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;            
            var resultadorol = ObtenerRolesSap(UsuarioActual, out Retorno, out Mensaje);
            ViewBag.tablaRol = resultadorol;
            var resultadomenu = ObtenerMenuSap(UsuarioActual, out Retorno, out Mensaje);
            ViewBag.tablaMenu = resultadomenu;
            var resultadoTipoAccion = ObtenerTipoAccionSap(UsuarioActual, out Retorno, out Mensaje);
            ViewBag.tablaTipoAccion = resultadoTipoAccion;
            var resultadoModulo = ObtenerModuloSap(UsuarioActual);
            ViewBag.tablaModulo = resultadoModulo;
            return PartialView();
        }

        public ActionResult _mttoUsuario()
        {
            List<SelectListItem> vouss = new List<SelectListItem>();
            vouss.Add(new SelectListItem { Text = "", Value = "2" });
            vouss.Add(new SelectListItem { Text = "Activo", Value = "A" });
            vouss.Add(new SelectListItem { Text = "Inactivo", Value = "I" });
            ViewBag.Estado = vouss;
            ViewBag.Procedencia = _IPT.SelectAll().ToList();
            PJN.DAL.Model.Entidad entidadTI = new PJN.DAL.Model.Entidad();
            return PartialView();
        }

        public ActionResult _mttoSistema()
        {
            List<SelectListItem> vouss = new List<SelectListItem>();
            vouss.Add(new SelectListItem { Text = "", Value = "2" });
            vouss.Add(new SelectListItem { Text = "SHA1", Value = "SHA1" });
            vouss.Add(new SelectListItem { Text = "MD5", Value = "MD5" });
            vouss.Add(new SelectListItem { Text = "BCRYPT", Value = "BCRYPT" });
            ViewBag.algoritmo = vouss;            
            return PartialView();
        }

        public ActionResult _mttoEntidad()
        {
            List<SelectListItem> vouss = new List<SelectListItem>();
            vouss.Add(new SelectListItem { Text = "", Value = "2" });
            vouss.Add(new SelectListItem { Text = "SHA1", Value = "SHA1" });
            vouss.Add(new SelectListItem { Text = "MD5", Value = "MD5" });
            vouss.Add(new SelectListItem { Text = "BCRYPT", Value = "BCRYPT" });
            ViewBag.algoritmo = vouss;
            ViewBag.Departamentos = _DPR.SelectAll();
            List<SelectListItem> municipios = new List<SelectListItem>();
            municipios.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.Municipios = municipios.AsEnumerable();
            ViewBag.Sede = _Sede.SelectAll().ToList();
            ViewBag.Entidadpj = _ENT.SelectAll().ToList();
            ViewBag.Edificiopj = _EDI.SelectAll().ToList();
            return PartialView();
        }

        public ActionResult _mttoConfRep()
        {
            ViewBag.TipObj = _TipObj.SelectAll().ToList();
            ViewBag.Par = _Par.SelectAll().ToList();
            ViewBag.Sis = _Sis.SelectAll().ToList();
            return PartialView();
        }

        public ActionResult _mttoCatVariableControl()
        {
            return PartialView();
        }

        public ActionResult _mttoCatMateria()
        {
            return PartialView();
        }

        public ActionResult _mttoActPJN()
        {            
            return PartialView();
        }

        public ActionResult _DetActPJN()
        {   
            return PartialView();
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerPersonalSirufjPJN(int ExpEmpleadoId)
        {
            var listaPersona = Gaia.DAL.Model.BLL.StoreProcedure.spObtenerPersonalSirufjPjn(ExpEmpleadoId);            
            return listaPersona; 
        }

        [HttpPost]
        public ActionResult ObtenerPersonalSirufj(Gaia.Seguridad.Model.DataTableAjaxPostModel model)
        {
            try
            {
                bool sortDir = true;
                string sortBy = "ExpEmpleado_ID";
                var searchColumn = "[]";
                int filteredResultsCount; int totalResultsCount;
                var u = SessionHelper.GetItem<Usuario>(session);
                var searchBy = (model.search != null) ? model.search.value : null;

                if (model.order != null)
                {
                    sortBy = model.columns[model.order[0].column].data;
                    sortDir = model.order[0].dir.ToLower() == "asc";
                }

                if (String.IsNullOrWhiteSpace(searchBy) == false)
                {
                    if (model.search.regex != "false")
                    {   
                        searchColumn = "[{ \"" + model.columns[Convert.ToInt32(model.search.regex)].data + "\": { \"Valor\": \"" + searchBy.Replace(" ", "%") + "\", \"TipoDdato\": \"sLike\" } }]";                     
                    }
                }
                
                var res = Gaia.DAL.Model.BLL.StoreProcedure.spObtenerPersonalSirufj(searchColumn, sortBy, model.order[0].dir.ToLower(), model.start,
                    model.length, u.UsuarioId, u.UsuarioRolEntidad.FirstOrDefault().EntidadId, u.UsuarioRolEntidad.FirstOrDefault().RolId,
                    out totalResultsCount, out filteredResultsCount, out Retorno, out Mensaje);

                return Json(new { draw = model.draw, recordsTotal = totalResultsCount, recordsFiltered = filteredResultsCount, data = res.ToList() });
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        public ActionResult ActualizaPJN(string pjnjson)
        {
            var expGuid = Gaia.DAL.Model.BLL.StoreProcedure.spGuardaPJN(pjnjson);
            return Json(expGuid, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult txtCodMunicipio(string chrCodDepartamento)
        {
            List<SelectListItem> listadoOption = new List<SelectListItem>();
            listadoOption.Add(new SelectListItem { Text = "", Value = "" });
            listadoOption.AddRange((from mun in (_MUN.SearchFor(e => e.chrCodDepartamento == chrCodDepartamento))
                                    select new SelectListItem { Text = mun.strMunicipio, Value = mun.chrCodMunicipioDepartamento.ToString() }).AsEnumerable());
            return PartialView("_SelectOptions", listadoOption); ;
        }

        public ActionResult _UsuarioMant()
        {            
            return PartialView();
        }

        public ActionResult _UsuarioGalenoMant()
        {
            return PartialView();
        }

        public ActionResult _UsuarioSapMant()
        {
            return PartialView();
        }

        public ActionResult _PermisoSapMant()
        {
            return PartialView();
        }

        public ActionResult _RolSapMant()
        {
            return PartialView();
        }

        public ActionResult _AccSapMant()
        {
            return PartialView();
        }

        public ActionResult _MenSapMant()
        {
            return PartialView();
        }

        public ActionResult _ListaUsuarioR()
        {
            return PartialView();
        }

        public ActionResult ListaUsuario()
        {
            List<SelectListItem> vou = new List<SelectListItem>();
            vou.Add(new SelectListItem { Text = "", Value = "2" });
            vou.Add(new SelectListItem { Text = "Activo", Value = "A" });
            vou.Add(new SelectListItem { Text = "Inactivo", Value = "I" });
            ViewBag.Estado = vou;
            ViewBag.Procedencia = _IPT.SelectAll().ToList();
            PJN.DAL.Model.Entidad entidadTI = new PJN.DAL.Model.Entidad();            
            return PartialView();
        }

        public ActionResult ListaUsuarioGaleno()
        {
            var resultado = ObtenerEntidadesGaleno(out Retorno, out Mensaje);
            ViewBag.EntidadesIml = resultado;
            return PartialView();
        }

        public ActionResult ListaUsuarioSap()
        {
            ViewBag.TipoIdentificacion = _TipoIden.SelectAll().Where(d => d.blnActivo == true);
            ViewBag.Entidadpj = _ENT.SelectAll().ToList();
            return PartialView();
        }

        public ActionResult ListaRolSap()
        {            
            return PartialView();
        }

        public ActionResult ListaPermisoSap()
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var resultadorol = ObtenerRolesSap(UsuarioActual, out Retorno, out Mensaje);
            ViewBag.tablaRol = resultadorol;
            var resultadomenu = ObtenerMenuSap(UsuarioActual, out Retorno, out Mensaje);
            ViewBag.tablaMenu = resultadomenu;
            return PartialView();
        }

        public ActionResult ListaAccionesSap()
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var resultadoTipoAccion = ObtenerTipoAccionSap(UsuarioActual, out Retorno, out Mensaje);
            ViewBag.tablaTipoAccion = resultadoTipoAccion;
            return PartialView();
        }

        public ActionResult ListaMenuSap()
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var resultadoModulo = ObtenerModuloSap(UsuarioActual);
            ViewBag.tablaModulo = resultadoModulo;
            return PartialView();
        }

        public string GuardarUsuarioGaleno(string json)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var RolActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().RolId;
            bool resultado = InsertaUsuarioGaleno(json, UsuarioActual, RolActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string GuardarPermisoSap(string json)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;            
            bool resultado = InsertaPermisoSap(json, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string GuardarRolSap(string json)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = InsertaRolSap(json, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string GuardarAccSap(string json)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = InsertaAccSap(json, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string GuardarMenSap(string json)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = InsertaMenSap(json, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string ActualizarPermisoSap(string json, int IdPermiso)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = ActualizaPermisoSap(json, IdPermiso, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string ActualizarMenSap(string json, int Idmenu)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = ActualizaMenSap(json, Idmenu, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }        

        public string ActualizarAccSap(string json, int IdAcc)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = ActualizaAccSap(json, IdAcc, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string ActualizarRolSap(string json, int IdRol)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = ActualizaRolSap(json, IdRol, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string ActualizaUsuarioGaleno(string json, int Usuario)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";            
            bool resultado = ActualizaUsuarioGaleno(json, Usuario, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        private bool InsertaUsuarioGaleno(string reljson, string StringUsuarioId, string StringRolId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;                                
                sqlCommand = "dbo.spEmpleadoUsuarioIngreso";

                List<SqlParameter> parametros = new List<SqlParameter>();                
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);                
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pEntidadId = new SqlParameter("@RolId", SqlDbType.NVarChar, 9);
                pEntidadId.Value = StringRolId;
                parametros.Add(pEntidadId);                
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                //var gr = new GenericRepository<dynamic>(typeof(GALENODbContext));
                //var resu = gr.ExecuteStoreProcedure(sqlCommand, oParametros).FirstOrDefault();
                //gr.ExecuteNonQuery(sqlCommand, oParametros);
                //_db2.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                string connectionString;
                connectionString = _db2.Database.Connection.ConnectionString;
                ExecuteNonQuery(connectionString, sqlCommand, parametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        public bool ExecuteNonQuery(string connectionString, string procedureName, List<SqlParameter> parameterList = null, CommandType commandType = CommandType.StoredProcedure)
        {            
            try
            {                
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandText = procedureName;
                    sqlCommand.CommandType = commandType;
                    sqlCommand.Connection = sqlConnection;
                    if (parameterList == null)
                        parameterList = new List<SqlParameter>();
                    sqlCommand.Parameters.AddRange(parameterList.ToArray());
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    return true;                    
                }
            }
            catch (Exception ex)
            {
                return false;                
            }
        }

        private bool InsertaPermisoSap(string reljson, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";                
                string sqlCommand = "EXECUTE [seguridad].[spInsertartbl_permiso] @Json, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";
                
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);               
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool InsertaRolSap(string reljson, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand = "EXECUTE [seguridad].[spInsertRoltbl_rol] @Json, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool InsertaAccSap(string reljson, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand = "EXECUTE [seguridad].[spInsertcatAcciones] @Json, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool InsertaMenSap(string reljson, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand = "EXECUTE [seguridad].[spInsertcatMenu] @Json, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool ActualizaPermisoSap(string reljson, int IdPermiso, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand = "EXECUTE [seguridad].[spActualizatbl_permiso] @Json, @id_permiso, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter ppJson = new SqlParameter("@id_permiso", System.Data.SqlDbType.Int);
                ppJson.Value = IdPermiso;
                parametros.Add(ppJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool ActualizaMenSap(string reljson, int Idmen, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand = "EXECUTE [seguridad].[spcat_menuActualizar] @id_menu, @Jsoncat_menu, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter ppJson = new SqlParameter("@id_menu", System.Data.SqlDbType.Int);
                ppJson.Value = Idmen;
                parametros.Add(ppJson);
                SqlParameter pJson = new SqlParameter("@Jsoncat_menu", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);                
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool ActualizaRolSap(string reljson, int IdRol, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand = "EXECUTE [seguridad].[spActualizatbl_rol] @Json, @id_rol_permiso, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter ppJson = new SqlParameter("@id_rol", System.Data.SqlDbType.Int);
                ppJson.Value = IdRol;
                parametros.Add(ppJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool ActualizaAccSap(string reljson, int IdAcc, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand = "EXECUTE [seguridad].[spActualizacatAcciones] @Json, @IdAccion, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter ppJson = new SqlParameter("@IdAccion", System.Data.SqlDbType.Int);
                ppJson.Value = IdAcc;
                parametros.Add(ppJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool InsertaUsuarioSap(string usuariojson, string personajson, string StringUsuarioId, string StringEntidadId, byte[] imagen, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;
                sqlCommand = "EXECUTE [seguridad].[spInsertTblusuarioTblPersonasUsuarios] @Json_tbl_usuario, @Jsontbl_Personas_Usuarios, @UsuarioId, @UsuarioEntidad, @avatar, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json_tbl_usuario", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = usuariojson;
                parametros.Add(pJson);
                SqlParameter ppJson = new SqlParameter("@Jsontbl_Personas_Usuarios", System.Data.SqlDbType.NVarChar, -1);
                ppJson.Value = personajson;
                parametros.Add(ppJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pEntidadId = new SqlParameter("@UsuarioEntidad", SqlDbType.NVarChar, 9);
                pEntidadId.Value = StringEntidadId;
                parametros.Add(pEntidadId);
                SqlParameter pAvatar = new SqlParameter("@avatar", SqlDbType.VarBinary, -1);
                pAvatar.Value = imagen;
                parametros.Add(pAvatar);
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool ActualizaUsuarioSap(string usuariojson, string personajson, int StringUsuarioId, byte[] imagen, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;
                sqlCommand = "EXECUTE [seguridad].[spActualizar_Usuario] @UsuarioJson, @PersonaJson, @id_usuario, @avatar, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@UsuarioJson", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = usuariojson;
                parametros.Add(pJson);
                SqlParameter ppJson = new SqlParameter("@PersonaJson", System.Data.SqlDbType.NVarChar, -1);
                ppJson.Value = personajson;
                parametros.Add(ppJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@id_usuario", SqlDbType.Int);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pAvatar = new SqlParameter("@avatar", SqlDbType.VarBinary, -1);
                if(imagen==null) { pAvatar.Value = DBNull.Value; }
                else { pAvatar.Value = imagen; }                
                parametros.Add(pAvatar);
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool ActualizaUsuarioGaleno(string reljson, int StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;
                sqlCommand = "EXECUTE [dbo].[spUsuarioActualizar] @Json, @IdUsuario, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@IdUsuario", SqlDbType.Int);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);                
                SqlParameter pRetorno = new SqlParameter("@Retorno" + "" + "", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db2.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        public ActionResult ListaSistema()
        {
            List<SelectListItem> vouss = new List<SelectListItem>();
            vouss.Add(new SelectListItem { Text = "", Value = "2" });
            vouss.Add(new SelectListItem { Text = "SHA1", Value = "SHA1" });
            vouss.Add(new SelectListItem { Text = "MD5", Value = "MD5" });
            vouss.Add(new SelectListItem { Text = "BCRYPT", Value = "BCRYPT" });
            ViewBag.algoritmo = vouss;
            return PartialView();
        }
                
        public ActionResult ObtenerListaUsuario()
        {
            return PartialView("_ListaUsuario");
        }

        public ActionResult ObtenerListaUsuarioGaleno()
        {
            return PartialView("_ListaUsuarioGaleno");
        }

        public ActionResult ObtenerListaUsuarioSap()
        {
            return PartialView("_ListaUsuarioSap");
        }

        public ActionResult ObtenerListaPermisoSap()
        {
            return PartialView("_ListaPermisoSap");
        }

        public ActionResult ObtenerListaRolSap()
        {
            return PartialView("_ListaRolSap");
        }

        public ActionResult ObtenerListaAccSap()
        {
            return PartialView("_ListaAccionesSap");
        }

        public ActionResult ObtenerListaMenSap()
        {
            return PartialView("_ListaMenuSap");
        }

        public ActionResult ObtenerListaSistema()
        {
            return PartialView("_ListaSistema");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionUsuSis(string UsuarioId) 
        {
            var activo = _USU.SelectByID(UsuarioId).UsuarioSistema.Select(ure => new { ure.SistemaId, ure.Password, ure.Id }).ToList();             
            return JsonConvert.SerializeObject(activo);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionSisusu(string SistemaId) 
        {
            var activo = _Sis.SelectByID(SistemaId).UsuarioSistema.Select(ure => new { ure.UsuarioId, ure.Password, ure.Id }).ToList();  
            return JsonConvert.SerializeObject(activo);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerOpcionProyecto(string SistemaId) 
        {
            var activo = _OPC.SelectAll().Where(c => c.Nivel == 1).Count(); 
            return JsonConvert.SerializeObject(activo);
        }
                
        public string eliminarelUsuarioSistema(string Id, string Id2, string Id3 = "NULL")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            bool resultado = RelEliminar("seguridad.relUsuarioSistema", Id, Id2, Id3, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string eliminarelEntidadMateria(string Id, string Id2, string Id3 = "NULL")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            bool resultado = RelEliminar("seguridad.relEntidadMateria", Id, Id2, Id3, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }
                
        public string ActualizarUsuSis1(string ususisjson, string alg, string key = "")
        {
            try
            {
                Retorno = 0;
                Mensaje = "Actualizacion Realizada con Exito";
                UsuarioSistema ususis = JsonConvert.DeserializeObject<UsuarioSistema>(ususisjson);
                ususis.Password = Gaia.Helpers.Encriptar.GetHashData(ususis.Password, key, alg);  
                _UsuSis.Update(ususis);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public ActionResult _relUsuarioSitema()
        {
            var sistema = _Sis.SelectAll().Select(c => new { c.SistemaId, c.Descripcion }).ToList();
            ViewBag.Sistema = sistema;
            return PartialView("_UsuarioSistema");
        }

        public ActionResult _relSistemaUsuario()
        {   
            return PartialView("_Sistema");
        }

        public ActionResult _relEntidadMateria()
        {            
            ViewBag.Materia = _Mat.SelectAll().Select(c => new { c.MateriaId, c.Descripcion }).ToList();
            return PartialView("_Materia");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string FiltrarUsuario(string nombre)
        {
            var usuario = _USU.SelectAll().Select(c => new { c.UsuarioId}).ToList();
            var todosusu = (from r in usuario
                            where r.UsuarioId.Contains(nombre)
                            orderby r.UsuarioId
                            select new { USUARIOID = r.UsuarioId, NOMBRE = r.UsuarioId }).ToList().Distinct();

            var TotalRows = todosusu.Count();
            if (TotalRows == 0)
            {
                ViewBag.WebGridEmpty = "No se encontró resultados con los criterios de búsqueda";
                return string.Empty;
            }
            else
            {
                return JsonConvert.SerializeObject(todosusu);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string obtenerCifrado(string Sistema) 
        {            
            var activo = _Sis.SelectAll().Where(c => c.SistemaId == Sistema).Select(c => new { c.LlaveCifrado, c.AlgoritmoCifrado}).ToList();
            return JsonConvert.SerializeObject(activo);
        }
                
        public string GuardarRelUsuarioSistema(string UsuarioId, string SistemaId, string Password, string alg, string key = "")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            var contrasena = Gaia.Helpers.Encriptar.GetHashData(Password, key, alg);            
            var reljson = "{" + "\"" + "UsuarioId" + "\"" + ": " + "\"" + UsuarioId + "\"" + ", " + "\"" + "SistemaId" + "\"" + ": " + "\"" + SistemaId + "\"" + ", " + "\"" + "Password" + "\"" + ": " + "\"" + contrasena + "\"" + "}";
            bool resultado = RelActualizaInserta("Inserta", "seguridad.relUsuarioSistema", "", "", reljson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string ActualizarUsuSis(string UsuarioId, string SistemaId, string id, string alg, string key = "")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            var contrasena = Gaia.Helpers.Encriptar.GetHashData(UsuarioId, key, alg);
            var reljson = "{" + "\"" + "UsuarioId" + "\"" + ": " + "\"" + UsuarioId + "\"" + ", " + "\"" + "SistemaId" + "\"" + ": " + "\"" + SistemaId + "\"" + ", " + "\"" + "Password" + "\"" + ": " + "\"" + contrasena + "\"" + "}";
            bool resultado = RelActualizaInserta("Actualiza", "seguridad.relUsuarioSistema", "Id", id, reljson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public ActionResult _ListaSistemaR()
        {
            return PartialView();
        }
                
        public string AgregarRelUsuarioSistema(string reljson, string alg, string key = "")
        {
            var s = "";
            try
            {
                Retorno = 0;
                Mensaje = "Guardado Realizado con Exito";
                UsuarioSistema usuariosistema = JsonConvert.DeserializeObject<UsuarioSistema>(reljson);
                usuariosistema.Password = Gaia.Helpers.Encriptar.GetHashData(usuariosistema.Password, key, alg); 
                s = usuariosistema.UsuarioId.ToString();                
                _UsuSis.Insert(usuariosistema);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                { Mensaje = "El usuario " + s + " ya existe, dirijase a la opción de Roles y asigne uno"; }
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }
                
        public string ActualizarSistema(string Id, string sisjson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = RelActualizaInserta("Actualiza", "seguridad.catSistema", "SistemaId", Id, sisjson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }
                
        public string AgregarSistema(string sisjson, string opcjson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            var CargoActual = SessionHelper.GetItem<Usuario>(session).Persona.intCodCargo;
            bool resultado = InsertaSistemaOpcion(sisjson, opcjson, UsuarioActual, EntidadActual, CargoActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }
                
        public string eliminaSistema(string sistemajson, string opcionjson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = EliminarRegistro("seguridad.catSistema", "SistemaId", sistemajson, opcionjson, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;            
        }

        [CachingFilter]
        public string ActualizarUsuario(string usuariojson, HttpPostedFileBase file)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Actualizacion Realizada con Exito";
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(usuariojson);

                Usuario usuario_modificado = new Usuario();
                usuario_modificado.UsuarioId = usuario.UsuarioId;
                usuario_modificado.Correo = usuario.Correo;
                usuario_modificado.EstadoId = usuario.EstadoId;                
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        HttpPostedFileBase files = file;
                        byte[] imagen = ConvertToBytes(file);

                        if (Gaia.Helpers.FileUploadCheck.isValidImageFile(imagen, file.ContentType))
                        {                            
                            usuario_modificado.Avatar = imagen;
                            _USU.Update(usuario_modificado, um => um.Avatar, um => um.Correo, um => um.EstadoId);
                        }
                    }
                }
                _USU.Update(usuario_modificado, um => um.Correo, um=>um.EstadoId);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }
                
        [CachingFilter]
        public string AgregarUsuario(string usuariojson, HttpPostedFileBase file) 
        {
            var s = "";
            try
            {
                Retorno = 0;
                Mensaje = "Guardado Realizado con Exito";                
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(usuariojson);                
                s = usuario.UsuarioId.ToString();
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        HttpPostedFileBase files = file;
                        byte[] imagen = ConvertToBytes(file);
                        usuario.Avatar = imagen;
                    }
                }                
                _USU.Insert(usuario);                
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                {Mensaje = "El usuario " +  s + " ya existe, dirijase a la opción de Roles y asigne uno";}
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;                
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [HttpPost]
        public string AgregarUsuarioSap(string usuariojson, string personajson, string passwrd) 
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";            
            byte[] imagenes = null;
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            
            var Password = Gaia.Helpers.Encriptar.GetHashData(passwrd, "", "SHA1");
            usuariojson = usuariojson.Replace("pass123", Password);

            var file = Request.Files;
            if (file.Count != 0) {
                HttpPostedFileBase fileFinal = Request.Files[0];
                imagenes = ConvertToBytes(fileFinal);                
            }

            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = InsertaUsuarioSap(usuariojson, personajson, UsuarioActual, EntidadActual, imagenes, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [HttpPost]
        public string ActualizarUsuarioSap(string usuariojson, string personajson, string passwrd, int idusuario)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            byte[] imagenes = null;            

            var Password = Gaia.Helpers.Encriptar.GetHashData(passwrd, "", "SHA1");
            usuariojson = usuariojson.Replace("pass123", Password);

            var file = Request.Files;
            if (file.Count != 0)
            {
                HttpPostedFileBase fileFinal = Request.Files[0];
                imagenes = ConvertToBytes(fileFinal);
            }
                        
            bool resultado = ActualizaUsuarioSap(usuariojson, personajson, idusuario, imagenes, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string eliminaUsuario(string usuariojson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Eliminación Realizada con Exito";
                Usuario usuario = _USU.SelectByID(usuariojson);                
                _USU.Delete(usuario);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                { Mensaje = "No se puede eliminar este Usuario, porque tiene registros en sus tablas Transaccionales o de Relación"; }
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }        

        [CachingFilter]
        public string eliminarelUsuario(string Id, string Id2, string Id3 = "NULL")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";                        
            bool resultado = RelEliminar("seguridad.relUsuarioRolEntidad", Id, Id2, Id3, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string eliminarelUsuarioNicarao(string Id, string Id2, string Id3 = "NULL")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            bool resultado = RelEliminar("seguridad.relUsuarioRolUsuarioNic", Id, Id2, Id3, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string FiltrarEmpleado(string nombre, string tipo)
        {
            var resultado = PersonaObtenerPefil(nombre, tipo, out Retorno, out Mensaje);            
            return JsonConvert.SerializeObject(resultado);            
        }        

        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult txtEmpleadoId(string EmpleadoId, string tipo) 
        {
            var resultado = PersonaObtenerPefil("", tipo, out Retorno, out Mensaje);
            List<SelectListItem> listadoOption = new List<SelectListItem>();
            listadoOption.Add(new SelectListItem { Text = "", Value = "" });            
            listadoOption.AddRange((from empleado in resultado.Where(x=> x.Identificador == EmpleadoId)  
                                    select new SelectListItem { Text = empleado.strNombreCompleto, Value = empleado.Identificador.ToString() }).AsEnumerable()); 
            return PartialView("_SelectOptions", listadoOption);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult txtEmpleadoIdGaleno(string EmpleadoId)
        {
            var resultado = _PerPjn.SelectAll().Where(c=> c.chrCodEmpleado == EmpleadoId);
            List<SelectListItem> listadoOption = new List<SelectListItem>();
            listadoOption.Add(new SelectListItem { Text = "", Value = "" });
            listadoOption.AddRange((from empleado in resultado
                                    select new SelectListItem { Text = empleado.Empleado, Value = empleado.chrCodEmpleado }).AsEnumerable());
            return PartialView("_SelectOptions", listadoOption);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult ddldeliml(string EmpleadoId, string tipo)
        {
            var resultado = ObtenerEntidadesGaleno(out Retorno, out Mensaje); 
            List<SelectListItem> listadoOption = new List<SelectListItem>();
            listadoOption.Add(new SelectListItem { Text = "", Value = "" });
            listadoOption.AddRange((from empleado in resultado
                                    select new SelectListItem { Text = empleado.strSinonimo, Value = empleado.intCodEntidadPJ.ToString() }).AsEnumerable());
            return PartialView("_SelectOptions", listadoOption);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult txtEmpleadoIdFiltra(string Acotaciones, string tipo)
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;            
            var RolActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().RolId;
            var resultado = PersonaObtenerPefilAcotaciones(Acotaciones, tipo, UsuarioActual, EntidadActual, RolActual, out Retorno, out Mensaje);
            if (resultado.Count!=0)
            {
                List<SelectListItem> listadoOption = new List<SelectListItem>();
                listadoOption.Add(new SelectListItem { Text = "", Value = "" });
                listadoOption.AddRange((from empleado in resultado.ToList()
                                        select new SelectListItem { Text = empleado.strNombreCompleto, Value = empleado.Identificador.ToString() }).AsEnumerable());
                return PartialView("_SelectOptions", listadoOption);
            }
            else
            {
                return default(PartialViewResult);
            }            
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult ddlrelSedeUsuario(string sedeid)
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var resultado = Nicarao.DAL.Model.BLL.StoreProcedure.ObtenerUsuarioSede(sedeid, UsuarioActual, out Retorno, out Mensaje);            
            List<SelectListItem> listadoOption = new List<SelectListItem>();
            listadoOption.Add(new SelectListItem { Text = "", Value = "" });
            listadoOption.AddRange((from empleado in resultado
                                    select new SelectListItem { Text = empleado.USUARIO_DESCRIPCION, Value = empleado.USUARIO }).AsEnumerable());
            return PartialView("_SelectOptions", listadoOption);
        }

        public ActionResult ObtenerListaUsuarioNic()
        {
            return PartialView("_ListaUsuarioNic");
        }

        public ActionResult ObtenerListaUsuarioOCNic()
        {
            return PartialView("_ListaUsuarioOCNic");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerCargoOrgano(string UsuarioId, string SedeId)
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var resultado = Nicarao.DAL.Model.BLL.StoreProcedure.ObtenerCargoOrganoP(UsuarioId, UsuarioActual, SedeId, out Retorno, out Mensaje);            
            return JsonConvert.SerializeObject(resultado);
            
        }

        public ICollection<Persona> PersonaObtenerPefilAcotaciones(string JsonAcotaciones, string Tipo, string UsuarioId, string EntidadId, string RolId,  out int Retorno, out string Mensaje)
        {
            List<Persona> resultado = new List<Persona>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<Persona> PerfilUsuario = new GenericRepository<Persona>(new GaiaDbContext());
                string sqlCommand = "dbo.spPersonaObtenerPerfilPorAcotaciones @JsonAcotaciones, @Tipo, @UsuarioId, @EntidadId, @RolId, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pAcotaciones = new SqlParameter("@JsonAcotaciones", System.Data.SqlDbType.NVarChar, -1);
                pAcotaciones.Value = JsonAcotaciones;
                parametros.Add(pAcotaciones);
                SqlParameter pTipo = new SqlParameter("@Tipo", System.Data.SqlDbType.VarChar, 100);
                pTipo.Value = Tipo;
                parametros.Add(pTipo);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = UsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pEntidadId = new SqlParameter("@EntidadId", SqlDbType.NVarChar, 30);
                pEntidadId.Value = EntidadId;
                parametros.Add(pEntidadId);
                SqlParameter pRolId = new SqlParameter("@RolId", SqlDbType.NVarChar, 30);
                pRolId.Value = RolId;
                parametros.Add(pRolId);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }                

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerUsuario(string UsuarioId = "")
        {
            if (UsuarioId == "")
            {                
                var listaPersona = BusquedaUsuarioGaia(out Retorno, out Mensaje);                
                var resultado = (from p in listaPersona
                                join u in db.Usuarios on p.Identificador equals u.Identificador
                                where u.TipoIdentificadorId == p.TipoIdentificadorId && u.UsuarioId == p.UsuarioId
                                select new { u.UsuarioId, u.Correo, p.Identificador, p.strNombreCompleto, p.TipoIdentificadorId, p.strCargo}).Distinct();
                return JsonConvert.SerializeObject(resultado);
            }
            else
            {
                var usuario = _USU.SearchFor(d => d.UsuarioId == UsuarioId).Select(c => new { c.UsuarioId, c.EmpleadoId, c.Password, c.Correo, c.Avatar, c.EstadoId, c.Identificador, c.TipoIdentificadorId }).ToList(); //c.CodMunicipio,                               
                return JsonConvert.SerializeObject(usuario);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerUsuarioNicarao(string UsuarioId = "")
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var listaPersona = BusquedaUsuarioNicarao(UsuarioId, UsuarioActual,out Retorno, out Mensaje);            
            return listaPersona;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerUsuarioGaleno(string UsuarioId = "")
        {
            if (UsuarioId == "")
            {
                var listaPersona = BusquedaUsuarioGaleno(out Retorno, out Mensaje);                
                return listaPersona;
            }
            else
            {
                var listaPersona = BusquedaUsuarioGalenoUsuario(UsuarioId, out Retorno, out Mensaje);
                return listaPersona;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerUsuarioSap(int UsuarioId = 0)
        {
            if (UsuarioId == 0)
            {
                var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
                var listaPersona = BusquedaUsuarioSap(UsuarioActual, out Retorno, out Mensaje);
                return listaPersona;
            }
            else
            {
                var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
                var listaPersona = BusquedaUsuarioSapUsuario(UsuarioId, UsuarioActual, out Retorno, out Mensaje);
                return listaPersona;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerPermisoSap(int PermisoId = 0)
        {
            if (PermisoId == 0)
            {
                var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
                var listaPermiso = BusquedaPermisoSap(UsuarioActual, out Retorno, out Mensaje);
                return listaPermiso;
            }
            else
            {
                var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
                var listaPermiso = BusquedaPermisoSapPermisoId(PermisoId, UsuarioActual, out Retorno, out Mensaje);
                return listaPermiso;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObteneRolSap(int id_rol = 0)
        {
            if (id_rol == 0)
            {
                var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
                var listaRol = BusquedaRolSap(UsuarioActual, out Retorno, out Mensaje);
                return listaRol;
            }
            else
            {
                var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
                var listaRol = BusquedaRolSapRolId(id_rol, UsuarioActual, out Retorno, out Mensaje);
                return listaRol;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObteneAccSap(int IdAccion = 0)
        {
            if (IdAccion == 0)
            {
                var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
                var listaAcc = BusquedaAccSap(UsuarioActual, out Retorno, out Mensaje);
                return listaAcc;
            }
            else
            {
                var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
                var listaAcc = BusquedaAccSapAccId(IdAccion, UsuarioActual, out Retorno, out Mensaje);
                return listaAcc;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObteneMenSap(int IdMenu = 0)
        {
            if (IdMenu == 0)
            {                
                var listaMen = BusquedaMenSap(out Retorno, out Mensaje);
                return listaMen;
            }
            else
            {
                var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
                var RolActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().RolId;
                var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
                var listaMen = BusquedaMenSapMenId(IdMenu, UsuarioActual, RolActual, EntidadActual, out Retorno, out Mensaje);
                return listaMen;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerSistema(string SistemaId = "")
        {
            if (SistemaId == "")
            {                
                var Sistema = _Sis.SelectAll().AsEnumerable().Select( c => new {
                    SistemaId = c.SistemaId,
                    OpcionId = c.OpcionId.ToString(),
                    Descripcion= c.Descripcion,
                    LlaveCifrado = c.LlaveCifrado,
                    AlgoritmoCifrado = c.AlgoritmoCifrado,
                    Activo = c.Activo
                }).ToList();
                return JsonConvert.SerializeObject(Sistema);
            }
            else
            {
                var sistema = _Sis.SearchFor(d => d.SistemaId == SistemaId).AsEnumerable().Select(c => new {
                    SistemaId = c.SistemaId,
                    OpcionId = c.OpcionId.ToString(),
                    Descripcion = c.Descripcion,
                    LlaveCifrado = c.LlaveCifrado,
                    AlgoritmoCifrado = c.AlgoritmoCifrado,
                    Activo = c.Activo
                }).ToList();
                return JsonConvert.SerializeObject(sistema);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionRolEntidad(string UsuarioId) 
        {
            var activo = _USU.SelectByID(UsuarioId).UsuarioRolEntidad.Select(ure => new { Llave = ure.RolId + ';' + ure.EntidadId, ure.RolId, ure.Rol.Descripcion, ure.EntidadId, EntidadDescripcion = ure.EntidadG.Descripcion }).ToList();  //.Where(r => r.RolId.Contains(Proyecto.ToString()))           
            return JsonConvert.SerializeObject(activo);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionEntidadMateria(string EntidadId) 
        {
            var activo = _ENG.SelectByID(EntidadId).relEntidadMateria.Select(ure => new { Llave = ure.EntidadId + ';' + ure.MateriaId, ure.EntidadId, ure.catMateria.Descripcion, ure.MateriaId }).ToList();     
            return JsonConvert.SerializeObject(activo);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionRolUsuarioG(string UsuarioId)
        {
            var listaPersona = BusquedaUsuarioRolGaleno(UsuarioId, out Retorno, out Mensaje);
            
            if (listaPersona != null) { return listaPersona; }
            else { return "[]"; }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionRolUsuarioS(string UsuarioId)
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var listaPersona = BusquedaUsuarioRolSap(UsuarioId, UsuarioActual, out Retorno, out Mensaje);

            if (listaPersona != null) { return listaPersona; }
            else { return "[]"; }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionPerAccSap(int IdPermiso)
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var listaPerAcc = BusquedaPermisoAccSap(IdPermiso, UsuarioActual, out Retorno, out Mensaje);

            if (listaPerAcc != null) { return listaPerAcc; }
            else { return "[]"; }
        }

        public ActionResult _RolUsuarioEntidad(string usuarioId = "")
        {
            var usuariorolentidad = _USU.SelectByID(usuarioId).UsuarioRolEntidad.Select(ure => new {ure.EntidadId, ure.RolId}).ToList(); //.Where(r => r.RolId.Contains(Proyecto.ToString())

            var rol = _ROL.SelectAll().ToList();
            var entidad = _ENG.SelectAll().ToList();

            ViewBag.Rol = rol;
            ViewBag.Entidad = entidad;
            return PartialView("_Rol");
        }

        public ActionResult _RolUsuarioEntidadNic(string usuarioId = "")
        {
            var rol = _vwUsuRolNic.SearchFor(c => c.UsuarioId == usuarioId).Select(u => new { LLave = u.relGUID, Descripcion =  u.RolDescripcion + " / " + u.EntidadDescripcion }).ToList();          
            var sedes = _SedeNic.SelectAll().ToList();
            ViewBag.Rol = rol;            
            ViewBag.Sede = sedes;
            return PartialView("_RelUsuNic");
        }

        public ActionResult _RolUsuarioG(string chrcodempleado = "")
        {
            var resultadoRol = ObtenerRolGaleno(chrcodempleado ,out Retorno, out Mensaje);
            ViewBag.RolNo = resultadoRol;
            var resultadoEnt = ObtenerEntImlGaleno(out Retorno, out Mensaje);
            ViewBag.EntIML = resultadoEnt;

            return PartialView("_RolUsuarioG");
        }

        public ActionResult _RolUsuarioS(int idusuario = 0)
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var resultado = ObtenerRolSap(idusuario, UsuarioActual, out Retorno, out Mensaje);
            ViewBag.RolNo = resultado;

            return PartialView("_RolUsuarioS");
        }

        public ActionResult _AccPerSap(int IdPermiso)
        {
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var resultado = ObtenerAccionSap(IdPermiso, UsuarioActual, out Retorno, out Mensaje);
            ViewBag.AccionNo = resultado;

            return PartialView("_AccPerSap");
        }

        [CachingFilter]
        public string GuardarRelUsuarioRolEntidad(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = RelActualizaInserta("Inserta", "seguridad.relUsuarioRolEntidad", "", "", reljson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        //[CachingFilter]
        public string GuardarRelUsuarioRolNic(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = RelActualizaInserta("Inserta", "seguridad.relUsuarioRolUsuarioNic", "", "", reljson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string GuardarRelEntidadMateria(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = RelActualizaInserta("Inserta", "seguridad.relEntidadMateria", "", "", reljson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }
                
        public string GuardarRelUsuarioRolG(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var RolActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().RolId;
            bool resultado = RelInsertaUsuarioRolG(reljson, UsuarioActual, RolActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string GuardarRelUsuarioRolS(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;            
            bool resultado = RelInsertaUsuarioRolS(reljson, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string GuardarRelPermisoAccS(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = RelInsertaPermisoAccS(reljson, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string EliminarUsuarioRolG(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var RolActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().RolId;
            bool resultado = EliminaUsuarioRolG(reljson, UsuarioActual, RolActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string EliminarUsuarioRolS(string reljson, int IdUsuarioRol)        
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = EliminaUsuarioRolS(reljson, IdUsuarioRol, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string EliminarPermisoAccS(string reljson, int IdPermisoAccion)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = EliminaPermisoAccS(reljson, IdPermisoAccion, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string restablecerUsuarioPassword(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;            
            bool resultado = restableceUsuarioPasswordG(reljson, UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public ActionResult _ListaEntidadMat()
        {
            return PartialView();
        }

        public ActionResult _ListaRolR()
        {
            return PartialView();
        }

        public ActionResult _ListaUsuarioNic()
        {
            return PartialView();
        }

        public ActionResult _ListaRolGR()
        {
            return PartialView();
        }

        public ActionResult _ListaRolSR()
        {
            return PartialView();
        }

        public ActionResult _ListaAccPerSap()
        {
            return PartialView();
        }

        public ActionResult _ListaUsuSisR()
        {
            return PartialView();
        }

        public ActionResult _RolMant()
        {
            return PartialView();
        }

        public ActionResult ListaRol()
        {
            return PartialView();
        }

        public ActionResult ObtenerListaRol()
        {
            return PartialView("_ListaRol");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRol(string RolId = "", string Proyecto= "")
        {
            if (RolId == "")
            {                
                var activo = _ROL.SelectAll().OrderBy(x => x.RolId).Where(r => r.RolId.Contains("Rol"+Proyecto.ToString())).Select(c => new { c.RolId, c.Descripcion, c.Activo }).ToList();
                return JsonConvert.SerializeObject(activo);
            }
            else
            {
                var rol = _ROL.SearchFor(d => d.RolId == RolId).Select(c => new { RolId = c.RolId.Replace("Rol"+Proyecto,"") , c.Descripcion, c.Activo }).ToList();
                return JsonConvert.SerializeObject(rol);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionUsuarioEntidad(string RolId, string Proyecto = "")
        {
            var listaPersona = BusquedaUsuarioRolEntidad(RolId, out Retorno, out Mensaje).Distinct();            
            return JsonConvert.SerializeObject(listaPersona);
        }

        public ActionResult _UsuarioRolEntidad(string nombre, string rolId = "", string Proyecto = "")
        {
            //var rolusuario = _ROL.SelectByID(rolId).UsuarioRolEntidad.Select(ure => new {ure.EntidadId, ure.RolId, ure.Usuario.EmpleadoId }).Where(r => r.RolId.Contains(Proyecto.ToString())).ToList();
            // _BUG.SelectAll().Select(a=> new { a.USUARIOID, a.NOMBRE}).ToList();


            //(from p in listaPersona
            //       join u in db.Usuarios on p.Identificador equals u.Identificador
            //       where u.TipoIdentificadorId == p.TipoIdentificadorId
            //       select new { USUARIOID = u.UsuarioId, NOMBRE = p.strNombreCompleto })

            //(from r in (from p in listaPersona
            //                      join u in db.Usuarios on p.Identificador equals u.Identificador
            //                      where u.TipoIdentificadorId == p.TipoIdentificadorId
            //                      select new { USUARIOID = u.UsuarioId, NOMBRE = p.strNombreCompleto })
            //           select r).ToList()
            //           .Distinct();

            //var usuario = from t in todosbug
            //                where !(from cv in rolusuario
            //                        select cv.EmpleadoId)
            //                        .Contains(t.EMPLEADOID)
            //                select t;

            //var entidad = from t in todosent
            //                where !(from cv in rolusuario
            //                        select cv.EntidadId)
            //                        .Contains(t.EntidadId)
            //                select t;            
            // usuario;
            var todosent = _ENG.SelectAll().Select(a => new { a.EntidadId, a.Descripcion }).ToList();
            ViewBag.Entidad = todosent; 
            return PartialView("_Usuario");
        }

        public ActionResult ObtenerListaSirufj()
        {
            return PartialView("_ListaSirufj");
        }

        public ActionResult _FiltraEmpleado()
        {            
            return PartialView();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string FiltrarEmpleados(string nombre)
        {           
            var listaPersona = BusquedaUsuarioGaia(out Retorno, out Mensaje);
            var SearchValues = nombre.ToUpper().Split(" ".ToCharArray());
            var q = PredicateBuilder.True<Persona>();
            foreach (var value in SearchValues)
                q = q.And(x => x.strNombreCompleto.ToUpper().Contains(value.ToUpper()));
            var filtrapersona = _Pe.SelectAll().AsExpandable().Where(q).ToList();
            
            var todos = (from r in (from p in listaPersona
                        join u in filtrapersona on p.Identificador equals u.Identificador
                        where u.TipoIdentificadorId == p.TipoIdentificadorId
                        select new { USUARIOID = p.UsuarioId, NOMBRE = p.strNombreCompleto })
                        select r).ToList()
                       .Distinct();

            var TotalRows = todos.Count();
                if (TotalRows == 0)
                {                    
                    return string.Empty;
                }
                else
                {
                    return JsonConvert.SerializeObject(todos);
                }
        }

        [CachingFilter]
        public string ActualizarRol(string roljson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Actualizacion Realizada con Exito";
                Rol rol = JsonConvert.DeserializeObject<Rol>(roljson);
                _ROL.Update(rol);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string AgregarRol(string roljson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Guardado Realizado con Exito";
                Rol rol = JsonConvert.DeserializeObject<Rol>(roljson);
                _ROL.Insert(rol);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (ex.HResult == -2146233087)
                {
                    Mensaje = "No puede insertar un registro ya existente";
                }
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string eliminaRol(string roljson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Eliminación Realizada con Exito";
                Rol rol = _ROL.SelectByID(roljson);
                _ROL.Delete(rol);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                {Mensaje = "No se puede eliminar este Rol, porque tiene registros en sus tablas Transaccionales o de Relación";}
                else
                {                    
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }               

        public ActionResult _ListaOpcAsoR()
        {
            return PartialView();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionOpcAso(string RolId, string Proyecto = "")
        {
            var activo = _ROL.SelectByID(RolId).RolOpciones.Select(c => new { OpcionId=c.Opcion.OpcionId.ToString(), c.Opcion.Opcion }).ToList();                        
            return JsonConvert.SerializeObject(activo);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionUsuarioOpcAso(string UsuarioId)
        {            
            var activo = _USU.SelectByID(UsuarioId).UsuarioOpciones.Select(d => new { OpcionId = d.OpcionId.ToString(), d.Opcion }).ToList();
            return JsonConvert.SerializeObject(activo);
        }

        public ActionResult _OpcAsoRol(string rolId = "", string proyecto = "")
        {
            var opciones = _OPC.SearchFor(o => o.Opcion == proyecto && o.Nivel == 1).FirstOrDefault();
            var opcasopro = _OPC.SearchFor(oh => oh.OpcionId.IsDescendantOf(opciones.OpcionId) && oh.Nivel > 1).AsEnumerable().Select(c => new {
                OpcionId = c.OpcionId.ToString(),
                Nivel = c.Nivel,
                PadreId = c.PadreId.ToString(),
                Opcion = c.Opcion,
                OpcionTipo = c.OpcionTipo,
                Mapeo = c.Mapeo,
                NombreIcono = c.NombreIcono
            }).ToList();

            var rolopcaso = _ROL.SelectByID(rolId).RolOpciones.Select(c => new
            {
                OpcionId = c.OpcionId.ToString(),
                Opcion = c.Opcion
            }).ToList();
            
            var resultado = (from t in opcasopro
                            where !(from cv in rolopcaso
                                    select cv.OpcionId)
                                    .Contains(t.OpcionId)
                            select t).Select( c => new
                            {
                                OpcionId= c.OpcionId.ToString(),
                                Opcion = c.OpcionId + " " + c.Opcion
                            });
            ViewBag.Opciones = resultado;
            return PartialView("_Opcion");
        }

        public ActionResult _OpcAsoUsuario()
        {
            var proyecto = _OPC.SelectAll().Where(o => o.Nivel == 1).Select(c => new { c.Opcion }).ToList();            
            ViewBag.Proyecto = proyecto;
            return PartialView("_OpcionUsuario");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult ddlrelOpcAso(string usuarioId, string proyecto = "")
        {
            var opciones = _OPC.SearchFor(o => o.Opcion == proyecto && o.Nivel == 1).FirstOrDefault();
            var opcasopro = _OPC.SearchFor(oh => oh.OpcionId.IsDescendantOf(opciones.OpcionId) && oh.Nivel > 1).AsEnumerable().Select(c => new {
                OpcionId = c.OpcionId.ToString(),
                Nivel = c.Nivel,
                PadreId = c.PadreId.ToString(),
                Opcion = c.Opcion,
                OpcionTipo = c.OpcionTipo,
                Mapeo = c.Mapeo,
                NombreIcono = c.NombreIcono
            }).ToList();

            var usuarioopcaso = _USU.SelectByID(usuarioId).UsuarioOpciones.Select(c => new
            {OpcionId = c.OpcionId.ToString(), Opcion = c.Opcion }).ToList();
            var resultado = (from t in opcasopro
                             where !(from cv in usuarioopcaso
                                     select cv.OpcionId)
                                     .Contains(t.OpcionId)
                             select t).Select(c => new
                             {
                                 OpcionId = c.OpcionId.ToString(),
                                 Opcion = c.OpcionId + " " + c.Opcion
                             });

            List<SelectListItem> listadoOption = new List<SelectListItem>();
            listadoOption.Add(new SelectListItem { Text = "", Value = "" });
            listadoOption.AddRange((from est in resultado
                                    select new SelectListItem { Text = est.Opcion, Value = est.OpcionId.ToString() }).AsEnumerable());
            return PartialView("_SelectOptions", listadoOption); ;
        }

        [CachingFilter]
        public string eliminarelOpcAso(string Id, string Id2, string Id3 = "NULL")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";                        
            bool resultado = RelEliminar("seguridad.relUsuarioOpcion", Id, Id2, Id3, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }
                
        public string eliminarelOpcAsoR(string Id, string Id2, string Id3 = "NULL")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            bool resultado = RelEliminar("seguridad.relRolOpcion", Id, Id2, Id3, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string GuardarRelRolOpcion(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = RelActualizaInserta("Inserta", "seguridad.relRolOpcion", "", "", reljson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string GuardarRelUsuarioOpcAso(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = RelActualizaInserta("Inserta", "seguridad.relUsuarioOpcion", "", "", reljson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public ActionResult _ListaEntidadR()
        {
            return PartialView();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionEntidad(string RolId, string Proyecto = "")
        {
            var activo = _ROL.SelectByID(RolId).UsuarioRolEntidad.Select(ure => new { ure.RolId, ure.EntidadId, ure.EntidadG.Descripcion}).Where(r => r.RolId.Contains(Proyecto.ToString())).Distinct().ToList();            
            return JsonConvert.SerializeObject(activo);
        }

        public ActionResult _EntidadRol(string rolId = "", string Proyecto = "")
        {
            var rolopcaso = _ROL.SelectByID(rolId).UsuarioRolEntidad.Select(ure => new {ure.EntidadId, ure.RolId }).Where(r => r.RolId.Contains(Proyecto.ToString())).ToList(); ;
            var todos = _ENG.SelectAll().ToList();
            var resultado = from t in todos
                            where !(from cv in rolopcaso
                                    select cv.EntidadId)
                                    .Contains(t.EntidadId)
                            select t;
            ViewBag.Entidad = resultado;
            return PartialView("_Entidad");            
        }

        [CachingFilter]
        public string GuardarRelRolEntidad(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = RelActualizaInserta("Inserta", "seguridad.relEntidadRol", "", "", reljson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string eliminarelEntidad(string Id, string Id2, string Id3 = "NULL")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";                        
            bool resultado = RelEliminar("seguridad.relEntidadRol", Id, Id2, Id3, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public ActionResult _OpcAsoMant()
        {
            return PartialView();
        }        

        public ActionResult ListaOpcAso()
        {
            return PartialView();
        }               

        public ActionResult ObtenerListaOpcAso()
        {
            return PartialView("_ListaOpcAso");
        }

        [CachingFilter]
        public string ActualizarOpcAso(string Id, string opcionjson)
        {           
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = RelActualizaInserta("Actualiza", "seguridad.catOpcion", "OpcionId", Id, opcionjson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string AgregarOpcAso(string opcionjson)
        {            
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";                       
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            var EntidadActual = SessionHelper.GetItem<Usuario>(session).UsuarioRolEntidad.FirstOrDefault().EntidadId;
            bool resultado = RelActualizaInserta("Inserta", "seguridad.catOpcion", "", "", opcionjson, UsuarioActual, EntidadActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string eliminaOpcAso(string opcionjson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            var UsuarioActual = SessionHelper.GetItem<Usuario>(session).UsuarioId;
            bool resultado = EliminarRegistro("seguridad.catOpcion", "OpcionId", opcionjson, "NULL", UsuarioActual, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerOpcAso(string OpcionId = "", string Proyecto="")
        {
            if (OpcionId == "")
            {
                var opciones = _OPC.SearchFor(o => o.Opcion == Proyecto && o.Nivel==1).FirstOrDefault();

                if (opciones != null)
                {
                    var opcionesHijo = _OPC.SearchFor(oh => oh.OpcionId.IsDescendantOf(opciones.OpcionId) && oh.Nivel > 1).AsEnumerable().Select(c => new
                    {
                        OpcionId = c.OpcionId.ToString(),
                        Nivel = c.Nivel,
                        PadreId = c.PadreId.ToString(),
                        Opcion = c.Opcion,
                        OpcionTipo = c.OpcionTipo,
                        Mapeo = c.Mapeo,
                        NombreIcono = c.NombreIcono,
                        DescripcionGeneral = c.DescripcionGeneral,
                        Activo = c.Activo
                    }).ToList();

                    return JsonConvert.SerializeObject(opcionesHijo);
                }
                return JsonConvert.SerializeObject("");

            }
            else
            {
                var opcion = _OPC.SelectAll().AsEnumerable().Where(x=> x.OpcionId.ToString() == OpcionId).Select(c => new {
                    OpcionId = c.OpcionId.ToString(),
                    Nivel = c.Nivel,
                    PadreId = c.PadreId.ToString(),
                    Opcion = c.Opcion,
                    OpcionTipo = c.OpcionTipo,
                    Mapeo = c.Mapeo,
                    NombreIcono = c.NombreIcono,
                    DescripcionGeneral = c.DescripcionGeneral,
                    Activo = c.Activo
                }).ToList();                
                return JsonConvert.SerializeObject(opcion);
            }
        }

        public ActionResult _EntidadMant()
        {
            return PartialView();
        }

        public ActionResult ListaEntidad()
        {
            ViewBag.Departamentos = _DPR.SelectAll();
            List<SelectListItem> municipios = new List<SelectListItem>();
            municipios.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.Municipios = municipios.AsEnumerable();            
            ViewBag.Sede = _Sede.SelectAll().ToList();
            ViewBag.Entidadpj = _ENT.SelectAll().ToList();
            ViewBag.Edificiopj = _EDI.SelectAll().ToList();
            return PartialView();
        }

        public ActionResult ObtenerListaEntidad()
        {
            return PartialView("_ListaEntidad");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerEntidad(string EntidadId = "", string Proyecto = "")
        {
            if (EntidadId == "")
            {               
                var activo = _ENG.SelectAll().OrderBy(x => x.EntidadId).Select(c => new { c.EntidadId, c.Descripcion, c.CodDepartamento, c.CodMunicipio, c.Domicilio, c.Telefonos, c.Estado, c.Circuito, c.DescripcionCorta, c.SedeJudicial, c.intCodEntidadPJ, c.chrCodEdificio, c.Activo }).ToList();
                var activodescripcion = _db.Entidades.ToList();
                var result = (from x in activo
                             join y in activodescripcion on x.intCodEntidadPJ equals y.intCodEntidadPJ into activoDesc
                              from fd in activoDesc.DefaultIfEmpty()                                  
                              select new
                             {x.EntidadId , x.Descripcion,
                                  strDepartamento = (fd == null) ? "" : fd.strDepartamento,
                                  strMunicipio = (fd == null) ? "" : fd.strMunicipio,
                                  x.Domicilio, x.Telefonos, x.Estado, x.Circuito, x.DescripcionCorta, x.SedeJudicial,
                                  strEntidad = (fd == null) ? "" : fd.strEntidad,
                                  DescripcionEdificio = (fd == null) ? "" : fd.DescripcionEdificio,
                                  x.Activo
                              }).Distinct().ToList();

               
                return JsonConvert.SerializeObject(result);                
            }
            else
            {                
                var entidad = _ENG.SearchFor(x=> x.EntidadId ==EntidadId).OrderBy(x => x.EntidadId).Select(c => new { c.EntidadId, c.Descripcion, c.CodDepartamento, c.CodMunicipio, c.Domicilio, c.Telefonos, c.Estado, c.Circuito, c.DescripcionCorta, c.SedeJudicial, c.intCodEntidadPJ, c.chrCodEdificio, c.Activo }).ToList();
                return JsonConvert.SerializeObject(entidad);
            }
        }

        [CachingFilter]
        public string ActualizarEntidad(string entidadjson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Actualizacion Realizada con Exito";
                EntidadG entidad = JsonConvert.DeserializeObject<EntidadG>(entidadjson);
                _ENG.Update(entidad);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string AgregarEntidad(string entidadjson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Guardado Realizado con Exito";
                EntidadG entidad = JsonConvert.DeserializeObject<EntidadG>(entidadjson);
                _ENG.Insert(entidad);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        [CachingFilter]
        public string eliminaEntidad(string entidadjson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Eliminación Realizada con Exito";
                EntidadG entidad = _ENG.SelectByID(entidadjson);
                _ENG.Delete(entidad);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                {Mensaje = "No se puede eliminar esta Entidad, porque tiene registros en sus tablas Transaccionales o de Relación";}
                else
                {                    
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public ActionResult ListaParametro()
        {
            return PartialView();
        }

        public ActionResult ObtenerListaParametro()
        {
            return PartialView("_ListaParametro");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerParametro(string ParametroId = "")
        {
            if (ParametroId == "")
            {
                var activo = _Par.SelectAll().OrderByDescending(x => x.ParametroId).Select(c => new { c.ParametroId, c.Descripcion, c.Valor }).ToList();
                return JsonConvert.SerializeObject(activo);
            }
            else
            {
                var Parametro = _Par.SearchFor(d => d.ParametroId.ToString() == ParametroId).Select(c => new { c.ParametroId, c.Descripcion, c.Valor }).ToList();
                return JsonConvert.SerializeObject(Parametro);
            }
        }
                
        public string ActualizarParametro(string parametrojson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Actualizacion Realizada con Exito";

                Gaia.DAL.Model.catParametro Parametro = JsonConvert.DeserializeObject<Gaia.DAL.Model.catParametro>(parametrojson);
                _Par.Update(Parametro);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }
                
        public string AgregarParametro(string parametrojson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Guardado Realizado con Exito";

                Gaia.DAL.Model.catParametro Parametro = JsonConvert.DeserializeObject<Gaia.DAL.Model.catParametro>(parametrojson);
                _Par.Insert(Parametro);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public string eliminaParametro(decimal parametrojson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Eliminación Realizada con Exito";
                Gaia.DAL.Model.catParametro Parametro = _Par.SelectByID(parametrojson);
                _Par.Delete(Parametro);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                {
                    Mensaje = "No se puede eliminar este Parametro, porque tiene registros en sus tablas Transaccionales o de Relación";
                }
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public ActionResult ListaReporte()
        {
            ViewBag.Sis = _Sis.SelectAll().ToList();
            return PartialView();
        }

        public ActionResult ObtenerListaReporte()
        {
            return PartialView("_ListaReporte");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerReporte(string ReporteId = "")
        {
            if (ReporteId == "")
            {
                var activo = _Rep.SelectAll().OrderByDescending(x => x.ReporteId).Select(c => new { c.ReporteId, c.Descripcion, c.NombreReporte, c.SistemaId, c.Modulo, c.Activo }).ToList();
                return JsonConvert.SerializeObject(activo);
            }
            else
            {
                var Reporte = _Rep.SearchFor(d => d.ReporteId.ToString() == ReporteId).Select(c => new { c.ReporteId, c.Descripcion, c.NombreReporte, c.SistemaId, c.Modulo, c.Activo }).ToList();
                return JsonConvert.SerializeObject(Reporte);
            }
        }
                
        public string ActualizarReporte(string reportejson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Actualizacion Realizada con Exito";

                Gaia.DAL.Model.catReporte Reporte = JsonConvert.DeserializeObject<Gaia.DAL.Model.catReporte>(reportejson);
                _Rep.Update(Reporte);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }
                
        public string AgregarReporte(string reportejson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Guardado Realizado con Exito";

                Gaia.DAL.Model.catReporte Reporte = JsonConvert.DeserializeObject<Gaia.DAL.Model.catReporte>(reportejson);
                _Rep.Insert(Reporte);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public ActionResult _relReporteParametro(string reporteId = "")
        {
            
            var ParametroReporte = db.relReporteCampo.Where(p => p.ReporteId.Equals(reporteId)).Select(p => p.CampoReporte.catParametro).ToList();
            //var ParametroReporte = db.catParametro.Where(p => p.Reportes.Any(r => r.ReporteId.ToString().Equals(reporteId))).ToList();            
            var todos = _Par.SelectAll().ToList();
            var resultado = from t in todos
                            where !(from cv in ParametroReporte
                                    select cv.ParametroId)
                                    .Contains(t.ParametroId)
                            select t;
            ViewBag.Parametro = resultado;
            return PartialView("_Parametro");
        }

        public ActionResult _relReporteCampo(string reporteId = "")
        {
            var CampoReporte = db.relReporteCampo.Where(p => p.ReporteId.ToString() == reporteId).Select(c => new { c.CampoReporteId}).ToList();             
            var todos = _CamRep.SelectAll().ToList();
            var resultado = from t in todos
                            where !(from cv in CampoReporte
                                    select cv.CampoReporteId)
                                    .Contains(t.CampoReporteId)
                            select t;
            ViewBag.Campo = resultado;
            List<SelectListItem> valoresddl = new List<SelectListItem>();
            valoresddl.Add(new SelectListItem { Text = "", Value = "" });
            valoresddl.Add(new SelectListItem { Text = "Si", Value = "true" });
            valoresddl.Add(new SelectListItem { Text = "No", Value = "false" });
            ViewBag.valoresddl = valoresddl;

            return PartialView("_CampoReporte");
        }

        public ActionResult _relReporteRol(string reporteId = "")
        {
            var CampoReporte = db.relRolReporte.Where(p => p.ReporteId.ToString() == reporteId).Select(c => new { c.RolId }).ToList();
            var todos = _ROL.SelectAll().ToList();
            var resultado = from t in todos
                            where !(from cv in CampoReporte
                                    select cv.RolId)
                                    .Contains(t.RolId)
                            select t;
            ViewBag.Rol = resultado;            

            return PartialView("_RolReporte");
        }

        public string GuardarRelReporteParametro(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";

            var u = SessionHelper.GetItem<Usuario>(session);
            bool resultado = RelCatMarInsertar("dbo.relReporteCampo", "", "", reljson, u.UsuarioId, u.UsuarioRolEntidad.FirstOrDefault().EntidadId, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string GuardarRelReporteCampo(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";

            var u = SessionHelper.GetItem<Usuario>(session);
            bool resultado = RelCatMarInsertar("dbo.relReporteCampo", "", "", reljson, u.UsuarioId, u.UsuarioRolEntidad.FirstOrDefault().EntidadId, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string GuardarRelReporteRol(string reljson)
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";

            var u = SessionHelper.GetItem<Usuario>(session);
            bool resultado = RelCatMarInsertar("dbo.relRolReporte", "", "", reljson, u.UsuarioId, u.UsuarioRolEntidad.FirstOrDefault().EntidadId, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        public string eliminaReporte(decimal reportejson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Eliminación Realizada con Exito";
                Gaia.DAL.Model.catReporte Reporte = _Rep.SelectByID(reportejson);
                _Rep.Delete(Reporte);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                {
                    Mensaje = "No se puede eliminar este Reporte, porque tiene registros en sus tablas Transaccionales o de Relación";
                }
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public ActionResult ListaTipoObjeto()
        {
            return PartialView();
        }

        public ActionResult _TipoObjetoMant()
        {
            return PartialView();
        }

        public ActionResult ObtenerListaTipoObjeto()
        {
            return PartialView("_ListaTipoObjeto");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerTipoObjeto(int TipoObjetoId = 0)
        {
            if (TipoObjetoId == 0)
            {
                var activo = _TipObj.SelectAll().OrderByDescending(x => x.TipoObjetoId).Select(c => new { c.TipoObjetoId, c.Descripcion, c.Activo }).ToList();
                return JsonConvert.SerializeObject(activo);
            }
            else
            {
                var TipoObjeto = _TipObj.SearchFor(d => d.TipoObjetoId == TipoObjetoId).Select(c => new { c.TipoObjetoId, c.Descripcion, c.Activo }).ToList();
                return JsonConvert.SerializeObject(TipoObjeto);
            }
        }

        public string ActualizarTipoObjeto(string tipoobjetojson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Actualizacion Realizada con Exito";

                Gaia.DAL.Model.catTipoObjeto TipoObjeto = JsonConvert.DeserializeObject<Gaia.DAL.Model.catTipoObjeto>(tipoobjetojson);
                _TipObj.Update(TipoObjeto);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }
                
        public string AgregarTipoObjeto(string tipoobjetojson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Guardado Realizado con Exito";

                Gaia.DAL.Model.catTipoObjeto TipoObjeto = JsonConvert.DeserializeObject<Gaia.DAL.Model.catTipoObjeto>(tipoobjetojson);
                _TipObj.Insert(TipoObjeto);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }        

        public string eliminaTipoObjeto(decimal tipoobjetojson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Eliminación Realizada con Exito";
                Gaia.DAL.Model.catTipoObjeto TipoObjeto = _TipObj.SelectByID(tipoobjetojson);
                _TipObj.Delete(TipoObjeto);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                {
                    Mensaje = "No se puede eliminar este TipoObjeto, porque tiene registros en sus tablas Transaccionales o de Relación";
                }
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public ActionResult ListaCampoReporte()
        {
            ViewBag.TipObj = _TipObj.SelectAll().ToList();
            ViewBag.Par = _Par.SelectAll().ToList();
            return PartialView();
        }

        public ActionResult _CampoReporteMant()
        {            
            return PartialView();
        }

        public ActionResult ObtenerListaCampoReporte()
        {
            return PartialView("_ListaCampoReporte");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerCampoReporte(int CampoReporteId = 0)
        {
            if (CampoReporteId == 0)
            {
                var activo = _CamRep.SelectAll().OrderByDescending(x => x.CampoReporteId).Select(c => new { c.CampoReporteId, c.TipoObjetoId, c.Placeholder, c.Titulo, c.ParametroId, c.Activo, c.IsPrincipal }).ToList();

                var resultado = (from p in activo
                                 join u in _TipObj.SelectAll() on p.TipoObjetoId equals u.TipoObjetoId
                                 join t in _Par.SelectAll() on p.ParametroId equals t.ParametroId                                 
                                 select new { p.CampoReporteId, descripciont= u.Descripcion, p.Placeholder, p.Titulo, descripcionp= t.Descripcion, p.Activo, p.IsPrincipal }).Distinct();

                return JsonConvert.SerializeObject(resultado);
            }
            else
            {
                var CampoReporte = _CamRep.SearchFor(d => d.CampoReporteId == CampoReporteId).Select(c => new { c.CampoReporteId, c.TipoObjetoId, c.Placeholder, c.Titulo, c.ParametroId, c.Activo, c.IsPrincipal }).ToList();
                return JsonConvert.SerializeObject(CampoReporte);
            }
        }

        public string ActualizarCampoReporte(string camporeportejson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Actualizacion Realizada con Exito";

                Gaia.DAL.Model.CampoReporte CampoReporte = JsonConvert.DeserializeObject<Gaia.DAL.Model.CampoReporte>(camporeportejson);
                _CamRep.Update(CampoReporte);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public string AgregarCampoReporte(string camporeportejson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Guardado Realizado con Exito";

                Gaia.DAL.Model.CampoReporte CampoReporte = JsonConvert.DeserializeObject<Gaia.DAL.Model.CampoReporte>(camporeportejson);
                _CamRep.Insert(CampoReporte);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public string eliminaCampoReporte(decimal camporeportejson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Eliminación Realizada con Exito";
                Gaia.DAL.Model.CampoReporte CampoReporte = _CamRep.SelectByID(camporeportejson);
                _CamRep.Delete(CampoReporte);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                {
                    Mensaje = "No se puede eliminar este CampoReporte, porque tiene registros en sus tablas Transaccionales o de Relación";
                }
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public ActionResult _ListaParametroR()
        {
            return PartialView();
        }

        public ActionResult _ListaCampoR()
        {
            return PartialView();
        }

        public ActionResult _ListaRolRep()
        {
            return PartialView();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionRepPar(int reporteId)
        {
            var Parametro = db.relReporteCampo.Where(p => p.ReporteId.Equals(reporteId)).Select(p => p.CampoReporte.catParametro).ToList();
            //var Parametro = db.catParametro.Where(p => p.Reportes.Any(r => r.ReporteId.Equals(reporteId))).Select(t=> new { t.Descripcion, t.ParametroId }).ToList();         
            return JsonConvert.SerializeObject(Parametro);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionRepCam(int reporteId)
        {
            var CampoReporte = db.relReporteCampo.Where(p => p.ReporteId == reporteId).Select(c => new { c.CampoReporteId, c.Orden, c.IsPrincipal }).ToList();
            var todos = _CamRep.SelectAll().ToList();
            var resultado = from t in todos
                            join s in CampoReporte on t.CampoReporteId equals s.CampoReporteId                            
                            select new {t.CampoReporteId, t.Placeholder, t.Titulo, s.Orden, s.IsPrincipal } ;            
            return JsonConvert.SerializeObject(resultado);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerRelacionRepRol(int reporteId)
        {
            var CampoReporte = db.relRolReporte.Where(p => p.ReporteId == reporteId).Select(c => new { c.RolId }).ToList();            
            return JsonConvert.SerializeObject(CampoReporte);
        }

        public ActionResult ObtenerListaVariableControl()
        {
            return PartialView("_ListaVariableControl");
        }

        public ActionResult ListaVariableControl()
        {
            return PartialView();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerVariableControl(string VariableControlId)
        {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "dd/MM/yyy hh:mm tt";
            if (String.IsNullOrWhiteSpace(VariableControlId))
            {
                var activo = _VC.SelectAll().OrderByDescending(x => x.VariableControlId).Select(c => new { c.VariableControlId, c.Descripcion, c.Cadena, c.Fecha, c.Numerico, c.TablaRel, c.CampoRel, c.Activo }).ToList();
                return JsonConvert.SerializeObject(activo);
            }
            else
            {
                var variablecontrol = _VC.SearchFor(d => d.VariableControlId == VariableControlId).Select(c => new { c.VariableControlId, c.Descripcion, c.Cadena, c.Fecha, c.Numerico, c.TablaRel, c.CampoRel, c.Activo }).ToList();
                return JsonConvert.SerializeObject(variablecontrol, jsonSettings);                
            }
        }

        public ActionResult _VariableControlMant()
        {
            return PartialView();
        }
                
        public string ActualizarVariableControl(string variablecontroljson, string VariableId)
        {
            try
            {               
                int Retorno = -1;
                string Mensaje = "Ocurrio un error no controlado";

                var u = SessionHelper.GetItem<Usuario>(session);
                bool resultado = RelActualizaInserta("Actualiza", "dbo.VariableControl", "VariableControlId", VariableId, variablecontroljson, u.UsuarioId, u.UsuarioRolEntidad.FirstOrDefault().EntidadId, out Retorno, out Mensaje);
                var resulta = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
                return resulta;
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }
                
        public string AgregarVariableControl(string variablecontroljson)
        {
            try
            {               
                int Retorno = -1;
                string Mensaje = "Ocurrio un error no controlado";

                var u = SessionHelper.GetItem<Usuario>(session);                
                bool resultado = RelActualizaInserta("Inserta", "dbo.VariableControl", "", "", variablecontroljson, u.UsuarioId, u.UsuarioRolEntidad.FirstOrDefault().EntidadId, out Retorno, out Mensaje);
                var resulta = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
                return resulta;
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }
                
        public string eliminaVariableControl(string variablecontroljson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Eliminación Realizada con Exito";
                Gaia.DAL.Model.GaiaVariableControl variablecontrol = _VC.SelectByID(variablecontroljson);
                _VC.Delete(variablecontrol);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                {
                    Mensaje = "No se puede eliminar esta Variable de Control, porque tiene registros en sus tablas Transaccionales o de Relación";
                }
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public ActionResult ObtenerListaMateria()
        {
            return PartialView("_ListaMateria");
        }

        public ActionResult ListaMateria()
        {
            return PartialView();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string ObtenerMateria(string MateriaId)
        {
            if (String.IsNullOrWhiteSpace(MateriaId))
            {
                var activo = _Mat.SelectAll().OrderByDescending(x => x.MateriaId).Select(c => new { c.MateriaId, c.Descripcion }).ToList();
                return JsonConvert.SerializeObject(activo);
            }
            else
            {
                var materia = _Mat.SearchFor(d => d.MateriaId == MateriaId).Select(c => new { c.MateriaId, c.Descripcion }).ToList();
                return JsonConvert.SerializeObject(materia);
            }
        }

        public ActionResult _MateriaMant()
        {
            return PartialView();
        }
                
        public string ActualizarMateria(string materiajson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Actualizacion Realizada con Exito";

                Gaia.DAL.Model.catMateriaG materia = JsonConvert.DeserializeObject<Gaia.DAL.Model.catMateriaG>(materiajson);
                _Mat.Update(materia);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }
                
        public string AgregarMateria(string materiajson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Guardado Realizado con Exito";

                Gaia.DAL.Model.catMateriaG materia = JsonConvert.DeserializeObject<Gaia.DAL.Model.catMateriaG>(materiajson);
                _Mat.Insert(materia);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }
                
        public string eliminaMateria(string materiajson)
        {
            try
            {
                Retorno = 0;
                Mensaje = "Eliminación Realizada con Exito";
                Gaia.DAL.Model.catMateriaG materia = _Mat.SelectByID(materiajson);
                _Mat.Delete(materia);
            }

            catch (Exception ex)
            {
                Retorno = ex.HResult;
                if (Retorno == -2146233087)
                {
                    Mensaje = "No se puede eliminar esta Materia, porque tiene registros en sus tablas Transaccionales o de Relación";
                }
                else
                {
                    Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                    Mensaje = Mensaje.Replace("\"", "");
                    Mensaje = Mensaje.Replace("\r", "");
                    Mensaje = Mensaje.Replace("\n", "");
                }
                resultado = false;
            }

            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));

            return result;
        }

        public string eliminaRelacion(string tabla, string Id, string Id2, string Id3 = "NULL")
        {
            int Retorno = -1;
            string Mensaje = "Ocurrio un error no controlado";
            bool resultado = RelEliminar(tabla, Id, Id2, Id3, out Retorno, out Mensaje);
            var result = jsonRetorno(Retorno, Mensaje, resultado, (Retorno == 0 ? ("\"" + "EXITO" + "\"") : ("\"" + "NINGUNO" + "\"")));
            return result;
        }

        private bool RelCatMarInsertar(string StringTabla, string StringCampo, string StringId, string reljson, string StringUsuarioId, string StringEntidadId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                string sqlCommand = "EXECUTE [spInsertar] @Tabla, @CampoId, @Id, @Json, @UsuarioAccion, @EntidadUsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();

                SqlParameter pTabla = new SqlParameter("@Tabla", System.Data.SqlDbType.NVarChar, 100);
                pTabla.Value = StringTabla;
                parametros.Add(pTabla);

                SqlParameter pCampo = new SqlParameter("@CampoId", System.Data.SqlDbType.NVarChar, 100);
                pCampo.Value = StringCampo;
                parametros.Add(pCampo);

                SqlParameter pId = new SqlParameter("@Id", System.Data.SqlDbType.NVarChar, 20);
                pId.Value = StringId;
                parametros.Add(pId);

                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);

                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioAccion", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);

                SqlParameter pEntidadId = new SqlParameter("@EntidadUsuarioId", SqlDbType.NVarChar, 9);
                pEntidadId.Value = StringEntidadId;
                parametros.Add(pEntidadId);

                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);

                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);

                SqlParameter[] oParametros = parametros.ToArray();

                db.Database.ExecuteSqlCommand(sqlCommand, oParametros);

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;                
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return false;
            }
        }

        private bool RelEliminar(string StringTabla, string StringId, string StringId2, string  StringId3, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand = "EXECUTE [dbo].[spEliminarRelacion] @Tabla, @Id, @Id2, @Id3, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pTabla = new SqlParameter("@Tabla", System.Data.SqlDbType.NVarChar, 100);
                pTabla.Value = StringTabla;
                parametros.Add(pTabla);                               
                SqlParameter pId = new SqlParameter("@Id", System.Data.SqlDbType.NVarChar, 100);
                pId.Value = StringId;
                parametros.Add(pId);
                SqlParameter pId2 = new SqlParameter("@Id2", System.Data.SqlDbType.NVarChar, 100);
                pId2.Value = StringId2;
                parametros.Add(pId2);
                SqlParameter pId3 = new SqlParameter("@Id3", System.Data.SqlDbType.NVarChar, 100);
                pId3.Value = StringId3;
                parametros.Add(pId3);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                db.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return false;
            }
        }

        private bool EliminarRegistro(string StringTabla, string StringCampo, string StringId, string StringOpcion, string StringUsuario, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand = "EXECUTE [dbo].[spEliminar] @Tabla, @Campo, @Id, @Opcion, @UsuarioAccion, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pTabla = new SqlParameter("@Tabla", System.Data.SqlDbType.NVarChar, 100);
                pTabla.Value = StringTabla;
                parametros.Add(pTabla);
                SqlParameter pCampo = new SqlParameter("@Campo", System.Data.SqlDbType.NVarChar, 100);
                pCampo.Value = StringCampo;
                parametros.Add(pCampo);
                SqlParameter pId = new SqlParameter("@Id", System.Data.SqlDbType.NVarChar, 20);
                pId.Value = StringId;
                parametros.Add(pId);
                SqlParameter pOpcion = new SqlParameter("@Opcion", System.Data.SqlDbType.NVarChar, 10);
                pOpcion.Value = StringOpcion;
                parametros.Add(pOpcion);
                SqlParameter pUsuario = new SqlParameter("@UsuarioAccion", System.Data.SqlDbType.NVarChar, 30);
                pUsuario.Value = StringUsuario;
                parametros.Add(pUsuario);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                db.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return false;
            }
        }

        private bool RelActualizaInserta(string Origen, string StringTabla, string StringCampo, string StringId, string reljson, string StringUsuarioId, string StringEntidadId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;
                if (Origen == "Inserta")
                {sqlCommand = "EXECUTE [dbo].[spInsertar] @Tabla, @CampoId, @Id, @Json, @UsuarioId, @EntidadUsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT"; }
                else {sqlCommand = "EXECUTE [dbo].[spActualizar] @Tabla, @CampoId, @Id, @Json, @Retorno OUTPUT, @Mensaje OUTPUT";}                
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pTabla = new SqlParameter("@Tabla", System.Data.SqlDbType.NVarChar, 100);
                pTabla.Value = StringTabla;
                parametros.Add(pTabla);
                SqlParameter pCampo = new SqlParameter("@CampoId", System.Data.SqlDbType.NVarChar, 100);
                pCampo.Value = StringCampo;
                parametros.Add(pCampo);
                SqlParameter pId = new SqlParameter("@Id", System.Data.SqlDbType.NVarChar, 20);
                pId.Value = StringId;
                parametros.Add(pId);
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                if (Origen == "Inserta")
                {
                    SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                    pUsuarioAccion.Value = StringUsuarioId;
                    parametros.Add(pUsuarioAccion);
                    SqlParameter pEntidadId = new SqlParameter("@EntidadUsuarioId", SqlDbType.NVarChar, 9);
                    pEntidadId.Value = StringEntidadId;
                    parametros.Add(pEntidadId);
                }    
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                db.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                
                return false;
            }
        }

        private bool RelInsertaUsuarioRolG(string reljson, string StringUsuarioId, string StringRolId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;
                
                sqlCommand = "EXECUTE [dbo].[spUsuarioRolIngresar] @Json, @UsuarioId, @RolId, @Retorno OUTPUT, @Mensaje OUTPUT"; 
                
                List<SqlParameter> parametros = new List<SqlParameter>();                
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pEntidadId = new SqlParameter("@RolId", SqlDbType.NVarChar, 30);
                pEntidadId.Value = StringRolId;
                parametros.Add(pEntidadId);                
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db2.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool RelInsertaUsuarioRolS(string reljson, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;

                sqlCommand = "EXECUTE [seguridad].[spIngresarRelacUsuarioRol] @Json, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);               
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool RelInsertaPermisoAccS(string reljson, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;

                sqlCommand = "EXECUTE [seguridad].[spInsertrel_Permisos_Opciones] @Json, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool EliminaUsuarioRolG(string reljson, string StringUsuarioId, string StringRolId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;

                sqlCommand = "EXECUTE [dbo].[spUsuarioRolEliminar] @Json, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);                
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db2.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool EliminaUsuarioRolS(string reljson, int id_usuario_rol, string UsuarioActual, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;

                sqlCommand = "EXECUTE [seguridad].[spEliminaRelacUsuarioRol] @Json, @Id_Usuario_Rol, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuario = new SqlParameter("@Id_Usuario_Rol", SqlDbType.Int);
                pUsuario.Value = id_usuario_rol;
                parametros.Add(pUsuario);                
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = UsuarioActual;
                parametros.Add(pUsuarioAccion);                               
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool EliminaPermisoAccS(string reljson, int StringIPermisoAcc, string UsuarioActual, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;

                sqlCommand = "EXECUTE [seguridad].[spActualizarel_Permisos_Opciones] @Json, @Id_PermisoAccion, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@Json", System.Data.SqlDbType.NVarChar, -1);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuario = new SqlParameter("@Id_PermisoAccion", SqlDbType.Int);
                pUsuario.Value = StringIPermisoAcc;
                parametros.Add(pUsuario);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = UsuarioActual;
                parametros.Add(pUsuarioAccion);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db3.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool restableceUsuarioPasswordG(string reljson, string StringUsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;

                sqlCommand = "EXECUTE [dbo].[spEmpleadoUsuarioContrasenaReset] @chrCodEmpleado, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";

                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pJson = new SqlParameter("@chrCodEmpleado", System.Data.SqlDbType.VarChar, 1000);
                pJson.Value = reljson;
                parametros.Add(pJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);                
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                _db2.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }

        private bool InsertaSistemaOpcion(string sisjson, string opcjson, string StringUsuarioId, string StringEntidadId, string StringCargoId, out int Retorno, out string Mensaje)
        {
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                string sqlCommand;
                
                sqlCommand = "EXECUTE [seguridad].[spCatSistemaOpcionInsertar] @jsonSistema, @jsonOpcion, @UsuarioId, @EntidadUsuarioId, @CargoId, @Retorno OUTPUT, @Mensaje OUTPUT";
                
                List<SqlParameter> parametros = new List<SqlParameter>();                
                SqlParameter psisJson = new SqlParameter("@jsonSistema", System.Data.SqlDbType.NVarChar, -1);
                psisJson.Value = sisjson;
                parametros.Add(psisJson);                
                SqlParameter popcJson = new SqlParameter("@jsonOpcion", System.Data.SqlDbType.NVarChar, -1);
                popcJson.Value = opcjson;
                parametros.Add(popcJson);
                SqlParameter pUsuarioAccion = new SqlParameter("@UsuarioId", SqlDbType.NVarChar, 30);
                pUsuarioAccion.Value = StringUsuarioId;
                parametros.Add(pUsuarioAccion);
                SqlParameter pEntidadId = new SqlParameter("@EntidadUsuarioId", SqlDbType.NVarChar, 9);
                pEntidadId.Value = StringEntidadId;
                parametros.Add(pEntidadId);
                SqlParameter pCargoId = new SqlParameter("@CargoId", SqlDbType.NVarChar, 9);
                pCargoId.Value = StringCargoId;
                parametros.Add(pCargoId);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                db.Database.ExecuteSqlCommand(sqlCommand, oParametros);
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                if (Retorno == 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");

                return false;
            }
        }        

        public ICollection<Persona> PersonaObtenerPefil(string Nombre, string Tipo, out int Retorno, out string Mensaje)
        {
            List<Persona> resultado = new List<Persona>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<Persona> PerfilUsuario = new GenericRepository<Persona>(new GaiaDbContext());
                string sqlCommand = "dbo.spPersonaObtenerPerfilPorNombre @Nombre, @Tipo, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();                                
                SqlParameter pNombre = new SqlParameter("@Nombre", System.Data.SqlDbType.VarChar, 50);
                pNombre.Value = Nombre;
                parametros.Add(pNombre);
                SqlParameter pTipo = new SqlParameter("@Tipo", System.Data.SqlDbType.VarChar, 100);
                pTipo.Value = Tipo;
                parametros.Add(pTipo);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);                
                SqlParameter[] oParametros = parametros.ToArray();                                
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public string BusquedaUsuarioGaleno(out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(GALENODbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                                
                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();                

                var result = ccnn.ExecuteStoreProcedure("EXECUTE dbo.spUsuarioObtenerListado @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty; 
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaUsuarioSap(string UsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pNombre = new SqlParameter("@UsuarioId", System.Data.SqlDbType.VarChar, 30);
                pNombre.Value = UsuarioId;
                Parametros.Add(pNombre);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListadoUsuarios @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaPermisoSap(string PermisoId, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pNombre = new SqlParameter("@UsuarioId", System.Data.SqlDbType.VarChar, 30);
                pNombre.Value = PermisoId;
                Parametros.Add(pNombre);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListPermTbl_permTbl_rolcat_menu @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaRolSap(string UsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pNombre = new SqlParameter("@UsuarioId", System.Data.SqlDbType.VarChar, 30);
                pNombre.Value = UsuarioId;
                Parametros.Add(pNombre);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListeInfoTbl_rol @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaAccSap(string UsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pNombre = new SqlParameter("@UsuarioId", System.Data.SqlDbType.VarChar, 30);
                pNombre.Value = UsuarioId;
                Parametros.Add(pNombre);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListacatAcciones_catTipoAccion @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaMenSap(out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();                
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListe_CatMenu_CatModulos @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaUsuarioRolGaleno(string UsuarioId, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(GALENODbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@chrCodEmpleado", System.Data.SqlDbType.VarChar, 1000);
                pEmpleado.Value = UsuarioId;
                Parametros.Add(pEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE dbo.spRolesUsuarioObtener @chrCodEmpleado, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaUsuarioRolSap(string UsuarioId, string usuarioActual, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@Id_usuario", System.Data.SqlDbType.Int);
                pEmpleado.Value = UsuarioId;
                Parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.VarChar, 30);
                ppEmpleado.Value = usuarioActual;
                Parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListeRolUsuario @Id_usuario, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaPermisoAccSap(int IPermisod, string usuarioActual, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@id_permiso", System.Data.SqlDbType.Int);
                pEmpleado.Value = IPermisod;
                Parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.VarChar, 30);
                ppEmpleado.Value = usuarioActual;
                Parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListerel_permisos_Opciones_catAcciones @id_permiso, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaUsuarioGalenoUsuario(string chrcodempleado, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(GALENODbContext));
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@chrCodEmpleado", System.Data.SqlDbType.VarChar, 1000);
                pEmpleado.Value = chrcodempleado;
                Parametros.Add(pEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE dbo.spUsuarioObtener @chrCodEmpleado, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaUsuarioSapUsuario(int idusuario, string UsuarioActual, out int Retorno, out string Mensaje)
        {
            try
            {                
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));                
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@id_usuario", System.Data.SqlDbType.Int);
                pEmpleado.Value = idusuario;
                Parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleado.Value = UsuarioActual;
                Parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListeDatosUsuario @id_usuario, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaPermisoSapPermisoId(int idpermiso, string UsuarioActual, out int Retorno, out string Mensaje)
        {
            try
            {                
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));
             
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@Id_permiso", System.Data.SqlDbType.Int);
                pEmpleado.Value = idpermiso;
                Parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleado.Value = UsuarioActual;
                Parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListPermTbl_permiso @Id_permiso, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaRolSapRolId(int id_rol, string UsuarioActual, out int Retorno, out string Mensaje)
        {
            try
            {                
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));
                
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@id_rol", System.Data.SqlDbType.Int);
                pEmpleado.Value = id_rol;
                Parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleado.Value = UsuarioActual;
                Parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();                

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListeInfoid_rol_tabla_rol @id_rol, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaAccSapAccId(int idaccion, string UsuarioActual, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));

                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@IdAccion", System.Data.SqlDbType.Int);
                pEmpleado.Value = idaccion;
                Parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleado.Value = UsuarioActual;
                Parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.spListacatAcciones @IdAccion, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public string BusquedaMenSapMenId(int idmenu, string UsuarioActual, string RolActual, string EntidadActual, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(SAPDbContext));

                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@id_menu", System.Data.SqlDbType.Int);
                pEmpleado.Value = idmenu;
                Parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleado.Value = UsuarioActual;
                Parametros.Add(ppEmpleado);
                SqlParameter ppEmpleador = new SqlParameter("@RolId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleador.Value = RolActual;
                Parametros.Add(ppEmpleador);
                SqlParameter ppEmpleadoe = new SqlParameter("@EntidadId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleadoe.Value = EntidadActual;
                Parametros.Add(ppEmpleadoe);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE dbo.spcat_menuObtenerDatos @id_menu, @UsuarioId, @RolId, @EntidadId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public ICollection<EntidadesGaleno> ObtenerEntidadesGaleno(out int Retorno, out string Mensaje)
        {
            List<EntidadesGaleno> resultado = new List<EntidadesGaleno>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<EntidadesGaleno> PerfilUsuario = new GenericRepository<EntidadesGaleno>(new GALENODbContext());
                string sqlCommand = "spUsuarioObtenerDelegacionIML @intValorRetorno OUTPUT, @strMensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pRetorno = new SqlParameter("@intValorRetorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@strMensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;               
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public ICollection<tablaRol> ObtenerRolesSap(string Usuario, out int Retorno, out string Mensaje)
        {
            List<tablaRol> resultado = new List<tablaRol>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<tablaRol> PerfilUsuario = new GenericRepository<tablaRol>(new SAPDbContext());
                string sqlCommand = "seguridad.spListPermTbl_permiso_Select @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleado.Value = Usuario;
                parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public ICollection<catMenu> ObtenerMenuSap(string Usuario, out int Retorno, out string Mensaje)
        {
            List<catMenu> resultado = new List<catMenu>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<catMenu> PerfilUsuario = new GenericRepository<catMenu>(new SAPDbContext());
                string sqlCommand = "seguridad.spListPermcat_menu @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleado.Value = Usuario;
                parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public ICollection<catTipoAccion> ObtenerTipoAccionSap(string Usuario, out int Retorno, out string Mensaje)
        {
            List<catTipoAccion> resultado = new List<catTipoAccion>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<catTipoAccion> PerfilUsuario = new GenericRepository<catTipoAccion>(new SAPDbContext());
                string sqlCommand = "seguridad.spListacattipoAccionSelect @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleado.Value = Usuario;
                parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public ICollection<catModulos> ObtenerModuloSap(string Usuario)
        {
            List<catModulos> resultado = new List<catModulos>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<catModulos> PerfilUsuario = new GenericRepository<catModulos>(new SAPDbContext());
                string sqlCommand = "dbo.spcatModuloslista @UsuarioId";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleado.Value = Usuario;
                parametros.Add(ppEmpleado);                
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
               
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public ICollection<RolG> ObtenerRolGaleno(string chrCodEmpleado, out int Retorno, out string Mensaje)
        {
            List<RolG> resultado = new List<RolG>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<RolG> PerfilUsuario = new GenericRepository<RolG>(new GALENODbContext());
                string sqlCommand = "spUsuarioRolNoasignado @chrCodEmpleado, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();                
                SqlParameter pEmpleado = new SqlParameter("@chrCodEmpleado", System.Data.SqlDbType.VarChar, 24);
                pEmpleado.Value = chrCodEmpleado;
                parametros.Add(pEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public ICollection<catEntidadIML> ObtenerEntImlGaleno(out int Retorno, out string Mensaje)
        {
            List<catEntidadIML> resultado = new List<catEntidadIML>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<catEntidadIML> PerfilUsuario = new GenericRepository<catEntidadIML>(new GALENODbContext());
                string sqlCommand = "spUsuarioObtenerEntidadesIML @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();                
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public ICollection<RolS> ObtenerRolSap(int idusuario, string usuarioactual, out int Retorno, out string Mensaje)
        {
            List<RolS> resultado = new List<RolS>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<RolS> PerfilUsuario = new GenericRepository<RolS>(new SAPDbContext());
                string sqlCommand = "seguridad.spListaRolesNoAsignadosUsuario @Id_usuario, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@Id_usuario", System.Data.SqlDbType.Int);
                pEmpleado.Value = idusuario;
                parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.VarChar, 30);
                ppEmpleado.Value = usuarioactual;
                parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public ICollection<catAccion> ObtenerAccionSap(int idpermiso, string usuarioactual, out int Retorno, out string Mensaje)
        {
            List<catAccion> resultado = new List<catAccion>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<catAccion> PerfilUsuario = new GenericRepository<catAccion>(new SAPDbContext());
                string sqlCommand = "seguridad.spListaIdAccionNoasignadopermiso @id_permiso, @UsuarioId, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@id_permiso", System.Data.SqlDbType.Int);
                pEmpleado.Value = idpermiso;
                parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.VarChar, 30);
                ppEmpleado.Value = usuarioactual;
                parametros.Add(ppEmpleado);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public ICollection<Persona> BusquedaUsuarioGaia(out int Retorno, out string Mensaje)
        {
            List<Persona> resultado = new List<Persona>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<Persona> PerfilUsuario = new GenericRepository<Persona>(new GaiaDbContext());
                string sqlCommand = "spBusquedaUsuarioGaia @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();                
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();                
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;                
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        public string BusquedaUsuarioNicarao(string usuarioid, string UsuarioActual, out int Retorno, out string Mensaje)
        {
            try
            {
                GenericRepository<string> ccnn = new GenericRepository<string>(typeof(GaiaDbContext));

                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";

                List<SqlParameter> Parametros = new List<SqlParameter>();
                SqlParameter pEmpleado = new SqlParameter("@UsuarioId", System.Data.SqlDbType.NVarChar, 30);
                pEmpleado.Value = usuarioid;
                Parametros.Add(pEmpleado);
                SqlParameter ppEmpleado = new SqlParameter("@SistemaId", System.Data.SqlDbType.NVarChar, 255);
                ppEmpleado.Value = "GAIA";
                Parametros.Add(ppEmpleado);
                SqlParameter ppEmpleador = new SqlParameter("@EntidadId", System.Data.SqlDbType.NVarChar, 30);
                ppEmpleador.Value = UsuarioActual;
                Parametros.Add(ppEmpleador);
                SqlParameter ppEmpleadoe = new SqlParameter("@RolId", System.Data.SqlDbType.NVarChar, -1);
                ppEmpleadoe.Value = UsuarioActual;
                Parametros.Add(ppEmpleadoe);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                Parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                Parametros.Add(pMensaje);

                object[] oParametros = Parametros.ToArray();

                var result = ccnn.ExecuteStoreProcedure("EXECUTE seguridad.relUsuarioRolUsuarioNicObtener @UsuarioId, @SistemaId, @EntidadId, @RolId, @Retorno OUTPUT, @Mensaje OUTPUT", oParametros).FirstOrDefault();

                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);

                if (Retorno == 0) return result;
                else return string.Empty;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return "[]";
            }
        }

        public ICollection<Persona> BusquedaUsuarioRolEntidad(string RolId,out int Retorno, out string Mensaje)
        {
            List<Persona> resultado = new List<Persona>();
            try
            {
                Retorno = -1;
                Mensaje = "Ocurrio un error no controlado";
                GenericRepository<Persona> PerfilUsuario = new GenericRepository<Persona>(new GaiaDbContext());
                string sqlCommand = "spBusquedaUsuarioRolEntidad @RolId, @Retorno OUTPUT, @Mensaje OUTPUT";
                List<SqlParameter> parametros = new List<SqlParameter>();
                SqlParameter pRolId = new SqlParameter("@RolId", System.Data.SqlDbType.VarChar, 30);
                pRolId.Value = RolId;
                parametros.Add(pRolId);
                SqlParameter pRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
                pRetorno.Direction = ParameterDirection.Output;
                parametros.Add(pRetorno);
                SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 1024);
                pMensaje.Direction = ParameterDirection.Output;
                parametros.Add(pMensaje);
                SqlParameter[] oParametros = parametros.ToArray();
                resultado = PerfilUsuario.ExecStoreProcedure(sqlCommand, oParametros).ToList();
                Retorno = Convert.ToInt32(pRetorno.Value);
                Mensaje = Convert.ToString(pMensaje.Value);
                return resultado;
            }
            catch (Exception ex)
            {
                Retorno = ex.HResult;
                Mensaje = ex.Message.ToString().Replace(".\r\n", " ");
                Mensaje = Mensaje.Replace("\"", "");
                Mensaje = Mensaje.Replace("\r", "");
                Mensaje = Mensaje.Replace("\n", "");
                return resultado;
            }
        }

        private string jsonRetorno(int Retorno, string Mensaje, bool Resultado, string Valor = "NINGUNO")
        {
            var jsonMensaje = "{" + "\"" + "Mensaje" + "\"" + ": " + "\"" + Mensaje + "\"" + ", " + "\"" + "Retorno" + "\"" + ": " + "\"" + Retorno.ToString() + "\"" + ", " + "\"" + "Resultado" + "\"" + ": " + "\"" + Resultado.ToString().ToLower() + "\"" + ", " + "\"" + "Valor" + "\"" + ": " + Valor + "}";
            return jsonMensaje;
        }        

        public ActionResult GetAvatars(string UsuarioId) 
        {            
            Usuario u = _USU.SelectByID(UsuarioId);            
            string imageBase64Data = Convert.ToBase64String(u.Avatar);            
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            ViewBag.ImageData = imageDataURL;
            return PartialView("Avatar");
        }

        public bool ValidarOrdenCampo(decimal ReporteId, int Orden)
        {
            var CampoReporte = db.relReporteCampo.Where(p => p.ReporteId == ReporteId && p.Orden== Orden).Count();
            if (CampoReporte == 0) { return true; }
            else { return false; }
        }

        public ActionResult GetAvatarsSap(int UsuarioId)
        {            
            tbl_usuario t = _TblUsuario.SelectByID(UsuarioId);         
            string imageBase64Data = Convert.ToBase64String(t.Avatar);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            ViewBag.ImageData = imageDataURL;
            return PartialView("Avatar");
        }

        public ActionResult Avatar()
        {
            return PartialView();
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}