using System.Collections.Generic;

namespace Green_Cycle.Models.ViewModels.Wallet
{
    public class WalletVm
    {
        public int Balance { get; set; }
        public IList<TransactionVm> Transactions { get; set; } = new List<TransactionVm>();
    }

    public class TransactionVm
    {
        public string Icon { get; set; }       // bootstrap icon name e.g., "bi-truck"
        public string Title { get; set; }      // "Pickup Completed – 3 kg Plastic"
        public string PointsText { get; set; } // "+20 pts" or "-50 pts"
        public string Tab { get; set; }        // "All" | "Earned" | "Redeemed"
    }
}
