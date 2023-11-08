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
        public ActionResult Justificante()
        {
            using (DB db = new DB())
            {
                string matricula = "Matrícula";
                var datos = db.Alumno.Where(a => a.Matricula == matricula).ToList();

                return View(datos);
            }
        }

        public ActionResult PaseDeSalida()

        {
            string matricula = "Matrícula";
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

        [HttpPost]
        public ActionResult GenerarP(string Nombre, string Primer_apellido, string Segundo_Apellido,
        string Matricula, string Carrera, string Grupo, string Motivo, string Medio, string autorizo,
        HttpPostedFileBase archivo, string TutorN, string parentezco, string nombreA)
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
                    Fecha = System.DateTime.Now


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


                if (ultimoRegistro != null)
                {
                    //  Task.Run(() => GenerarPdf(ultimoRegistro));
                    return RedirectToAction("GenerarPdf", ultimoRegistro);
                }
            }

            return RedirectToAction("PaseDeSalida");
        }

        public ActionResult GenerarPdf(Pase_de_salida modelo)
        {
            string n = modelo.Matricula + ".pdf";

            // Lógica para generar el PDF utilizando Rotativa u otra biblioteca
            var viewAsPdf = new ViewAsPdf("PDFPase", modelo);
            viewAsPdf.PageSize = Rotativa.Options.Size.Letter;
            viewAsPdf.PageMargins = new Rotativa.Options.Margins(13, 13, 13, 13);

            // Convierte el PDF en bytes
            byte[] pdfBytes = viewAsPdf.BuildFile(ControllerContext);

            // Devuelve el PDF como un archivo PDF en una nueva pestaña
            return File(pdfBytes, "application/pdf");
        }

        [HttpPost]
        public ActionResult GenerarJ(string Nombre, string Primer_apellido, string Segundo_Apellido,
            string Matricula, string Carrera, string Grupo, string Motivo, string fecha, HttpPostedFileBase archivo, DateTime? fecha1 = null
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
                        Activo = true

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
                        Activo = true

                    };
                    db.Justificante.Add(justificante);
                    db.SaveChanges();
                }

                bitacora b = new bitacora();
                { }

            }

            return RedirectToAction("Justificante");
        }



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
