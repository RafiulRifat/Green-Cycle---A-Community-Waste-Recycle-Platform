using System.Collections.Generic;
using System.Web.Mvc;
using Green_Cycle.Models.ViewModels.Home;   // ViewModels live under Green_Cycle.Models.*

namespace GreenCycle.Controllers          // Controllers in your project use GreenCycle.Controllers
{
    public class HomeController : Controller
    {
        // GET: / or /Home/Index
        [AllowAnonymous]
        public ActionResult Index()
        {
            // Basic sample data so the Index view can render safely
            var vm = new HomeIndexViewModel
            {
                CO2Saved = "157.6 kg",
                ItemsRecycled = "2,463",
                ChallengesJoined = 18,
                Events = new List<EventItem>
                {
                    new EventItem { Title = "Beach Cleanup",     Date = "Apr 22, 2025", Location = "Oceanview Beach", Icon = "map-marker" },
                    new EventItem { Title = "Park Restoration",   Date = "May 06, 2025", Location = "Hilltop Park",    Icon = "leaf" }
                },
                Posts = new List<PostItem>
                {
                    new PostItem { Title = "Eco-Friendly Gardening",   Author = "Emma C.", Date = "Apr 12", Snippet = "Tips for sustainable gardening and reducing waste." },
                    new PostItem { Title = "How to Reduce Food Waste", Author = "Emma C.", Date = "Apr 07", Snippet = "Simple ways to minimize food waste in your daily life." }
                }
            };

            // Will resolve to Views/Home/Index.cshtml
            return View("Index", vm);
        }

        // GET: /Home/Privacy
        [AllowAnonymous]
        public ActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/Thanks
        [AllowAnonymous]
        public ActionResult Thanks()
        {
            return View();
        }
    }
}
