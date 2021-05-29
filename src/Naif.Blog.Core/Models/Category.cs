using System;
using System.Collections.Generic;

namespace Naif.Blog.Models
{
    public class Category
    {
        public Category()
        {
            CategoryId = -1;
            ParentCategoryId = null;
            Name = String.Empty;
            Posts = new List<Post>();
            Count = 0;
        }
        
        public int CategoryId { get; set; }
        
        public int? ParentCategoryId { get; set; }
        
        public string Name { get; set; }
        
        public int Count { get; set; }
        
        public Blog Blog { get; set; }

        public IList<Post> Posts { get; set; }
    }
}