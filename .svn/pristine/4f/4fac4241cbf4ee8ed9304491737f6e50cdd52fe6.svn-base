using PDV.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PDV.Controllers
{
    public class HomeController : ComunController
    {

        [AuthorizePDV]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UsuarioSinAcceso()
        {
            ViewBag.Mensaje = "Usuario no tiene acceso a la ruta indicada.";

            return View("Index");
        }
        [AuthorizePDV]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AuthorizePDV]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AuthorizePDV]
        public ActionResult CajaNoValida()
        {
            ViewBag.Mensaje = "Caja seleccionada no es válida para el usuario actual.";
            return View("Index");
        }
    }
}