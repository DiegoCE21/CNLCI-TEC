﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CNLCI.App_Start
{
    public class JustificantesController : Controller
    {
        // GET: Justificantes
        public ActionResult Index()
        {
            return View();
        }

        // GET: Justificantes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Justificantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Justificantes/Create
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

        // GET: Justificantes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Justificantes/Edit/5
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

        // GET: Justificantes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Justificantes/Delete/5
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
