
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDV.Controllers
{
    public class DocumentoController : ComunController
    {
        // GET: Documento
        public ActionResult Index()
        {
            return View();
        }

        // GET: Documento/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Documento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Documento/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Documento/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Documento/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Documento/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Documento/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Documento
        public ActionResult AnulacionDocumentos()
        {
            return View();
        }
    }
}
