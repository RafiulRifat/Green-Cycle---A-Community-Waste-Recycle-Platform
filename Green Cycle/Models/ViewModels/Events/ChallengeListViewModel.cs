using Green_Cycle.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green_Cycle.Models.ViewModels.Events
{
    public class ChallengeListViewModel
    {
        public IReadOnlyList<Challenge> Current { get; set; } = new List<Challenge>();
        public IReadOnlyList<Challenge> Upcoming { get; set; } = new List<Challenge>();
        public IReadOnlyList<Challenge> Past { get; set; } = new List<Challenge>();
    }
}