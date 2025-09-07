using System;
using System.Web.Mvc;
using GreenCycle.Models.ViewModels;
using System.Collections.Generic;

namespace GreenCycle.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public ActionResult Impact()
        {
            // demo data to match your UI
            var vm = new DashboardViewModel
            {
                TotalCO2SavedKg = 31.6,
                ItemsDiverted = 510,
                ChallengesJoined = 6,
                WeeklyCO2 = new List<WeeklyPoint>
                {
                    new WeeklyPoint{ WeekLabel="Jun 23", Kg=2.8 },
                    new WeeklyPoint{ WeekLabel="Jul 7",  Kg=3.3 },
                    new WeeklyPoint{ WeekLabel="Jul 14", Kg=3.6 },
                    new WeeklyPoint{ WeekLabel="Jul 21", Kg=3.2 },
                    new WeeklyPoint{ WeekLabel="Jul 28", Kg=5.4 },
                    new WeeklyPoint{ WeekLabel="Aug 1",  Kg=1.7 },
                },
                Recent = new List<ActivityItem>
                {
                    new ActivityItem{ Date=new DateTime(2025,8,10), Label="Plastic Bottle", Material="Plastic", SavedCO2Kg=0.08 },
                    new ActivityItem{ Date=new DateTime(2025,8,7),  Label="Cardboard Box", Material="Paper",   SavedCO2Kg=0.04 },
                    new ActivityItem{ Date=new DateTime(2025,8,6),  Label="Glass Jar",     Material="Glass",   SavedCO2Kg=0.02 },
                    new ActivityItem{ Date=new DateTime(2025,8,3),  Label="Aluminum Can",  Material="Metal",   SavedCO2Kg=0.12 },
                    new ActivityItem{ Date=new DateTime(2025,8,2),  Label="Plastic Bag",   Material="Plastic", SavedCO2Kg=0.08 },
                }
            };

            return View(vm); // Views/Dashboard/Impact.cshtml
        }
    }
}
