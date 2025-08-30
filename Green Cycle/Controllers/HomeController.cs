using System.Collections.Generic;
using System.Web.Mvc;
using Green_Cycle.Models.ViewModels.Home;   // ✅ add this namespace

namespace Green_Cycle.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var vm = new HomeIndexViewModel
            {
                CO2Saved = "157,6 kg",
                ItemsRecycled = "2,463",
                ChallengesJoined = 18,
                Events = new List<EventItem>
                {
                    new EventItem { Title = "Beach Cleanup", Date = "April 22, 2025", Location = "Oceanview Beach", Icon = "map-marker" },
                    new EventItem { Title = "Park Restoration", Date = "May 6, 2025", Location = "Hilltop Park", Icon = "leaf" }
                },
                Posts = new List<PostItem>
                {
                    new PostItem { Title = "Eco-Friendly Gardening", Author = "Emma C.", Date = "Apr 12", Snippet = "Tips for sustainable gardening and reducing waste." },
                    new PostItem { Title = "How to Reduce Food Waste", Author = "Emma C.", Date = "Apr 7", Snippet = "Simple ways to minimize food waste in your daily life." }
                }
            };

            return View(vm);
        }

        public ActionResult Privacy() => View();

        public ActionResult Thanks() => View();
    }
}
