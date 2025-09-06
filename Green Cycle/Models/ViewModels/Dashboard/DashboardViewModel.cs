using System;
using System.Collections.Generic;

namespace GreenCycle.Models.ViewModels
{
    public class DashboardViewModel
    {
        public double TotalCO2SavedKg { get; set; }
        public int ItemsDiverted { get; set; }
        public int ChallengesJoined { get; set; }

        public List<WeeklyPoint> WeeklyCO2 { get; set; } = new List<WeeklyPoint>();
        public List<ActivityItem> Recent { get; set; } = new List<ActivityItem>();
    }

    public class WeeklyPoint
    {
        public string WeekLabel { get; set; }  // e.g., "Jun 23"
        public double Kg { get; set; }         // value in kg
    }

    public class ActivityItem
    {
        public DateTime Date { get; set; }
        public string Label { get; set; }      // e.g., "Plastic Bottle"
        public string Material { get; set; }   // e.g., "Plastic"
        public double SavedCO2Kg { get; set; } // e.g., 0.08
    }
}
