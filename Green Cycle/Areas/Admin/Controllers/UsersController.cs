using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Green_Cycle.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Green_Cycle.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
        }

        // GET: Admin/Users
        public ActionResult Index(string q = null, string role = null, string status = null)
        {
            // Ensure roles exist (idempotent)
            EnsureRole("Admin");
            EnsureRole("Collector");
            EnsureRole("User");

            var users = _db.Users.ToList();

            // role id -> name lookup
            var roleMap = _db.Roles.ToDictionary(r => r.Id, r => r.Name);

            var items = users.Select(u => new UsersListItemVm
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Roles = u.Roles.Select(r => roleMap.ContainsKey(r.RoleId) ? roleMap[r.RoleId] : r.RoleId).ToList()
            });

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                items = items.Where(u =>
                    (u.Email ?? "").ToLower().Contains(q.ToLower()) ||
                    (u.FullName ?? "").ToLower().Contains(q.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                items = items.Where(u => u.Roles.Contains(role));
            }

            var vm = new UsersIndexVm
            {
                Query = q,
                Role = role,
                Users = items.OrderBy(u => u.Email).ToList()
            };

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCollector(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction(nameof(Index));

            EnsureRole("Collector");
            await _userManager.AddToRoleAsync(id, "Collector");
            TempData["ok"] = "Collector role added.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveCollector(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction(nameof(Index));

            EnsureRole("Collector");
            await _userManager.RemoveFromRoleAsync(id, "Collector");
            TempData["ok"] = "Collector role removed.";
            return RedirectToAction(nameof(Index));
        }

        private void EnsureRole(string roleName)
        {
            if (!_roleManager.RoleExists(roleName))
            {
                _roleManager.Create(new IdentityRole(roleName));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _roleManager?.Dispose();
                _userManager?.Dispose();
                _db?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    // ----- View Models -----
    public class UsersIndexVm
    {
        public string Query { get; set; }
        public string Role { get; set; }
        public List<UsersListItemVm> Users { get; set; }
    }

    public class UsersListItemVm
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
    }
}
