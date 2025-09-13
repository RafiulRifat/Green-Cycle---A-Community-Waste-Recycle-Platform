// Models/Entities/Post.cs
using System;

namespace GreenCycle.Models.Entities
{
    public enum PostStatus { Draft, Published }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Body { get; set; } = "";
        public string Author { get; set; } = "";
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? PublishedAtUtc { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Draft;

        public string CoverImagePath { get; set; } = "";   // e.g. /content/uploads/abc.jpg
        public string CoverImageAlt { get; set; } = "";
    }
}
