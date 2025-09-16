using System.ComponentModel.DataAnnotations;

namespace Green_Cycle.Models.ViewModels.Account
{
    public class CustomerProfileViewModel
    {
        [Required, Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        // Read-only display of total recognitions
        public int RecognitionCount { get; set; }
    }
}
