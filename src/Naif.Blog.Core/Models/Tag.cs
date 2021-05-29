using System;
using System.Collections.Generic;

namespace Naif.Blog.Models
{
    public class Tag
    {
        public Tag()
        {
            TagId = -1;
            Name = String.Empty;
            Posts = new List<Post>();
        }
        
        public int TagId { get; set; }
        
        public string Name { get; set; }
        
        public int Count { get; set; }
        
        public Blog Blog { get; set; }
        
        public IList<Post> Posts { get; set; }
    }
}