using Green_Cycle.Models;
using Green_Cycle.Models.Entities;
using Green_Cycle.Models.ViewModels.DropOffPoints;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Green_Cycle.Controllers
{
    [Authorize]  // ✅ add this
    public class DropOffPointsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: /DropOffPoints
        // Search by name/address, filter by max distance (km), paging + page-size
        [HttpGet]
        public async Task<ActionResult> Index(string search, int? distance, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var q = _db.DropOffPoints.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(p => p.Name.Contains(search) || p.Address.Contains(search));

            if (distance.HasValue)
                q = q.Where(p => p.DistanceKm <= distance.Value);

            q = q.OrderBy(p => p.DistanceKm).ThenBy(p => p.Name);

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var vm = new DropOffPointsIndexViewModel
            {
                Items = items,
                Search = search,
                Distance = distance,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            return View(vm); // View: Views/DropOffPoints/Index.cshtml (typed to DropOffPointsIndexViewModel)
        }

        // GET: /DropOffPoints/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var point = await _db.DropOffPoints.FindAsync(id);
            if (point == null) return HttpNotFound();
            return View(point); // View: Views/DropOffPoints/Details.cshtml (typed to DropOffPoint)
        }

        // GET: /DropOffPoints/Create
        [Authorize, HttpGet]
        public ActionResult Create()
        {
            return View(new CreateDropOffPointViewModel()); // View: Views/DropOffPoints/Create.cshtml
        }

        // POST: /DropOffPoints/Create
        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Name,Address,Latitude,Longitude,SelectedMaterials")]
            CreateDropOffPointViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Rehydrate chips list so the view renders properly after validation errors
                if (vm.AvailableMaterials == null || vm.AvailableMaterials.Count == 0)
                {
                    vm.AvailableMaterials = new List<string>
                    { "Plastic", "Paper", "Glass", "Metal", "Organic", "E-Waste" };
                }
                return View(vm);
            }

            var entity = new DropOffPoint
            {
                Name = vm.Name,
                Address = vm.Address,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                MaterialsAccepted = string.Join(",", vm.SelectedMaterials ?? Enumerable.Empty<string>())
                // DistanceKm: compute elsewhere if you need it
            };

            _db.DropOffPoints.Add(entity);
            await _db.SaveChangesAsync();

            // Show green success banner on the same page
            ModelState.Clear();
            return View(new CreateDropOffPointViewModel { Created = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
