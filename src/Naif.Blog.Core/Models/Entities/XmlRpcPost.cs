using System;
using System.Collections.Generic;
using Naif.Blog.XmlRpc;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Naif.Blog.Models.Entities
{
    internal class XmlRpcPost
    {
        public XmlRpcPost()
        {
            CustomFields = new Dictionary<string, string>();
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

        [XmlRpcProperty("mt_keywords")]
        public string Keywords { get; set; }

        [XmlRpcProperty("dateModified")]
        public DateTime LastModified { get; set; }

        [XmlRpcProperty("dateCreated")]
        public DateTime PubDate { get; set; }

        [XmlRpcProperty("wp_slug")]
        public string Slug { get; set; }
        
        [XmlRpcProperty("title")]
        public string Title { get; set; }
        
        // - Custom Fields supported by Markdown Monster only
        
        public Dictionary<string, string> CustomFields { get; set; }
    }
}