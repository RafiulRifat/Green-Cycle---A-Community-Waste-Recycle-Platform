using System.ComponentModel.DataAnnotations;

namespace Green_Cycle.Models.Entities.Hybrid
{
    public class Material
    {
        [Key]                    // not strictly needed if property is named "Id", but safe
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }   // e.g., Plastic, Paper

        public bool IsActive { get; set; }
    }
}
