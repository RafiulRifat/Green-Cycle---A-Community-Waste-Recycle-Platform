using System.Web.Mvc;

namespace Green_Cycle.Controllers
{
    [Authorize(Roles = "Collector,Admin")]
    public class CollectorFeatureController : Controller
    {
        public ActionResult Index()
        {
            // Redirect the old placeholder to the real dashboard
            return RedirectToAction("MyRoutes", "Collector");
        }
    }
}
