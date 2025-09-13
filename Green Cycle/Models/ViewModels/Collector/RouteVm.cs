using System;
using System.Collections.Generic;

namespace Green_Cycle.Models.ViewModels.Collector
{
    public class CollectorRouteVm
    {
        public string RouteId { get; set; }
        public string ZoneName { get; set; }          // e.g., "Zone A – Downtown"
        public int TotalStops { get; set; }           // e.g., 12
        public int CompletedStops { get; set; }       // e.g., 0..TotalStops
        public DateTime StartTime { get; set; }       // scheduled start
        public string Status { get; set; }            // NotStarted | InProgress | Completed
        public string Notes { get; set; }             // right-side subtitle (ETA, speed, etc.)
        public double DistanceKm { get; set; }        // optional overall distance
        public DateTime? FinishBy { get; set; }       // optional SLA/finish-by
        public IEnumerable<RouteStopVm> Stops { get; set; } = new List<RouteStopVm>();

        public int RemainingStops => Math.Max(0, TotalStops - CompletedStops);
        public int ProgressPct => TotalStops == 0 ? 0 : (int)Math.Round(100.0 * CompletedStops / TotalStops);
    }

    public class RouteStopVm
    {
        public string Code { get; set; }              // customer / pickup code
        public string Address { get; set; }
        public bool Done { get; set; }
        public DateTime? Eta { get; set; }
    }

    public class CollectorRouteSummaryVm
    {
        public int TotalRoutes { get; set; }
        public int TotalStops { get; set; }
        public int RemainingStops { get; set; }
        public double DistanceKm { get; set; }
        public DateTime? FinishBy { get; set; }

        public IEnumerable<CollectorRouteVm> Routes { get; set; } = new List<CollectorRouteVm>();
        public string Filter { get; set; }            // All | NotStarted | InProgress | Completed
        public string Search { get; set; }
    }
}
