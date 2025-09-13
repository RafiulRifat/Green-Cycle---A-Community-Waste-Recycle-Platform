// Models/ViewModels/Posts/PostListViewModel.cs
using System.Collections.Generic;
using GreenCycle.Models.Entities;

namespace GreenCycle.Models.ViewModels.Posts
{
    public class PostListViewModel
    {
        // For search/filter input (optional)
        public string Search { get; set; } = "";

        // All posts to display on the page
        public IReadOnlyList<Post> Posts { get; set; } = new List<Post>();
    }
}
