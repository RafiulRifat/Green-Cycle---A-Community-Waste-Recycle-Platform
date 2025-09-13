using Green_Cycle.Models;
using Green_Cycle.Models.Entities;
using Green_Cycle.Models.ViewModels.DropOffPoints;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Green_Cycle.Controllers
{
    [Authorize]  // all actions require sign-in by default
    public class DropOffPointsController : Controller
    {
        private readonly ApplicationDbContext _db = ApplicationDbContext.Create();

        private static readonly List<string> DefaultMaterials = new List<string>
        { "Plastic", "Paper", "Glass", "Metal", "Organic", "E-Waste" };

        // GET: /DropOffPoints
        // Search by name/address, filter by max distance (km), paging + page-size
        [HttpGet]
        public async Task<ActionResult> Index(string search, int? distance, int page = 1, int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 100);

            var q = _db.DropOffPoints.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(p => p.Name.Contains(s) || p.Address.Contains(s));
            }

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

            return View(vm); // Views/DropOffPoints/Index.cshtml
        }

        // GET: /DropOffPoints/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var point = await _db.DropOffPoints.FindAsync(id);
            if (point == null) return HttpNotFound();
            return View(point); // Views/DropOffPoints/Details.cshtml (model: DropOffPoint)
        }

        // GET: /DropOffPoints/Create
        [Authorize(Roles = "Collector,Admin"), HttpGet]
        public ActionResult Create()
        {
            return View(new CreateDropOffPointViewModel
            {
                AvailableMaterials = DefaultMaterials
            }); // Views/DropOffPoints/Create.cshtml
        }

        // POST: /DropOffPoints/Create
        [Authorize(Roles = "Collector,Admin"), HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Name,Address,Latitude,Longitude,SelectedMaterials")]
            CreateDropOffPointViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Rehydrate options so the view re-renders correctly
                vm.AvailableMaterials = vm.AvailableMaterials?.Count > 0 ? vm.AvailableMaterials : DefaultMaterials;
                return View(vm);
            }

            var entity = new DropOffPoint
            {
                Name = vm.Name?.Trim(),
                Address = vm.Address?.Trim(),
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                MaterialsAccepted = string.Join(",", vm.SelectedMaterials ?? Enumerable.Empty<string>())
                // DistanceKm can be computed elsewhere (e.g., background job or when listing)
            };

            _db.DropOffPoints.Add(entity);
            await _db.SaveChangesAsync();

            // Show green success banner on the same page, ready for another entry
            ModelState.Clear();
            return View(new CreateDropOffPointViewModel
            {
                Created = true,
                AvailableMaterials = DefaultMaterials
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
