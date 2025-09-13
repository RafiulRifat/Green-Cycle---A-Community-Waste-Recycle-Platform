// Models/Entities/RouteStop.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Green_Cycle.Models.Entities
{
    public class RouteStop
    {
        public int Id { get; set; }

        [Required]
        public int RouteId { get; set; }

        [ForeignKey("RouteId")]
        public CollectorRoute Route { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Address { get; set; }

        public int Order { get; set; }

        public DateTime? ScheduledTime { get; set; }

        [MaxLength(32)]
        public string Status { get; set; } = "Pending"; // Pending / Completed

        public DateTime? CompletedAt { get; set; }
    }
}
