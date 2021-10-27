using System;
using Naif.Blog.Models;

namespace Naif.Blog.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            ParentPostId = String.Empty;
        }

        public string PostId { get; set; }

        public string BlogId { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Author { get; set; }

        public string Excerpt { get; set; }

        public string Content { get; set; }

        public string Slug { get; set; }

        public bool IncludeInLists { get; set; }

        public bool IsPublished { get; set; }

        public string Categories { get; set; }

        public string Tags { get; set; }

        public PostType PostType { get; set; }

        public string ParentPostId { get; set; }

        public int PageOrder { get; set; }
    }
}
