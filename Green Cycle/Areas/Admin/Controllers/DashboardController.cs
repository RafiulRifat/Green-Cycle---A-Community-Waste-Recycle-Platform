using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Green_Cycle.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            var vm = new AdminDashboardVm
            {
                TotalPickups = 1254,
                MaterialsDeposits = 980,
                MaterialsCollectedTons = 5.2,
                PointsIssued = 42300,
                MaterialsOverTime = new List<MaterialOverTimeVm>
                {
                    new MaterialOverTimeVm { Month = "Jan", Plastic = 120, Paper = 80, Metal = 50 },
                    new MaterialOverTimeVm { Month = "Feb", Plastic = 140, Paper = 90, Metal = 60 },
                    new MaterialOverTimeVm { Month = "Mar", Plastic = 130, Paper = 85, Metal = 70 },
                    new MaterialOverTimeVm { Month = "Apr", Plastic = 160, Paper = 100, Metal = 75 },
                },
                CollectionByChannel = new Dictionary<string, int>
                {
                    { "Door-to-Door", 350 },
                    { "Machines", 650 }
                },
                RecentActivities = new List<ActivityVm>
                {
                    new ActivityVm { Text = "Pickup completed by Collector X (2.3kg Plastic)", TimeAgo = "1 hour ago" },
                    new ActivityVm { Text = "Machine MAACH-001 jammed", TimeAgo = "2 hours ago" },
                    new ActivityVm { Text = "Reward redeemed: Eco Tote Bag (500 points)", TimeAgo = "5 hours ago" }
                },
                Issues = new List<IssueVm>
                {
                    new IssueVm { Id = 1104, Type = "Pickup", Description = "Missed collection", Status = "Open" },
                    new IssueVm { Id = 1103, Type = "Machine", Description = "MAACH-002 jam", Status = "Open" },
                    new IssueVm { Id = 1102, Type = "Reward", Description = "Invalid coupon", Status = "In Progress" },
                }
            };

            return View(vm);
        }
    }

    // ViewModels
    public class AdminDashboardVm
    {
        public int TotalPickups { get; set; }
        public int MaterialsDeposits { get; set; }
        public double MaterialsCollectedTons { get; set; }
        public int PointsIssued { get; set; }

        public List<MaterialOverTimeVm> MaterialsOverTime { get; set; }
        public Dictionary<string, int> CollectionByChannel { get; set; }
        public List<ActivityVm> RecentActivities { get; set; }
        public List<IssueVm> Issues { get; set; }
    }

    public class MaterialOverTimeVm
    {
        public string Month { get; set; }
        public int Plastic { get; set; }
        public int Paper { get; set; }
        public int Metal { get; set; }
    }

    public class ActivityVm
    {
        public string Text { get; set; }
        public string TimeAgo { get; set; }
    }

    public class IssueVm
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
