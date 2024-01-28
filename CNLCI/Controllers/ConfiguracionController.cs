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
        public ActionResult Importar()
        { return View(); }


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
                        mensaje = "Los registros se han subido correctamente.";
                    }
                    catch (Exception ex)
                    {
                        mensaje = "Error al subir los registros: " + ex.Message;
                    }
                }
            }
            else
            {
                mensaje = "No se ha seleccionado ningún archivo o el archivo está vacío.";
            }



            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CopiaS()
        {
            string connectionString = "data source=WIN-HTKAANTQI7R\\CONALEPCHECKIN;initial catalog=CONALEPcheckin;persist security info=True;;Initial Catalog=CONALEPcheckin;user id=sa;password=Chec-123";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var nombre = "N'C:/Users/Administrador/Downloads/CONALEPcheckin"+ DateTime.Now.ToString().Substring(0, 2)+ DateTime.Now.ToString().Substring(3,2) + DateTime.Now.ToString().Substring(6, 4) + ".bak'";

                using (var command = new SqlCommand("BACKUP DATABASE [CONALEPcheckin] TO DISK = "+nombre+" WITH NOFORMAT, NOINIT, NAME = N'CONALEPcheckin-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            return Json("La copia de seguridad se ha guardado con éxito en Descargas", JsonRequestBehavior.AllowGet);
        }


    }
}

