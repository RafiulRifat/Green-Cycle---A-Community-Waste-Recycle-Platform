using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;                         // EF6 helpers
using Microsoft.AspNet.Identity;

using Green_Cycle.Models;
using Green_Cycle.Models.Entities;                // CollectorRoute
using Green_Cycle.Models.ViewModels;              // CollectorRoutesVm

namespace Green_Cycle.Controllers
{
    [Authorize(Roles = "Collector,Admin")]
    public class CollectorController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: /Collector/MyRoutes?status=InProgress&q=East
        [HttpGet]
        public ActionResult MyRoutes(string status = "All", string q = null)
        {
            var userId = User.Identity.GetUserId();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1); // precompute for EF

            // base: my routes for today
            var baseQuery = _db.CollectorRoutes
                               .AsNoTracking()
                               .Where(r => r.CollectorId == userId &&
                                           r.StartTime >= today && r.StartTime < tomorrow);

            // tab counters (one SQL)
            var totals = baseQuery
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    All = g.Count(),
                    NotStarted = g.Sum(r => r.Status == "NotStarted" ? 1 : 0),
                    InProgress = g.Sum(r => r.Status == "InProgress" ? 1 : 0),
                    Completed = g.Sum(r => r.Status == "Completed" ? 1 : 0)
                })
                .FirstOrDefault() ?? new { All = 0, NotStarted = 0, InProgress = 0, Completed = 0 };

            ViewBag.CountAll = totals.All;
            ViewBag.CountNotStarted = totals.NotStarted;
            ViewBag.CountInProgress = totals.InProgress;
            ViewBag.CountCompleted = totals.Completed;

            // listing query (search + status)
            var query = baseQuery;

            if (!string.IsNullOrWhiteSpace(q))
            {
                var ql = q.Trim().ToLower();
                query = query.Where(r => (r.ZoneName ?? "").ToLower().Contains(ql));
            }

            switch ((status ?? "All").Trim())
            {
                case "NotStarted":
                    query = query.Where(r => r.Status == "NotStarted");
                    break;
                case "InProgress":
                    query = query.Where(r => r.Status == "InProgress");
                    break;
                case "Completed":
                    query = query.Where(r => r.Status == "Completed");
                    break;
                    // "All" => no extra filter
            }

            var routes = query
                .OrderBy(r => r.StartTime)
                .AsNoTracking()
                .ToList();

            // keep UI state
            ViewBag.Status = status ?? "All";
            ViewBag.Q = q ?? "";

            var vm = new CollectorRoutesVm { Routes = routes };
            return View(vm);
        }

        // GET: /Collector/RouteDetails/5
        // Minimal details page (no .Stops access so it compiles even if you haven't added RouteStop yet)
        [HttpGet]
        public ActionResult RouteDetails(int id)
        {
            var userId = User.Identity.GetUserId();

            var route = _db.CollectorRoutes
                           .AsNoTracking()
                           .FirstOrDefault(r => r.Id == id &&
                                                (r.CollectorId == userId || User.IsInRole("Admin")));

            if (route == null) return HttpNotFound();

            return View(route); // Views/Collector/RouteDetails.cshtml (model = CollectorRoute)
        }

        // POST: /Collector/StartRoute/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult StartRoute(int id, string status = "All", string q = null)
        {
            var route = _db.CollectorRoutes.Find(id);
            if (route != null)
            {
                route.Status = "InProgress";
                _db.SaveChanges();
            }
            // explicit keys to avoid older compiler issues with anonymous-type shorthand
            return RedirectToAction(nameof(MyRoutes), new { status = status, q = q });
        }

        // POST: /Collector/CompleteRoute/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CompleteRoute(int id, string status = "All", string q = null)
        {
            var route = _db.CollectorRoutes.Find(id);
            if (route != null)
            {
                route.Status = "Completed";
                route.CompletedStops = route.TotalStops;
                route.EstimatedFinish = DateTime.Now;
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(MyRoutes), new { status = status, q = q });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
