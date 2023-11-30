using CNLCI.Models;
using Rotativa;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace CNLCI.Controllers
{
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
        public ActionResult GenerarPDFP(string matricula)
        {

            using (DB db = new DB())
            {

                var modelo = db.Pase_de_salida.Where(a => a.Matricula == matricula).ToList();

                return new ViewAsPdf("PDFPase", modelo)
                {
                    PageSize = Rotativa.Options.Size.Letter,
                    PageMargins = new Rotativa.Options.Margins(13, 13, 13, 13)
                };
            }

        }

        public ActionResult GenerarPDFJ(string matricula)
        {

            using (DB db = new DB())
            {

                var modelo = db.Justificante.Where(a => a.Matricula == matricula).ToList();

                return new ViewAsPdf("PDFJustificante", modelo)
                {
                    PageSize = Rotativa.Options.Size.Letter,
                    PageMargins = new Rotativa.Options.Margins(13, 13, 13, 13)
                };
            }

        }
    }
}
