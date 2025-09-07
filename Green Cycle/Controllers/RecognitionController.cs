using GreenCycle.Models.ViewModels;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenCycle.Controllers
{
    public class RecognitionController : Controller
    {
        private static readonly string[] AllowedContentTypes = { "image/jpeg", "image/png" };
        private const int MaxBytes = 5 * 1024 * 1024;   // 5 MB
        private const int MinWidth = 256;
        private const int MinHeight = 256;
        private const string UploadFolder = "~/Content/uploads";

        [HttpGet]
        public ActionResult Index()
        {
            return View(new RecognitionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RecognitionViewModel model)
        {
            var file = model.Photo; // must match input name="Photo"

            // Presence
            if (file == null || file.ContentLength == 0)
            {
                model.Error = "Please choose a JPG or PNG image.";
                return View(model);
            }

            // Type / size
            if (!AllowedContentTypes.Contains(file.ContentType))
            {
                model.Error = "Only JPG and PNG are supported.";
                return View(model);
            }
            if (file.ContentLength > MaxBytes)
            {
                model.Error = "Max file size is 5 MB.";
                return View(model);
            }

            // Ensure folder exists
            var physicalUploads = Server.MapPath(UploadFolder);
            Directory.CreateDirectory(physicalUploads);

            // Sanitize: re-encode without metadata (EXIF/GPS) by redrawing
            var ext = file.ContentType == "image/png" ? ".png" : ".jpg";
            var safeBase = Path.GetFileNameWithoutExtension(file.FileName);
            var fileName = safeBase + "-" + Guid.NewGuid().ToString("N") + ext;
            var physicalPath = Path.Combine(physicalUploads, fileName);
            var virtualPath = VirtualPathUtility.ToAbsolute(UploadFolder + "/" + fileName);

            try
            {
                using (var src = Image.FromStream(file.InputStream))
                {
                    if (src.Width < MinWidth || src.Height < MinHeight)
                    {
                        model.Error = "Image must be at least 256 × 256 pixels.";
                        return View(model);
                    }

                    using (var bmp = new Bitmap(src.Width, src.Height))
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.DrawImage(src, 0, 0, src.Width, src.Height);

                        if (ext == ".png")
                        {
                            bmp.Save(physicalPath, ImageFormat.Png);
                        }
                        else
                        {
                            var enc = ImageCodecInfo.GetImageDecoders()
                                .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                            var p = new EncoderParameters(1);
                            p.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                            bmp.Save(physicalPath, enc, p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.Error = "That file doesn’t look like a valid image. " + ex.Message;
                return View(model);
            }

            // Fake analysis for now (replace with your service later)
            var result = new RecognitionResultViewModel
            {
                Label = "Plastic Bottle",
                Confidence = 0.92,
                Material = "Plastic",
                SavedCO2 = 0.08,
                Instructions = new[]
                {
                    "Rinse the bottle.",
                    "Remove the cap and recycle separately."
                },
                ScannedAt = DateTime.Now,
                PreviewImagePath = virtualPath
            };

            TempData["RecognitionResult"] = result; // pass to Result page
            return RedirectToAction("Result");
        }

        [HttpGet]
        public ActionResult Result()
        {
            var model = TempData["RecognitionResult"] as RecognitionResultViewModel;
            if (model == null) return RedirectToAction("Index");
            return View(model);
        }
    }
}
