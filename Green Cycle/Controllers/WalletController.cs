using System.Collections.Generic;
using System.Web.Mvc;
using Green_Cycle.Models.ViewModels.Wallet;

namespace Green_Cycle.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        // GET: /Wallet
        public ActionResult Index()
        {
            var vm = new WalletVm
            {
                Balance = 120,
                Transactions = new List<TransactionVm>
                {
                    new TransactionVm{ Icon="bi-truck",   Title="Pickup Completed – 3 kg Plastic", PointsText="+20 pts", Tab="Earned"   },
                    new TransactionVm{ Icon="bi-geo-alt", Title="Deposit at MACH-002",           PointsText="+20 pts", Tab="Earned"   },
                    new TransactionVm{ Icon="bi-percent", Title="Reward Redemption",            PointsText="−50 pts", Tab="Redeemed" },
                    new TransactionVm{ Icon="bi-gear",    Title="Points Adjustment",           PointsText="+10 pts", Tab="Earned"   },
                }
            };
            return View(vm);
        }
    }
}
