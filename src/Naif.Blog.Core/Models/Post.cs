using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Naif.Blog.XmlRpc;
using Newtonsoft.Json;

namespace Naif.Blog.Models
{
    public class Post
    {
        public Post()
        {
            PostId = Guid.NewGuid().ToString();

            Author = String.Empty;
            BlogId = String.Empty;
            Categories = new string[] { };
            Content = String.Empty;
            Excerpt = String.Empty;
            IsPublished = false;
            Keywords = String.Empty;
            LastModified = DateTime.UtcNow;
            ParentPostId = String.Empty;
            PostType = PostType.Post;
            PubDate = DateTime.UtcNow;
            Slug = String.Empty;
            Template = String.Empty;
            Title = String.Empty;
        }
        
        [XmlRpcProperty("postid")]
        public string PostId { get; set; }
        
        [XmlRpcProperty("author")]
        public string Author { get; set; }
        
        [XmlRpcProperty("blogId")]
        public string BlogId { get; set; }

        [XmlRpcProperty("categories")]
        public string[] Categories { get; set; }

        [XmlRpcProperty("description")]
        public string Content { get; set; }
        
        [XmlRpcProperty("mt_excerpt")]
        public string Excerpt { get; set; }

        public bool IsPublished { get; set; }

        [XmlRpcProperty("mt_keywords")]
        public string Keywords { get; set; }

        [XmlRpcProperty("dateModified")]
        public DateTime LastModified { get; set; }

        [XmlRpcProperty("mt_parent")]
        public string ParentPostId { get; set; }

        [XmlRpcProperty("mt_posttype")]
        public PostType PostType { get; set; }

        [XmlRpcProperty("dateCreated")]
        public DateTime PubDate { get; set; }

        [XmlRpcProperty("wp_slug")]
        public string Slug { get; set; }
        
        [JsonIgnore]
        public List<string> Tags
        {
            get
            {
                List<string> tags = new List<string>();
                if (!string.IsNullOrEmpty(Keywords))
                {
                    tags.AddRange(Keywords.Split(',').Select(tag => tag.Trim()));
                }
                return tags;
            }
        }

        [Display(Name="Page Template")]
        public string Template { get; set; }

        [Required]
        [XmlRpcProperty("title")]
        public string Title { get; set; }
    }
}