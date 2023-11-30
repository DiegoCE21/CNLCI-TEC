using CNLCI.Models;
using CNLCI.Models.ViewModel;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace CNLCI.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {

        public ActionResult PorAlumno() { return View(); }

        [HttpPost]
        public ActionResult TraerDatos(string matricula, DateTime fechaInicial, DateTime fechaFinal)
        {
            using (DB db = new DB())
            {

                var matriculaBuscada = matricula;

                var response = db.Justificante
                    .Where(j => j.Matricula == matriculaBuscada &&
                        j.Fecha_al_dia.HasValue &&
                        j.Fecha_al_dia.Value >= fechaInicial &&
                        j.Fecha_al_dia.Value <= fechaFinal)
                    .GroupBy(j => new { Mes = j.Fecha_al_dia.Value.Month, Año = j.Fecha_al_dia.Value.Year })
                    .Select(g => new ReporteA
                    {
                        Mes = g.Key.Mes,
                        Año = g.Key.Año,
                        Valor = g.Count()
                    })
                    .ToList();

                return Json(response, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult PorGrupo() { return View(); }
        public ActionResult PorCarrera() { return View(); }
        public ActionResult PorSemestre() { return View(); }
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reportes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reportes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reportes/Create
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

        // GET: Reportes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reportes/Edit/5
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

        // GET: Reportes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reportes/Delete/5
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
