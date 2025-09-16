using System.ComponentModel.DataAnnotations;
using System.Web; // for HttpPostedFileBase
using GreenCycle.Models.Entities;

namespace GreenCycle.Models.ViewModels
{
    public class RecognitionViewModel
    {
        [Required]
        [Display(Name = "Material")]
        public MaterialType Material { get; set; }

        [Required]
        [Range(typeof(decimal), "0.1", "20", ErrorMessage = "Weight must be between 0.1 and 20 kg.")]
        [Display(Name = "Weight (kg)")]
        public decimal WeightKg { get; set; }

        [Display(Name = "Upload a photo (optional)")]
        public HttpPostedFileBase Photo { get; set; }
    }
}
