using System.Web.Mvc;
using GreenCycle.Models.Entities;          // for MaterialType
using GreenCycle.Models.ViewModels;
using GreenCycle.Services.Interfaces;
using GreenCycle.Services.Implementations; // for default fallback

namespace GreenCycle.Controllers
{
    public class RecognitionController : Controller
    {
        private readonly IRecognitionService _recognitionService;

        // DI ctor
        public RecognitionController(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }

        // Fallback ctor if you don't have a DI container
        public RecognitionController() : this(new RecognitionService()) { }

        // GET /Recognition
        [HttpGet]
        public ActionResult Index()
        {
            var vm = new RecognitionViewModel
            {
                Material = MaterialType.Plastic,
                WeightKg = 1.0m
            };
            return View(vm);
        }

        // POST /Recognition
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RecognitionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _recognitionService.CalculateEmissions(model.Material, model.WeightKg);

            var resultVm = new RecognitionResultViewModel
            {
                Material = model.Material,
                WeightKg = model.WeightKg,
                FactorMinKgPerKg = result.FactorMinKgPerKg,
                FactorMaxKgPerKg = result.FactorMaxKgPerKg,
                EmissionsMinKg = result.EmissionsMinKg,
                EmissionsMaxKg = result.EmissionsMaxKg,
                EmissionsAvgKg = result.EmissionsAvgKg,
                Notes = result.Notes
            };

            // PRG: stash the VM then redirect to GET /Recognition/Result
            TempData["result"] = resultVm;
            return RedirectToAction("Result");
        }

        // GET /Recognition/Result  <-- this is what fixes the 404
        [HttpGet]
        public ActionResult Result()
        {
            var vm = TempData["result"] as RecognitionResultViewModel;
            if (vm == null)
            {
                // User hit /Recognition/Result directly (no POST first)
                return RedirectToAction("Index");
            }
            return View(vm); // Views/Recognition/Result.cshtml
        }
    }
}
