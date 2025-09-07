using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GreenCycle.Models.ViewModels
{
    public class RecognitionViewModel
    {
        [Display(Name = "Upload an image")]
        public HttpPostedFileBase Photo { get; set; }   // MVC5 type (NOT IFormFile)

        public string ResultTitle { get; set; }
        public string ResultDetail { get; set; }
        public string Error { get; set; }
    }
}
