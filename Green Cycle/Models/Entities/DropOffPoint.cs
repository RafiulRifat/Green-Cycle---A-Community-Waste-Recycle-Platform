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

        [Display(Name = "Distance (km)")]
        public double DistanceKm { get; set; }
    }
}
