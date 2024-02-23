using System;
using LinqKit;
using System.Web;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Microsoft.SqlServer;
using PJN.DAL;
using Gaia.DAL;
using Gaia.DAL.Model;
using Gaia.BLL.Repository;
using Gaia.Seguridad.Controllers;
using Gaia.Seguridad.Filters;

namespace Gaia_App.Controllers
{
    public class AuditoriaController : Controller
    {
        int Retorno;
        string Mensaje;

        GaiaDbContext db = new GaiaDbContext();
        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
        GenericRepository<Gaia.DAL.Model.Auditoria> _Auditoria = new GenericRepository<Gaia.DAL.Model.Auditoria>(new GaiaDbContext());
        GenericRepository<Gaia.DAL.Model.Sistema> _Sistema = new GenericRepository<Gaia.DAL.Model.Sistema>(new GaiaDbContext());
        GenericRepository<Gaia.DAL.Model.vwAuditoria> _vwAuditoria = new GenericRepository<Gaia.DAL.Model.vwAuditoria>(new GaiaDbContext());
        GenericRepository<Gaia.DAL.Model.EntidadG> _Entidad = new GenericRepository<Gaia.DAL.Model.EntidadG>(new GaiaDbContext());
        GenericRepository<Persona> _Pe = new GenericRepository<Persona>(new GaiaDbContext());

        // GET: Auditoria
        public ActionResult Index()
        {
            return View();
        }

        //[CachingFilter]
        public ActionResult ListaAuditoria()
        {
            ViewBag.Sistema = _Sistema.SelectAll();
            ViewBag.Entidad = _Entidad.SelectAll();
            return PartialView();
        }

        public ActionResult _ListaAuditoria()
        {
            return PartialView();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string FiltrarUsuarios(string nombre)
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

        [AcceptVerbs(HttpVerbs.Get)]
        public string Consulta(string Proyecto = "")
        {
            var activo = _Auditoria.SelectAll().OrderByDescending(x => x.AuditoriaId).Select(c => new { c.AuditoriaId, c.SessionID, c.UsuarioId, c.RolId, c.EntidadId, c.Host, c.PaginaAccedida, c.NombreControlador, c.NombreAccion }).Where(r => r.RolId.Contains(Proyecto.ToString())).ToList();
            return JsonConvert.SerializeObject(activo);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string FiltrarMiembro(string nombre, string Proyecto)
        {
            if (String.IsNullOrWhiteSpace(nombre))
            {
                var activo = _Auditoria.SelectAll().OrderByDescending(x => x.AuditoriaId).Where((x => x.RolId.Contains(Proyecto.ToString()))).ToList();
                return JsonConvert.SerializeObject(activo);
            }
            else
            {
                if (nombre.Length > 0)
                {
                    var SearchValues = nombre.ToUpper().Split(" ".ToCharArray());
                    var q = PredicateBuilder.True<Auditoria>();
                    //var q = PredicateBuilder.New<Auditoria>();

                    foreach (var value in SearchValues)
                        q = q.And(x => x.UsuarioId.ToUpper().Contains(value.ToUpper()));
                    q = q.And(x => x.RolId.Contains(Proyecto.ToString()));

                    var Paginado = _Auditoria.SelectAll().AsExpandable().Where(q).ToList();
                    Paginado = Paginado.OrderByDescending(x => x.AuditoriaId)
                                                            .ToList();

                    return JsonConvert.SerializeObject(Paginado);
                }
                else { return string.Empty; }
            }
        }
                
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult FiltrarvwAuditoria(string nombre, string Proyecto)
        {
            if (String.IsNullOrWhiteSpace(nombre))
            {
                var activo = _vwAuditoria.SelectAll().OrderByDescending(x => x.AuditoriaId).Where((x => x.RolId.Contains(Proyecto.ToString()))).ToList();
                //return JsonConvert.SerializeObject(activo);
                return Json(activo, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (nombre.Length > 0)
                {
                    var SearchValues = nombre.ToUpper().Split(" ".ToCharArray());
                    var q = PredicateBuilder.True<vwAuditoria>();                    

                    foreach (var value in SearchValues) 
                        q = q.And(x => x.BUSQUEDA.ToUpper().Contains(value.ToUpper()));
                    q = q.And(x => x.RolId.Contains(Proyecto.ToString()));                                       

                    var Paginado = _vwAuditoria.SelectAll().AsExpandable().Where(q).ToList();
                    Paginado = Paginado.OrderByDescending(x => x.AuditoriaId)
                                                            .ToList();

                    //return JsonConvert.SerializeObject(Paginado);
                    return Json(Paginado, JsonRequestBehavior.AllowGet);
                }
                else { return Json(string.Empty, JsonRequestBehavior.AllowGet); } //return string.Empty;
            }
        }

        [HttpPost]
        [CachingFilter]
        public ActionResult FiltrarAuditoria(string UsuarioId, string Proyecto, string Entidad, string Fecha, string Busqueda, Gaia.Seguridad.Model.DataTableAjaxPostModel model)
        {
            try
            {
                bool sortDir = true;
                var searchBy = Proyecto.ToUpper() + "," + Entidad + ","+ UsuarioId + "," + Fecha + "," + Busqueda;
                string sortBy = "AuditoriaId";
                var searchColumn = "RolId,EntidadId,UsuarioId,FechaRegistro,BUSQUEDA";                
                int filteredResultsCount; int totalResultsCount;
                Usuario u = SessionHelper.GetItem<Usuario>(session);

                if (model.order != null)
                {
                    sortBy = model.columns[model.order[0].column].data;
                    sortDir = model.order[0].dir.ToLower() == "asc";
                }

                if (model.search.value != null)
                {
                    searchBy = Proyecto + "," + Entidad + "," + UsuarioId + "," + Fecha + "," + model.search.value;
                    searchColumn = "RolId,EntidadId,UsuarioId,FechaRegistro";
                }
                //var s = _Auditoria.SelectAll().Where(c=> c.RolId.Contains(Proyecto.ToString()))
                var res = _vwAuditoria.fnServerSide(searchBy, searchColumn, sortBy, model.start, model.length, out filteredResultsCount, out totalResultsCount);

                return Json(new { draw = model.draw, recordsTotal = totalResultsCount, recordsFiltered = filteredResultsCount, data = res.ToList() });
            }
            catch (Exception ex) { throw ex; }
        }
    }
}