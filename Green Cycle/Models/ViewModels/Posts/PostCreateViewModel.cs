// Models/ViewModels/Posts/PostCreateViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace GreenCycle.Models.ViewModels.Posts
{
    public class PostCreateViewModel
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        [StringLength(240)]
        public string Summary { get; set; }

        [Required]
        public string Body { get; set; }

        // Uploads
        public string ExistingCoverImagePath { get; set; }
        public string CoverImageAlt { get; set; }
    }
}
