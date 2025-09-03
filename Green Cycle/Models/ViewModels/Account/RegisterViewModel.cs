using System.ComponentModel.DataAnnotations;

namespace Green_Cycle.Models.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), MinLength(6)]
        public string Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm password"), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
