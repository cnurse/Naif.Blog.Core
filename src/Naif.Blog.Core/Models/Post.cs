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

            CustomFields = new Dictionary<string, string>();

            Author = String.Empty;
            BlogId = String.Empty;
            Categories = new string[] { };
            Content = String.Empty;
            Excerpt = String.Empty;
            IsPublished = false;
            Keywords = String.Empty;
            LastModified = DateTime.UtcNow;
            Markdown = String.Empty;
            ParentPostId = String.Empty;
            PostType = PostType.Post;
            PubDate = DateTime.UtcNow;
            Slug = String.Empty;
            SubTitle = String.Empty;
            Template = String.Empty;
            Title = String.Empty;
        }
        
        [XmlRpcProperty("postid")]
        public string PostId { get; set; }
        
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

        [Required]
        [XmlRpcProperty("title")]
        public string Title { get; set; }
        
        // - Custom Fields supported by Markdown Monster only
        
        [JsonIgnore]
        public Dictionary<string, string> CustomFields { get; set; }

        public string Author 
        {
            get => GetCustomField("mt_author");
            set => CustomFields["mt_author"] = value;
        }

        public string Markdown
        {
            get => GetCustomField("mt_markdown");
            set => CustomFields["mt_markdown"] = value;
        }

        public string ParentPostId
        {
            get => GetCustomField("mt_parentpostid");
            set => CustomFields["mt_parentpostid"] = value;
        }

        public PostType PostType 
        {
            get => (PostType) Enum.Parse(typeof(PostType), GetCustomField("mt_posttype", "Post"));
            set => CustomFields["mt_posttype"] = value.ToString();
        }
        
        public string PostTypeDetail
        {
            get => GetCustomField("mt_posttypedetail");
            set => CustomFields["mt_posttypedetail"] = value;
        }

        public string RelatedPosts
        {
            get => GetCustomField("mt_related");
            set => CustomFields["mt_related"] = value;
        }

        [JsonIgnore]
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
        }

        public string SubTitle 
        {
            get => GetCustomField("mt_subtitle");
            set => CustomFields["mt_subtitle"] = value;
        }

        [Display(Name="Page Template")]
        public string Template
        {
            get => GetCustomField("mt_template");
            set => CustomFields["mt_template"] = value;
        }

        private string GetCustomField(string field, string defaultValue = "")
        {
            var customField = defaultValue;
            if (CustomFields.ContainsKey(field))
            {
                customField = CustomFields[field];
            }

            return customField;
        }
        
    }
}