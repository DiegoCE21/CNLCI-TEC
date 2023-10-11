using System.Web.Mvc;

namespace CNLCI.Controllers
{
    [Authorize]
    public class JustificantesController : Controller
    {
        // GET: Justificantes
        public ActionResult Justificante()
        { return View(); }

        public ActionResult PaseDeSalida()
        { return View(); }
    }
}
