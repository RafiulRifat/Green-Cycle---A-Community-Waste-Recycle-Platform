namespace Green_Cycle.Models.ViewModels.Rewards
{
    public class RewardVm
    {
        public int Id { get; set; }
        public string Title { get; set; }      // e.g., "Eco-friendly Tote Bag"
        public string Subtitle { get; set; }   // e.g., "Made from recycled cotton"
        public int CostPoints { get; set; }
        public string ImageUrl { get; set; }   // optional placeholder
    }

    public class RedemptionVm
    {
        public string RewardTitle { get; set; }
        public string Date { get; set; }       // show friendly date
        public string Status { get; set; }     // Pending | Approved | Shipped | Rejected
    }
}
