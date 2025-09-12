using System.Data.Entity;
using Green_Cycle.Models.Entities.Hybrid;

namespace Green_Cycle.Data
{
    public class GreenCycleContext : DbContext
    {
        public GreenCycleContext() : base("GreenCycleContext") { }

        // Catalog & geography
        public DbSet<Material> Materials { get; set; }        // if you have Material; ok to add later
        public DbSet<ServiceZone> ServiceZones { get; set; }

        // Machines
        public DbSet<Machine> Machines { get; set; }

        // (more DbSets later as you build features)
    }
}
