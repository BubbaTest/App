using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gaia.Notificacion.Controllers
{
    public class ConfiguracionController : Controller
    {
        // GET: Configuracion
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ModalConfiguracion()
        {
            return PartialView("ConfigNotificacion");
        }
    }
}