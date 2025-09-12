using System.Collections.Generic;
using System.Web.Mvc;
using Green_Cycle.Models.ViewModels.Rewards;

namespace Green_Cycle.Controllers
{
    [Authorize]
    public class RewardsController : Controller
    {
        // GET: /Rewards
        public ActionResult Index()
        {
            // stub catalog
            var catalog = new List<RewardVm>
            {
                new RewardVm{ Id=1, Title="Eco-friendly Tote Bag", Subtitle="Made from recycled cotton", CostPoints=50, ImageUrl=null },
                new RewardVm{ Id=2, Title="Reusable Water Bottle", Subtitle="BPA-free, 1 liter", CostPoints=50, ImageUrl=null },
                new RewardVm{ Id=3, Title="Transit Pass", Subtitle="Valid for 30 days", CostPoints=100, ImageUrl=null },
            };

            var redemptions = new List<RedemptionVm>
            {
                new RedemptionVm{ RewardTitle="Eco-friendly Tote Bag", Date="Sep 15, 2025", Status="Pending" },
                new RedemptionVm{ RewardTitle="Reusable Water Bottle", Date="Sep 5, 2025", Status="Approved" },
                new RedemptionVm{ RewardTitle="Transit Pass", Date="Aug 20, 2025", Status="Shipped" },
            };

            ViewBag.Balance = 120;
            ViewBag.Redemptions = redemptions;
            return View(catalog);
        }

        // POST: /Rewards/Redeem/1
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Redeem(int id)
        {
            // TODO: check points + create redemption
            TempData["RedeemMsg"] = "Redemption requested!";
            return RedirectToAction("Index");
        }
    }
}
