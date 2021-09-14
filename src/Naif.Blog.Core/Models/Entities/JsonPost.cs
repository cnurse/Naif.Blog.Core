using System;

namespace Naif.Blog.Models.Entities
{
    internal class JsonPost
    {
        public JsonPost()
        {
            Categories = Array.Empty<string>();
            IncludeInLists = true;
        }
        
        public string PostId { get; set; }
        
        public string BlogId { get; set; }

        public string[] Categories { get; set; }

        public string Content { get; set; }
        
        public string Excerpt { get; set; }

        public bool IsPublished { get; set; }

        public string Keywords { get; set; }

        public DateTime LastModified { get; set; }

        public DateTime PubDate { get; set; }

        public string Slug { get; set; }
        
        public string Title { get; set; }
        
        // - Custom Fields supported by Markdown Monster only
        
        public string Author  { get; set; }
        
        public bool IncludeInLists { get; set; }

        public string Markdown { get; set; }
        
        public int PageOrder { get; set; }

        public string ParentPostId { get; set; }

        public PostType PostType  { get; set; }
        
        public string PostTypeDetail  { get; set; }

        public string RelatedPosts  { get; set; }

        public string SubTitle  { get; set; }

        public string Template  { get; set; }
    }
}