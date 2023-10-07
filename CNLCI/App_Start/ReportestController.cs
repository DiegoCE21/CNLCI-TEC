using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CNLCI.App_Start
{
    public class ReportestController : Controller
    {
        // GET: Reportest
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reportest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reportest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reportest/Create
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

        // GET: Reportest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reportest/Edit/5
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

        // GET: Reportest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reportest/Delete/5
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
