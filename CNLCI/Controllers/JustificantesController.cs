using CNLCI.Models;
using CNLCI.Models.ViewModel;
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
                string matricula = "matrícula";
                var datos = db.Alumno.Where(a => a.Matricula == matricula).ToList();

                return View(datos);
            }
        }

        public ActionResult PaseDeSalida()

        {
            string matricula = "";
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
        public ActionResult GenerarJ(string Nombre, string Primer_apellido, string Segundo_Apellido,
            string Matricula, string Carrera, string Grupo, string Motivo, string fecha, HttpPostedFileBase archivo, DateTime? fecha1 = null
            , DateTime? fechaDelDia = null, DateTime? fechaAlDia = null)
        {
            string Extension = Path.GetExtension(archivo.FileName);
            MemoryStream ms = new MemoryStream();
            archivo.InputStream.CopyTo(ms);
            byte[] data = ms.ToArray();
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
                        Fecha = fecha1,
                        Comprobante = data


                    };
                    db.Justificante.Add(justificante);
                    db.SaveChanges();
                }
                else
                {


                    for (DateTime F = (DateTime)fechaDelDia; F <= (DateTime)fechaAlDia; F = F.AddDays(1))
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
                            Fecha = F,
                            Comprobante = data


                        };
                        db.Justificante.Add(justificante);
                        db.SaveChanges();
                    }

                }

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
