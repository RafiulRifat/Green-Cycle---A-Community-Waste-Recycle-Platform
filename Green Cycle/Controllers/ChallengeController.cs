using Green_Cycle.Models.Entities;
using Green_Cycle.Models.ViewModels.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Green_Cycle.Controllers
{
    public class ChallengeController : Controller
    {
        // normally inject a service/repo; mocked here:
        private static readonly Challenge[] _seed = new[]
        {
            new Challenge {
                Id = 1, Title = "Plastic-Free Week",
                StartUtc = new DateTime(2025,8,12), EndUtc = new DateTime(2025,8,18),
                PointsPerScan = 20, Status = ChallengeStatus.Current, IsUserJoined = false
            },
            new Challenge {
                Id = 2, Title = "Summer Recycling Marathon",
                StartUtc = new DateTime(2025,7,1), EndUtc = new DateTime(2025,8,31),
                PointsPerScan = 25, Status = ChallengeStatus.Upcoming, IsUserJoined = false
            },
            new Challenge {
                Id = 3, Title = "E-Waste Collection Drive",
                StartUtc = new DateTime(2025,9,1), EndUtc = new DateTime(2025,9,30),
                PointsPerScan = 15, Status = ChallengeStatus.Upcoming, IsUserJoined = false
            },
        };

        public ActionResult Index(string tab = "current")
        {
            var vm = new ChallengeListViewModel
            {
                Current = _seed.Where(c => c.Status == ChallengeStatus.Current).ToList(),
                Upcoming = _seed.Where(c => c.Status == ChallengeStatus.Upcoming).ToList(),
                Past = _seed.Where(c => c.Status == ChallengeStatus.Past).ToList(),
            };
            ViewBag.Tab = tab.ToLowerInvariant();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(int id)
        {
            var c = _seed.FirstOrDefault(x => x.Id == id);
            if (c != null) c.IsUserJoined = true; // persist with your DB in real app
            TempData["toast"] = $"Joined '{c?.Title}'";
            return RedirectToAction("Index");
        }
    }
}