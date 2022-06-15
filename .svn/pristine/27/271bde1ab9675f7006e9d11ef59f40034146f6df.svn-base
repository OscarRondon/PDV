using EntidadesPDV.CargaArchivos;
using NegocioPDV.CargaArchivos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDV.Controllers
{
    public class CargaArchivosController : Controller
    {
        // GET: Upload  
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SubeArchivos()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubeArchivo(HttpPostedFileBase file)
        {
            CargaArchivosBusiness.LeeLineas(Server.MapPath("~/Reports/RESPUESTA.TXT"), Server.MapPath("~/Reports/PRUEBANUEVO_"));

            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Reports"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "Bien";
                return View();
            }
            catch
            {
                ViewBag.Message = "Mal";
                return View();
            }
        }
    }
}
