using System.Collections.Generic;
using Green_Cycle.Models.Entities;

namespace Green_Cycle.Models.ViewModels
{
    public class RouteDetailsVm
    {
        public CollectorRoute Route { get; set; }
        public List<RouteStop> Stops { get; set; } = new List<RouteStop>();

        public int Completed => Route?.CompletedStops ?? 0;
        public int Total => Route?.TotalStops ?? 0;
        public int Percent => Total > 0 ? (int)(100.0 * Completed / Total) : 0;
    }
}
