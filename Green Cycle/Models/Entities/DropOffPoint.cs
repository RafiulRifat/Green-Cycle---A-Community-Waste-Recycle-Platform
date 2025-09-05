using System.ComponentModel.DataAnnotations;

namespace Green_Cycle.Models.Entities
{
    public class DropOffPoint
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required, StringLength(300)]
        public string Address { get; set; }

        // Optional geo; range-validated in VM
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Comma-separated list (e.g. "Plastic,Paper,Glass")
        [StringLength(400)]
        public string MaterialsAccepted { get; set; }

        // Kept from index page (optional)
        [Display(Name = "Distance (km)")]
        public double DistanceKm { get; set; }
    }
}
