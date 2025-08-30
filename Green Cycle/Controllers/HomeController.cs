using System.Web.Mvc;

namespace Green_Cycle.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() => View();
        public ActionResult Privacy() => View();
        public ActionResult Thanks() => View();
    }
}
