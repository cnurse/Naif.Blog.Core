using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Naif.Blog.XmlRpc;
using Newtonsoft.Json;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable StringLiteralTypo

namespace Naif.Blog.Models
{
    public class Post
    {
        public Post()
        {
            PostId = Guid.NewGuid().ToString();
            BlogId = String.Empty;
            Title = String.Empty;
            SubTitle = String.Empty;
            Author = String.Empty;
            Excerpt = String.Empty;
            Content = String.Empty;
            IsPublished = false;
            PubDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
            Slug = String.Empty;

            Categories = new List<Category>();
            Tags = new List<Tag>();
            
            IncludeInLists = true;
            Markdown = String.Empty;
            ParentPostId = String.Empty;
            PostType = PostType.Post;
            PostTypeDetail = String.Empty;
            Template = String.Empty;
        }

        public static readonly Func<Post, bool> SearchPredicate = p => (p.PostType == PostType.Post || p.PostType == PostType.Page)
                                                                       && p.IncludeInLists 
                                                                       && p.IsPublished 
                                                                       && p.PubDate <= DateTime.UtcNow;
        
        public string PostId { get; set; }
        
        public string Title { get; set; }

        public string SubTitle  { get; set; }

        public string Author { get; set; }

        public string Excerpt { get; set; }

        public string Content { get; set; }
        
        public bool IsPublished { get; set; }

        public DateTime PubDate { get; set; }

        public DateTime LastModified { get; set; }

        public string Slug { get; set; }
        
        // - Custom Fields supported by Markdown Monster only
        
        [NotMapped]
        public bool IncludeInLists { get; set; }

        [NotMapped]
        public string Markdown { get; set; }

        [NotMapped]
        public string ParentPostId { get; set; }

        [NotMapped]
        public PostType PostType  { get; set; }
        
        [NotMapped]
        public string PostTypeDetail { get; set; }

        [NotMapped]
        public string Template { get; set; }

        /*[NotMapped]
        public IList<Post> RelatedPosts { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<string> Related
        {
            get
            {
                string relatedPosts = RelatedPosts;
                List<string> posts = new List<string>();
                if (!string.IsNullOrEmpty(relatedPosts))
                {
                    posts.AddRange(relatedPosts.Split(',').Select(post => post.Trim()));
                }
                return posts;
            }
            set => RelatedPosts = string.Join(',', value);
        }*/
        
        //Relationships
        
        public string BlogId { get; set; }

        public Blog Blog { get; set; }
        
        public IList<Category> Categories { get; set; }

        public IList<Tag> Tags { get; set; }
    }
}