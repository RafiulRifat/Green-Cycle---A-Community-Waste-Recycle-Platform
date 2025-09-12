using System.Collections.Generic;

namespace Green_Cycle.Models.Entities.Hybrid
{
    public class ServiceZone
    {
        public int Id { get; set; }
        public string Name { get; set; }            // e.g., Downtown
        public string GeoJson { get; set; }         // optional
        public bool IsActive { get; set; }

        public virtual ICollection<Machine> Machines { get; set; }
    }
}
