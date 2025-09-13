// Controllers/PostController.cs
using Green_Cycle.Models.Entities;
using GreenCycle.Models.Entities;
using GreenCycle.Models.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GreenCycle.Controllers
{
    public class PostController : Controller
    {
        // Demo store – replace with DbContext
        private static readonly List<Post> _posts = new List<Post> ();

        [HttpGet]
        public ActionResult Create()
        {
            return View(new PostCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PostCreateViewModel vm, HttpPostedFileBase coverImage, string submit)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Save upload (if any)
            string savedPath = vm.ExistingCoverImagePath ?? "";
            if (coverImage != null && coverImage.ContentLength > 0)
            {
                var ext = Path.GetExtension(coverImage.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("", "Invalid image type.");
                    return View(vm);
                }

                var fileName = $"{Guid.NewGuid():N}{ext}";
                var relPath = $"/content/uploads/{fileName}";
                var absPath = Server.MapPath(relPath);
                Directory.CreateDirectory(Path.GetDirectoryName(absPath));
                coverImage.SaveAs(absPath);
                savedPath = relPath;
            }

            var status = (submit ?? "Draft").Equals("Publish", StringComparison.OrdinalIgnoreCase)
                ? PostStatus.Published
                : PostStatus.Draft;

            var post = new Post
            {
                Id = _posts.Count == 0 ? 1 : _posts.Max(p => p.Id) + 1,
                Title = vm.Title,
                Summary = vm.Summary ?? "",
                Body = vm.Body,
                Author = User?.Identity?.Name ?? "Admin",
                CreatedAtUtc = DateTime.UtcNow,
                PublishedAtUtc = status == PostStatus.Published ? (DateTime?)DateTime.UtcNow : null,
                Status = status,
                CoverImagePath = savedPath ?? "",
                CoverImageAlt = vm.CoverImageAlt ?? ""
            };

            _posts.Add(post);

            TempData["toast"] = status == PostStatus.Published ? "Post published." : "Draft saved.";
            return RedirectToAction("Index");
        }

        // existing actions (Index, Details) can read from _posts or your DB
        public ActionResult Index(string search = "")
        {
            // EF
            //var q = db.Posts.Where(p => p.Status == PostStatus.Published);

            var q = _posts.Where(p => p.Status == PostStatus.Published).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                 q = q.Where(p => p.Title.Contains(search)
                              || p.Summary.Contains(search)
                              || p.Body.Contains(search));
            }

            var list = q.OrderByDescending(p => p.PublishedAtUtc ?? p.CreatedAtUtc).ToList();

            var vm = new PostListViewModel
            {
                Search = search,
                Posts = list
            };

            return View(vm);
        }
       
        
        public ActionResult Details(int id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);
            return post == null ? (ActionResult)HttpNotFound() : View(post);
        }
    }
}
