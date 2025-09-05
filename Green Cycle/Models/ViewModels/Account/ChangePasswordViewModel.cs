using System.ComponentModel.DataAnnotations;

namespace Green_Cycle.Models.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required, DataType(DataType.Password), MinLength(6), Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password), Compare("NewPassword"), Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }
    }
}
