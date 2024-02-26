using System.Text;
using System.Web.Mvc;
using System.Configuration;

using Gaia.Helpers;
using Gaia.Seguridad.Model;
using System.Web;
using System.Net.Http;

using Newtonsoft.Json;
using System;
using PJN.DAL;
using PJN.DAL.Model;
using Newtonsoft.Json.Linq;
using static Gaia.Seguridad.Controllers.UsuarioController;

namespace Gaia.Seguridad.Controllers
{
    public class RouterController : Controller
    {
        HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
        PJN.DAL.Model.Utilisatrice sessionUsuarioActual;       

        private int Retorno = -99;
        private string Mensaje = string.Empty;

        [Route(@"~/{id}")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Ir(string id)
        {         
            if (string.IsNullOrEmpty(id))
                return Json(new { Retorno, Mensaje = "No se puede ejecutar la acción solicitada" });

            sessionUsuarioActual = SessionHelper.GetItem<PJN.DAL.Model.Utilisatrice>(session);
            var toke = "";

            var param = Request.Params;
            string ruta = HtmlEncriptUrl.DecodeUrl(id);
            var appstring = ruta.Split('_');
            var adress = appstring[1];
            string baseurl = ConfigurationManager.AppSettings[appstring[0]];
            
            foreach(var item in param.Keys)
            {
                var name = "{" + item + "}";
                var value = param.Get(item.ToString());

                adress = adress.Replace(name, value);
            }

            if (string.IsNullOrEmpty(adress) || string.IsNullOrEmpty(baseurl))
                return Json(new { Retorno, Mensaje = "No se puede ejecutar la acción solicitada" });


            if (sessionUsuarioActual.VIN != null) { toke = sessionUsuarioActual.VIN; };

            var res = WebService.ApiGet(adress, baseurl, toke, out Retorno, out Mensaje);

            if (Retorno == 401)
                return Json(new { Retorno = Retorno, Mensaje = "No esta autorizado para está acción." }, JsonRequestBehavior.AllowGet);
            //return Json(new { Retorno, Mensaje });

            return Content(res, "application/json", Encoding.UTF8);
        }

        [Route(@"~/{id}")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IrPost(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new { Retorno, Mensaje = "No se puede ejecutar la acción solicitada" });

            sessionUsuarioActual = SessionHelper.GetItem<PJN.DAL.Model.Utilisatrice>(session);
            var toke = "";

            var param = Request.Params;
            string ruta = HtmlEncriptUrl.DecodeUrl(id);
            var appstring = ruta.Split('_');
            var adress = appstring[1];
            string baseurl = ConfigurationManager.AppSettings[appstring[0]];
            
            if (string.IsNullOrEmpty(appstring[1]) || string.IsNullOrEmpty(baseurl))
                return Json(new { Retorno, Mensaje = "No se puede ejecutar la acción solicitada" });

            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();

            foreach (var item in param.Keys)
            {
                var name = "{" + item + "}";
                var value = param.Get(item.ToString());

                adress = adress.Replace(name, value);
            }

            if (sessionUsuarioActual.VIN != null) { toke = sessionUsuarioActual.VIN; };

            var res = WebService.WebApipost(adress, baseurl, HttpUtility.UrlDecode(json), "application/x-www-form-urlencoded", toke, out Retorno, out Mensaje);

            if(Retorno == 401)
                return Json(new { Retorno = Retorno, Mensaje = "No esta autorizado para está acción." }, JsonRequestBehavior.AllowGet);                
            
            return Content(res, "application/json", Encoding.UTF8);
        }

        [Route(@"~/{id}")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IrPaginado(Gaia.Seguridad.Model.DataTableAjaxPostModel model, string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new { Retorno, Mensaje = "No se puede ejecutar la acción solicitada" });

            sessionUsuarioActual = SessionHelper.GetItem<PJN.DAL.Model.Utilisatrice>(session);
            var toke = "";

            var param = Request.Params;
            string ruta = HtmlEncriptUrl.DecodeUrl(id);
            var appstring = ruta.Split('_');
            var adress = appstring[1];
            string baseurl = ConfigurationManager.AppSettings[appstring[0]];

            if (string.IsNullOrEmpty(appstring[1]) || string.IsNullOrEmpty(baseurl))
                return Json(new { Retorno, Mensaje = "No se puede ejecutar la acción solicitada" });

            var sortDir = "asc";
            string sortBy = model.columns[model.order[0].column].data;
            var json = ""; var Campo = ""; var Valor = "";
            var searchBy = (model.search != null) ? model.search.value : null;

            if (model.order != null)
            {
                sortBy = model.columns[model.order[0].column].data;
            }

            if (String.IsNullOrWhiteSpace(searchBy) == false)
            {
                if (model.search.regex != "false")
                {
                    if (model.search.regex == "100")
                    {
                        var sBusqueda = "Busqueda";
                        Campo = sBusqueda;
                        Valor = searchBy.Replace(" ", "%");
                    }
                    else
                    {                     
                        var IndiceBusqueda = Convert.ToInt32(model.search.regex);
                        Campo = model.columns[IndiceBusqueda].data;
                        Valor = searchBy.Replace(" ", "%");
                    }
                }
                json = "{\"JsonCondicional\":{\"Campo\":\"" + Campo + "\",\"Valor\":\"" + Valor + "\",\"OrdenPor\":\"" + sortBy + "\",\"OrdenTipo\":\"" + sortDir + "\",\"NPagina\":\"" + model.start + "\",\"NRegPag\":\"" + model.length + "\"}}";
                json = json.ToString().Replace("/ /g", "");
            }
            else
            {
                json = "{\"JsonCondicional\":{\"Campo\":\"" + Campo + "\",\"Valor\":\"" + Valor + "\",\"OrdenPor\":\"" + sortBy + "\",\"OrdenTipo\":\"" + sortDir + "\",\"NPagina\":\"" + model.start + "\",\"NRegPag\":\"" + model.length + "\"}}";
                json = json.ToString().Replace("/ /g", "");
            }

            if (sessionUsuarioActual.VIN != null) { toke = sessionUsuarioActual.VIN; };

            var res = WebService.WebApipost(adress, baseurl, HttpUtility.UrlDecode(json), "application/json", toke, out Retorno, out Mensaje);

            if (Retorno == 0)
            {
                var jsonDeserializado = JsonConvert.DeserializeObject<dynamic>(res);
                var Datos = jsonDeserializado["Objeto"]["Datos"];
                var NRegistro = jsonDeserializado["Objeto"]["NRegistro"];
                var NTotalRegistro = jsonDeserializado["Objeto"]["NTotalRegistro"];
                var result = System.Web.Helpers.Json.Decode<dynamic>(Datos.ToString());
                return Json(new { Retorno = Retorno, Mensaje = Mensaje, draw = model.draw, recordsTotal = NTotalRegistro.Value, recordsFiltered = NRegistro.Value, data = result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Datos = "[]";
                var result = System.Web.Helpers.Json.Decode<dynamic>(Datos.ToString());
                return Json(new { Retorno = Retorno, Mensaje = Mensaje, draw = 0, recordsTotal = 0, recordsFiltered = 0, data = result });
                //return Json(new { Retorno = 404, Mensaje = Mensaje, draw = 1, recordsTotal = 0, recordsFiltered = 0, error = "{\"code\":\"" + Retorno + "\",\"message\":\"" + Mensaje + "\"}" }, JsonRequestBehavior.AllowGet);                    
            }           
        }
    }
}