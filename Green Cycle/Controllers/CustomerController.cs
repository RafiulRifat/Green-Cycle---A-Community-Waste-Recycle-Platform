using System.Threading.Tasks;
using System.Web.Mvc;
using Green_Cycle.Models;
using Green_Cycle.Models.ViewModels.Account;
using Microsoft.AspNet.Identity;

namespace GreenCycle.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = _db.Users.Find(userId);

            var vm = new CustomerProfileViewModel
            {
                FullName = user?.FullName,
                Email = user?.Email,
                PhoneNumber = user?.PhoneNumber,
                RecognitionCount = user?.RecognitionCount ?? 0
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(CustomerProfileViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = User.Identity.GetUserId();
            var user = _db.Users.Find(userId);
            if (user != null)
            {
                user.FullName = vm.FullName;
                user.Email = vm.Email;          // Identity also keeps normalized values
                user.UserName = vm.Email;       // keep username aligned if you're using email as username
                user.PhoneNumber = vm.PhoneNumber;
                _db.SaveChanges();
            }

            TempData["ProfileSaved"] = true;
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
