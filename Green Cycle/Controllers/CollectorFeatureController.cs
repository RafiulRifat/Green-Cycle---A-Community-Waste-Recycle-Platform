using System.Web.Mvc;

namespace Green_Cycle.Controllers
{
    [Authorize(Roles = "Collector,Admin")]
    public class CollectorFeatureController : Controller
    {
        public ActionResult Index()
        {
            // TODO: replace placeholder with your upcoming UI
            return View();
        }
    }
}
