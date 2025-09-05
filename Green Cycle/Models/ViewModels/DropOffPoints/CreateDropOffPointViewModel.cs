using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Green_Cycle.Models.ViewModels.DropOffPoints
{
    public class CreateDropOffPointViewModel
    {
        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required, StringLength(300)]
        public string Address { get; set; }

        [Display(Name = "Latitude")]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double? Latitude { get; set; }

        [Display(Name = "Longitude")]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double? Longitude { get; set; }

        // chips
        public List<string> SelectedMaterials { get; set; } = new List<string>();

        // supplied by controller for rendering chips
        public List<string> AvailableMaterials { get; set; } = new List<string>
        { "Plastic", "Paper", "Glass", "Metal", "Organic", "E-Waste" };

        // show success banner
        public bool Created { get; set; }
    }
}
