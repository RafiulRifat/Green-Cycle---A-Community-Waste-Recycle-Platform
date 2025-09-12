using System;

namespace Green_Cycle.Models.ViewModels.Machines
{
    /// <summary>
    /// Read-only data used by Views/Machines/Nearby.cshtml
    /// </summary>
    public class MachineVm
    {
        public int Id { get; set; }

        // Header
        public string Code { get; set; }           // e.g. MACH-001

        // Secondary line (show Address if present, else "Zone: <ZoneName>")
        public string Address { get; set; }        // e.g. 123 Main St (optional)
        public string ZoneName { get; set; }       // e.g. Downtown

        // Right-side chips
        public string Status { get; set; }         // "Available" | "Near Full" | "Full" | "Cancelled"
        public int FillPercent { get; set; }       // 0–100

        // NEW: for map display
        public double? Lat { get; set; }           // Latitude (nullable in case missing)
        public double? Lng { get; set; }           // Longitude (nullable in case missing)
    }
}
