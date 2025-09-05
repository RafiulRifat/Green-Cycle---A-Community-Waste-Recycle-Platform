using Green_Cycle.Models;
using Green_Cycle.Models.Entities;    // 👈
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Green_Cycle.Controllers
{
    public class DropOffPointsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public async Task<ActionResult> Index(string search, int? distance)
        {
            var q = _db.DropOffPoints.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(p => p.Name.Contains(search) || p.Address.Contains(search));

            if (distance.HasValue)
                q = q.Where(p => p.DistanceKm <= distance.Value);

            var list = await q.OrderBy(p => p.DistanceKm).ThenBy(p => p.Name).ToListAsync();
            return View(list);
        }

        public async Task<ActionResult> Details(int id)
        {
            var point = await _db.DropOffPoints.FindAsync(id);
            if (point == null) return HttpNotFound();
            return View(point);
        }

        public ActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DropOffPoint model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.DropOffPoints.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
