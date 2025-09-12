using System.Linq;
using System.Data.Entity.Migrations;
using Green_Cycle.Models.Entities.Hybrid;

namespace Green_Cycle.Data
{
    public static class DbInitializer
    {
        public static void Run(GreenCycleContext db)
        {
            // Seed service zones
            db.ServiceZones.AddOrUpdate(z => z.Name,
                new ServiceZone { Name = "Downtown", IsActive = true },
                new ServiceZone { Name = "North", IsActive = true }
            );
            db.SaveChanges();

            var downtownId = db.ServiceZones.Single(z => z.Name == "Downtown").Id;
            var northId = db.ServiceZones.Single(z => z.Name == "North").Id;

            // Seed machines with coordinates
            db.Machines.AddOrUpdate(m => m.Code,
                new Machine
                {
                    Code = "MACH-001",
                    Address = "123 Main St",
                    ZoneId = downtownId,
                    Status = "Available",
                    FillPercent = 40,
                    Lat = 23.7806,   // sample coords
                    Lng = 90.4070
                },
                new Machine
                {
                    Code = "MACH-002",
                    Address = "456 Elm St",
                    ZoneId = downtownId,
                    Status = "Available",
                    FillPercent = 20,
                    Lat = 23.7925,
                    Lng = 90.4043
                },
                new Machine
                {
                    Code = "MACH-003",
                    Address = "789 Pine St",
                    ZoneId = downtownId,
                    Status = "NearFull",
                    FillPercent = 85,
                    Lat = 23.7762,
                    Lng = 90.4150
                },
                new Machine
                {
                    Code = "MACH-004",
                    Address = "101 Oak St",
                    ZoneId = northId,
                    Status = "Full",
                    FillPercent = 100,
                    Lat = 23.8020,
                    Lng = 90.4185
                }
            );
            db.SaveChanges();
        }
    }
}
