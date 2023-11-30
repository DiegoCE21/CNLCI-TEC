using CNLCI.Models;
using CNLCI.Models.ViewModel;
using Rotativa;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CNLCI.Controllers
{
    [Authorize]
    public class JustificantesController : Controller
    {
        // GET: Justificantes
        public ActionResult Justificante(string ruta = null)
        {
            ViewBag.Ruta = ruta;
            using (DB db = new DB())
            {
                string matricula = "Matrícula";
                var datos = db.Alumno.Where(a => a.Matricula == matricula).ToList();

                return View(datos);
            }
        }

        public JsonResult ObtenerGruposPorCarrera(string planDeEstudiosCve)
        {
            // Lógica para obtener grupos en función del planDeEstudiosCve
            using (DB db = new DB())
            {
                var grupos = db.Grupo
                    .Where(g => g.Carrera.Plan_de_Estudios == planDeEstudiosCve)
                    .Select(g => new { Nombre = g.Grupo_Referente })
                    .ToList();

                // Devolver los datos como JsonResult
                return Json(grupos, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult PaseDeSalida(string ruta = null)
        {

            ViewBag.Ruta = ruta;
            string matricula = "Matrícula";
            using (DB db = new DB())
            {
                var planesEstudio = db.Carrera.ToList();
                ViewBag.PlanesEstudio = new SelectList(planesEstudio, "Plan_de_Estudios_Cve", "Plan_de_Estudios");

                var datos = (from Alumno in db.Alumno
                             join Tutor in db.Tutor on Alumno.Matricula equals Tutor.Matricula
                             where matricula == Alumno.Matricula
                             select new MJ
                             {
                                 Matricula = Alumno.Matricula,
                                 Nombre = Alumno.Nombre,
                                 Primer_apellido = Alumno.Primer_apellido,
                                 Segundo_apellido = Alumno.Segundo_apellido,
                                 Tutor_Nombre = Tutor.Tutor_Nombre

                             }).ToList();
                return View("PaseDeSalida", datos);


            }

        }



        [HttpPost]
        public ActionResult GenerarP(string Nombre, string Primer_apellido, string Segundo_Apellido,
        string Matricula, string Carrera, string Grupo, string Motivo, string Medio, string autorizo,
        HttpPostedFileBase archivo, string TutorN, string parentezco, string nombreA, string Observaciones)
        {
            string par, TN;
            byte[] data;
            if (archivo != null)
            {
                string Extension = Path.GetExtension(archivo.FileName);
                MemoryStream ms = new MemoryStream();
                archivo.InputStream.CopyTo(ms);
                data = ms.ToArray();

            }
            else { data = null; }

            using (DB db = new DB())
            {

                if (autorizo == "Tutor")
                {
                    TN = TutorN;
                    par = "TUTOR";
                }
                else
                {
                    par = parentezco;
                    TN = nombreA;
                }


                Pase_de_salida p = new Pase_de_salida
                {
                    Matricula = Matricula,
                    Nombre = Nombre,
                    Primer_apellido = Primer_apellido,
                    Segundo_apellido = Segundo_Apellido,
                    Plan_de_Estudios = Carrera,
                    Grupo_Referente = Grupo,
                    Motivo = Motivo,
                    Autoriza = TN,
                    Parentezco = par,
                    Medio_de_Autorizacion = Medio,
                    Comprobante = data,
                    Activo = true,
                    Valor = 1,
                    Fecha = System.DateTime.Now,
                    Observaciones = Observaciones


                };
                db.Pase_de_salida.Add(p);
                db.SaveChanges();

                int ultimoNoPase = db.Pase_de_salida.Max(up => up.No__pase);
                bitacora b = new bitacora
                {
                    accion = "Insercion",
                    realizada_por = User.Identity.Name,
                    fecha = System.DateTime.Now,
                    tabla = "Pase de salida",
                    folio = ultimoNoPase
                };

                db.bitacora.Add(b);
                db.SaveChanges();

                Pase_de_salida ultimoRegistro = db.Pase_de_salida.OrderByDescending(x => x.No__pase).FirstOrDefault();

                /*  if (ultimoRegistro != null)
                 {
                     // Crear un objeto anónimo con las propiedades del último registro
                     var resultadoJson = new
                     {
                         NoPase = ultimoRegistro.No__pase,
                         Matricula = ultimoRegistro.Matricula,
                         // Agrega más propiedades según tus necesidades
                     };

                     // Devolver un JsonResult con el objeto anónimo
                     return Json(resultadoJson, JsonRequestBehavior.AllowGet);
                 }*/
                if (ultimoRegistro != null)
                {
                    //  Task.Run(() => GenerarPdf(ultimoRegistro));
                    return RedirectToAction("GenerarPDF", ultimoRegistro);
                }
            }

            return RedirectToAction("PaseDeSalida");
            //  return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerarPDF(Pase_de_salida modelo)
        {

            return new ViewAsPdf("PDFPase", modelo)
            {
                PageSize = Rotativa.Options.Size.Letter,
                PageMargins = new Rotativa.Options.Margins(13, 13, 13, 13)
            };
        }

        /*  public ActionResult GenerarPdfP(Pase_de_salida modelo)
          {
              string nombreArchivo = modelo.Matricula + "P" + modelo.No__pase + ".pdf";
              var viewAsPdf = new ViewAsPdf("PDFPase", modelo);
              viewAsPdf.PageSize = Rotativa.Options.Size.Letter;
              viewAsPdf.PageMargins = new Rotativa.Options.Margins(13, 13, 13, 13);
              byte[] pdfBytes = viewAsPdf.BuildFile(ControllerContext);
              string rutaArchivo = Path.Combine(Server.MapPath("~/ArchivosPDF"), nombreArchivo);
              System.IO.File.WriteAllBytes(rutaArchivo, pdfBytes);
              return RedirectToAction("PaseDeSalida", new { ruta = nombreArchivo }); 
          }*/




        [HttpPost]
        public ActionResult GenerarJ(string Nombre, string Primer_apellido, string Segundo_Apellido,
            string Matricula, string Carrera, string Grupo, string Motivo, string fecha, string Observaciones, HttpPostedFileBase archivo, DateTime? fecha1 = null
            , DateTime? fechaDelDia = null, DateTime? fechaAlDia = null)
        {

            byte[] data;
            if (archivo != null)
            {

                string Extension = Path.GetExtension(archivo.FileName);
                MemoryStream ms = new MemoryStream();
                archivo.InputStream.CopyTo(ms);
                data = ms.ToArray();

            }
            else { data = null; }
            using (DB db = new DB())
            {


                if (fecha == "unica")
                {

                    Justificante justificante = new Justificante
                    {
                        Nombre = Nombre,
                        Primer_apellido = Primer_apellido,
                        Segundo_apellido = Segundo_Apellido,
                        Matricula = Matricula,
                        Plan_de_Estudios = Carrera,
                        Grupo_Referente = Grupo,
                        Motivo = Motivo,
                        Fecha_del_dia = fecha1,
                        Fecha_al_dia = fecha1,
                        Comprobante = data,
                        Valor = 1,
                        Activo = true,
                        Observaciones = Observaciones

                    };

                    db.Justificante.Add(justificante);
                    db.SaveChanges();
                }
                else
                {
                    int valor = 0;

                    for (DateTime F = (DateTime)fechaDelDia; F <= (DateTime)fechaAlDia; F = F.AddDays(1))
                    {
                        valor = valor + 1;
                    }

                    Justificante justificante = new Justificante
                    {
                        Nombre = Nombre,
                        Primer_apellido = Primer_apellido,
                        Segundo_apellido = Segundo_Apellido,
                        Matricula = Matricula,
                        Plan_de_Estudios = Carrera,
                        Grupo_Referente = Grupo,
                        Motivo = Motivo,
                        Fecha_del_dia = fechaDelDia,
                        Fecha_al_dia = fechaAlDia,
                        Comprobante = data,
                        Valor = valor,
                        Activo = true,
                        Observaciones = Observaciones

                    };
                    db.Justificante.Add(justificante);
                    db.SaveChanges();
                }

                int ultimoNoJus = db.Justificante.Max(up => up.No__justificante);
                bitacora b = new bitacora
                {
                    accion = "Insercion",
                    realizada_por = User.Identity.Name,
                    fecha = System.DateTime.Now,
                    tabla = "Justificantes",
                    folio = ultimoNoJus
                };

                db.bitacora.Add(b);
                db.SaveChanges();

                Justificante ultimoRegistro = db.Justificante.OrderByDescending(x => x.No__justificante).FirstOrDefault();
                if (ultimoRegistro != null)
                {
                    //  Task.Run(() => GenerarPdf(ultimoRegistro));
                    return RedirectToAction("GenerarPdfJ", ultimoRegistro);
                }
            }

            return RedirectToAction("Justificante");
        }
        public ActionResult GenerarPDFJ(Justificante modelo)
        {

            return new ViewAsPdf("PDFJustificante", modelo)
            {
                PageSize = Rotativa.Options.Size.Letter,
                PageMargins = new Rotativa.Options.Margins(13, 13, 13, 13)
            };
        }

        /*  public ActionResult GenerarPdfJ(Justificante modelo)
          {
              string nombreArchivo = modelo.Matricula + "J" + modelo.No__justificante + ".pdf";
              var viewAsPdf = new ViewAsPdf("PDFJustificante", modelo);
              viewAsPdf.PageSize = Rotativa.Options.Size.Letter;
              viewAsPdf.PageMargins = new Rotativa.Options.Margins(13, 13, 13, 13);
              byte[] pdfBytes = viewAsPdf.BuildFile(ControllerContext);
              string rutaArchivo = Path.Combine(Server.MapPath("~/ArchivosPDF"), nombreArchivo);
              System.IO.File.WriteAllBytes(rutaArchivo, pdfBytes);
              return RedirectToAction("Justificante", new { ruta = nombreArchivo });
          }*/

        public ActionResult Buscar(string matricula)
        {
            if (matricula != "")
            {
                using (DB db = new DB())
                {
                    var datos = db.Alumno.Where(a => a.Matricula == matricula).ToList();
                    return View("Justificante", datos);
                }
            }
            else { return RedirectToAction("Justificante"); }


        }

        public ActionResult BuscarP(string matricula)
        {
            using (DB db = new DB())
            {
                var datos = (from Alumno in db.Alumno
                             join Tutor in db.Tutor on Alumno.Matricula equals Tutor.Matricula
                             where matricula == Alumno.Matricula
                             select new MJ
                             {
                                 Matricula = Alumno.Matricula,
                                 Nombre = Alumno.Nombre,
                                 Primer_apellido = Alumno.Primer_apellido,
                                 Segundo_apellido = Alumno.Segundo_apellido,
                                 Tutor_Nombre = Tutor.Tutor_Nombre

                             }).ToList();
                return View("PaseDeSalida", datos);


            }

        }
    }
}
