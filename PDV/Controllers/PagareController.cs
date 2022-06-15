using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDV.Controllers
{
    public class PagareController : ComunController
    {
        // GET: Pagare
        public ActionResult Index()
        {
            return View();
        }

        // GET: Pagare/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pagare/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pagare/Create
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

        // GET: Pagare/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pagare/Edit/5
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

        // GET: Pagare/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pagare/Delete/5
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
    }
}
