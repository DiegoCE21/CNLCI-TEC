using CNLCI.Models;
using CNLCI.Models.ViewModel;
using Rotativa;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Mvc;

namespace CNLCI.Controllers
{
    [Authorize]
    public class HistorialController : Controller
    {
        public ActionResult PaseS()
        {
            using (DB db = new DB())
            {
                var Datos = db.Pase_de_salida.OrderByDescending(j => j.No__pase).ToList();

                return View(Datos);
            }
        }
        public ActionResult Justificante()
        {
            using (DB db = new DB())
            {
                var Datos = db.Justificante.OrderByDescending(j => j.No__justificante).ToList();

                return View(Datos);
            }

        }
        public ActionResult GenerarPDFP(int id)
        {

            using (DB db = new DB())
            {
                Pase_de_salida modelo = db.Pase_de_salida.OrderByDescending(x => x.No__pase == id).FirstOrDefault();
                P modelof = new P
                {
                    No__pase = modelo.No__pase,
                    Matricula = modelo.Matricula,
                    Nombre = modelo.Nombre,
                    Primer_apellido = modelo.Primer_apellido,
                    Segundo_apellido = modelo.Segundo_apellido,
                    Plan_de_Estudios = modelo.Plan_de_Estudios,
                    Grupo_Referente = modelo.Grupo_Referente,
                    Motivo = modelo.Motivo,
                    Autoriza = modelo.Autoriza,
                    Parentezco = modelo.Parentezco,
                    Medio_de_Autorizacion = modelo.Medio_de_Autorizacion,
                    Fecha = modelo.Fecha,
                    ElaboradoPor = modelo.ElaboradoPor

                };
                return new ViewAsPdf("PDFPase", modelof)
                {
                    PageSize = Rotativa.Options.Size.Letter,
                    PageMargins = new Rotativa.Options.Margins(13, 13, 13, 13)
                };
            }

        }




        public ActionResult GenerarPDFJ(int id)
        {

            using (DB db = new DB())
            {

                Justificante modelo = db.Justificante.SingleOrDefault(i => i.No__justificante == id);
                J modelof = new J
                {
                    No__justificante = modelo.No__justificante,
                    Matricula = modelo.Matricula,
                    Nombre = modelo.Nombre,
                    Primer_apellido = modelo.Primer_apellido,
                    Segundo_apellido = modelo.Segundo_apellido,
                    Plan_de_Estudios = modelo.Plan_de_Estudios,
                    Grupo_Referente = modelo.Grupo_Referente,
                    Motivo = modelo.Motivo,
                    Fecha_del_dia = modelo.Fecha_del_dia,
                    Fecha_al_dia = modelo.Fecha_al_dia,
                    ElaboradoPor = modelo.ElaboradoPor
                };

                return new ViewAsPdf("PDFJustificante", modelof)
                {
                    PageSize = Rotativa.Options.Size.Letter,
                    PageMargins = new Rotativa.Options.Margins(13, 13, 13, 13)
                };
            }

        }


        public ActionResult buscadorJ(string matricula)
        {

            using (DB db = new DB())
            {

                var resultado = db.Justificante
    .Where(j => j.Matricula.StartsWith(matricula))
    .Select(j => new
    {
        j.No__justificante,
        j.Matricula,
        j.Grupo_Referente,
        j.Motivo,
        Fecha_del_dia = SqlFunctions.DatePart("dd", j.Fecha_del_dia) + "/" + SqlFunctions.DatePart("mm", j.Fecha_del_dia) + "/" + SqlFunctions.DatePart("yyyy", j.Fecha_del_dia),
        Fecha_al_dia = SqlFunctions.DatePart("dd", j.Fecha_al_dia) + "/" + SqlFunctions.DatePart("mm", j.Fecha_al_dia) + "/" + SqlFunctions.DatePart("yyyy", j.Fecha_al_dia)

    })
    .OrderByDescending(x => x.No__justificante)
    .ToList();



                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult buscadorP(string matricula)
        {

            using (DB db = new DB())
            {

                var resultado = db.Pase_de_salida
    .Where(j => j.Matricula.StartsWith(matricula))
    .Select(j => new
    {
        j.No__pase,
        j.Matricula,
        j.Grupo_Referente,
        j.Motivo,
        Fecha = SqlFunctions.DatePart("dd", j.Fecha) + "/" + SqlFunctions.DatePart("mm", j.Fecha) + "/" + SqlFunctions.DatePart("yyyy", j.Fecha),

    })
    .OrderByDescending(x => x.No__pase)
    .ToList();



                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult p(int id)
        {

            using (DB db = new DB())
            {
                var pase = db.Pase_de_salida.FirstOrDefault(j => j.No__pase == id);

                if (pase != null && pase.Comprobante != null && pase.Comprobante.Length > 0)
                {
                    return File(pase.Comprobante, "application/octet-stream", "Comprobande de pase de salida.PDF");
                }
                else
                {
                    return HttpNotFound("El archivo no existe.");
                }
            }
        }
        public ActionResult j(int id)
        {

            using (DB db = new DB())
            {
                var Justificante = db.Justificante.FirstOrDefault(j => j.No__justificante == id);

                if (Justificante != null && Justificante.Comprobante != null && Justificante.Comprobante.Length > 0)
                {
                    return File(Justificante.Comprobante, "application/octet-stream", "Comprobande de justificante.PDF");
                }
                else
                {
                    return HttpNotFound("El archivo no existe.");
                }



            }
        }

    }
}

