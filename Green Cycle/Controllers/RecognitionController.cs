using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using GreenCycle.Models.Entities;          // for MaterialType
using GreenCycle.Models.ViewModels;
using GreenCycle.Services.Interfaces;
using GreenCycle.Services.Implementations;

namespace GreenCycle.Controllers
{
    public class RecognitionController : Controller
    {
        private readonly IRecognitionService _recognitionService;

        public RecognitionController(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }

        public RecognitionController() : this(new RecognitionService()) { }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RecognitionViewModel model)
        {
            // Validate material/weight
            if (!ModelState.IsValid)
                return View(model);

            // Optional image validation
            string photoDataUrl = null;
            if (model.Photo != null && model.Photo.ContentLength > 0)
            {
                // 1) Content type / extension allowlist
                var okTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
                var okExts = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                var contentType = (model.Photo.ContentType ?? "").ToLowerInvariant();
                var ext = Path.GetExtension(model.Photo.FileName ?? "").ToLowerInvariant();

                if (!okTypes.Contains(contentType) || !okExts.Contains(ext))
                {
                    ModelState.AddModelError("Photo", "Please upload a JPG, PNG, GIF, or WEBP image.");
                    return View(model);
                }

                // 2) Size limit (e.g., 5 MB)
                const int maxBytes = 5 * 1024 * 1024;
                if (model.Photo.ContentLength > maxBytes)
                {
                    ModelState.AddModelError("Photo", "Image is too large (max 5 MB).");
                    return View(model);
                }

                // 3) Read and convert to data URL to preview without saving to disk
                using (var ms = new MemoryStream())
                {
                    model.Photo.InputStream.CopyTo(ms);
                    var bytes = ms.ToArray();
                    var base64 = Convert.ToBase64String(bytes);
                    var mime = contentType == "image/jpg" ? "image/jpeg" : contentType;
                    photoDataUrl = $"data:{mime};base64,{base64}";
                }
            }

            // Calculate emissions
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
                Notes = result.Notes,
                PhotoDataUrl = photoDataUrl
            };

            TempData["result"] = resultVm;
            return RedirectToAction("Result");
        }

        [HttpGet]
        public ActionResult Result()
        {
            var vm = TempData["result"] as RecognitionResultViewModel;
            if (vm == null) return RedirectToAction("Index");
            return View(vm);
        }
    }
}
