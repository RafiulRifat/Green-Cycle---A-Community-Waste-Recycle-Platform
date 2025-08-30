using System.Collections.Generic;

namespace Green_Cycle.Models.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public string CO2Saved { get; set; }
        public string ItemsRecycled { get; set; }
        public int ChallengesJoined { get; set; }

        public List<EventItem> Events { get; set; }
        public List<PostItem> Posts { get; set; }
    }

    public class EventItem
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string Icon { get; set; }
    }

    public class PostItem
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Snippet { get; set; }
    }
}
