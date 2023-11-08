using CNLCI.Models;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace CNLCI.Controllers
{
    [Authorize]
    public class ConfiguracionController : Controller
    {
        // GET: Configuracion
        public ActionResult Index()
        {
            return View();
        }

        // GET: Configuracion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Configuracion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Configuracion/Create
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

        // GET: Configuracion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Configuracion/Edit/5
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

        // GET: Configuracion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Configuracion/Delete/5
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





        public ActionResult Importar()
        { return View(); }

        /*  [HttpPost]
          public ActionResult Importar(HttpPostedFileBase archivo)
          {
              if (archivo != null && archivo.ContentLength > 0)
              {
                  string RutaArchivo = Server.MapPath("~/ArchivosPI/" + Path.GetFileName(archivo.FileName));
                  archivo.SaveAs(RutaArchivo);

                  using (DB db = new DB())
                  {

                      db.Database.ExecuteSqlCommand("EXEC importar @RutaArchivo", new SqlParameter("@RutaArchivo", RutaArchivo));
                  }

              }

              return RedirectToAction("Importar");
          }*/
        [HttpPost]
        public ActionResult Importar(HttpPostedFileBase archivo)
        {
            string mensaje = "";

            if (archivo != null && archivo.ContentLength > 0)
            {
                string RutaArchivo = Server.MapPath("~/ArchivosPI/" + Path.GetFileName(archivo.FileName));
                archivo.SaveAs(RutaArchivo);

                using (DB db = new DB())
                {
                    try
                    {
                        db.Database.ExecuteSqlCommand("EXEC importar @RutaArchivo", new SqlParameter("@RutaArchivo", RutaArchivo));
                        mensaje = "Los archivos se han subido correctamente.";
                    }
                    catch (Exception ex)
                    {
                        mensaje = "Error al subir los archivos: " + ex.Message;
                    }
                }
            }
            else
            {
                mensaje = "No se ha seleccionado ningún archivo o el archivo está vacío.";
            }

            ViewBag.UploadResult = mensaje;

            return View();
        }

    }
}
