using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Green_Cycle.Data;
using Green_Cycle.Models.ViewModels.Machines;

namespace Green_Cycle.Controllers
{
    [Authorize] // requires authentication for all actions in this controller
    public class MachinesController : Controller
    {
        private readonly GreenCycleContext db = new GreenCycleContext();

        // GET: /Machines/Nearby?q=&status=
        public ActionResult Nearby(string q = null, string status = null)
        {
            var query = db.Machines
                          .AsNoTracking()
                          .Include(m => m.Zone)
                          .AsQueryable();

            // Search by code, address, or zone
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(m =>
                    (m.Address != null && m.Address.Contains(q)) ||
                    (m.Zone.Name != null && m.Zone.Name.Contains(q)) ||
                    (m.Code != null && m.Code.Contains(q)));
            }

            // Status filter (case-insensitive; supports "Near Full" and "NearFull")
            if (!string.IsNullOrWhiteSpace(status))
            {
                var norm = status.Trim();
                if (norm.Equals("Near Full", StringComparison.OrdinalIgnoreCase)) norm = "NearFull";
                norm = norm.ToLower(); // EF can translate ToLower()

                query = query.Where(m => (m.Status ?? "").ToLower() == norm);
            }

            // Map to ViewModel (NO custom methods inside the EF projection)
            var list = query
                .OrderBy(m => m.Code)
                .Select(m => new MachineVm
                {
                    Id = m.Id,
                    Code = m.Code,
                    Address = m.Address,
                    ZoneName = m.Zone.Name,
                    // Inline normalization that EF understands
                    Status = (m.Status == "NearFull") ? "Near Full" : m.Status,
                    FillPercent = m.FillPercent,
                    Lat = m.Lat,
                    Lng = m.Lng
                })
                .ToList();

            return View(list);
        }

        // GET: /Machines/How
        public ActionResult How()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
