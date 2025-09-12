namespace Green_Cycle.Models.ViewModels.Issues
{
    public class IssueVm
    {
        public string IssueType { get; set; }              // Pickup | Machine | Redemption | Other
        public string ReferenceId { get; set; }            // e.g., pickup id or machine code
        public string Description { get; set; }
        public string Priority { get; set; }               // Low | Medium | High
        public bool NotifyByPhone { get; set; }
        public bool NotifyInApp { get; set; }
    }
}
