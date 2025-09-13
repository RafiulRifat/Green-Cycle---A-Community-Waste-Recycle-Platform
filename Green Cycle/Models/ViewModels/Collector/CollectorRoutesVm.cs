using System;
using System.Collections.Generic;
using System.Linq;
using Green_Cycle.Models.Entities;

namespace Green_Cycle.Models.ViewModels
{
    public class CollectorRoutesVm
    {
        public List<CollectorRoute> Routes { get; set; } = new List<CollectorRoute>();
        public int TotalRoutes => Routes?.Count ?? 0;
        public int TotalStops => Routes?.Sum(r => r.TotalStops) ?? 0;
        public int RemainingStops => Routes?.Sum(r => r.TotalStops - r.CompletedStops) ?? 0;
        public double TotalDistance => Routes?.Sum(r => r.DistanceKm) ?? 0;
        public DateTime? FinishBy => Routes?.Where(r => r.EstimatedFinish.HasValue).Max(r => r.EstimatedFinish);
    }
}
