using System.Web.Mvc;
using Green_Cycle.Models.ViewModels.Issues;

namespace Green_Cycle.Controllers
{
    [Authorize]
    public class IssuesController : Controller
    {
        // GET: /Issues/Raise
        public ActionResult Raise()
        {
            // empty form
            return View(new IssueVm { IssueType = "Pickup", Priority = "Low" });
        }

        // POST: /Issues/Raise
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Raise(IssueVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // TODO: persist to DB. For now, flash a success message.
            TempData["IssueSuccess"] = "Thanks! Your issue was submitted.";
            return RedirectToAction("Raise");
        }
    }
}
