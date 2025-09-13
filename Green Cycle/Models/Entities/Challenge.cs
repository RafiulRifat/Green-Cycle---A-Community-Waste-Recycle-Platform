using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green_Cycle.Models.Entities
{
    
    public enum ChallengeStatus { Current, Upcoming, Past }

    public class Challenge
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int PointsPerScan { get; set; }
        public ChallengeStatus Status { get; set; }
        public bool IsUserJoined { get; set; }   // computed per user in service
        public bool IsActiveWindow =>
            Status == ChallengeStatus.Current && DateTime.UtcNow >= StartUtc && DateTime.UtcNow <= EndUtc;
    }
}
