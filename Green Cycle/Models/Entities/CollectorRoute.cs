using System;

namespace Green_Cycle.Models.Entities
{
    public class CollectorRoute
    {
        public int Id { get; set; }
        public string CollectorId { get; set; }   // FK to AspNetUsers
        public string ZoneName { get; set; }
        public int TotalStops { get; set; }
        public int CompletedStops { get; set; }
        public DateTime StartTime { get; set; }
        public string Status { get; set; } // "NotStarted", "InProgress", "Completed"
        public double DistanceKm { get; set; }
        public DateTime? EstimatedFinish { get; set; }

        public virtual ApplicationUser Collector { get; set; }
    }
}
