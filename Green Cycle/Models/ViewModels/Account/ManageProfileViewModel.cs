using System;
using System.ComponentModel.DataAnnotations;

namespace Green_Cycle.Models.ViewModels.Account
{
    public class ManageProfileViewModel
    {
        [Display(Name = "Name")]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Joined")]
        public DateTime JoinedOn { get; set; }

        public bool EmailConfirmed { get; set; }

        [Display(Name = "Enable 2FA")]
        public bool TwoFactorEnabled { get; set; }
    }
}
