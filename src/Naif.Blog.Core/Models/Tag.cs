using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Naif.Blog.Models
{
    public class Tag
    {
        public Tag()
        {
            TagId = -1;
            Name = String.Empty;
            Posts = new List<Post>();
            Count = 0;
        }
        
        public int TagId { get; set; }
        
        public string Name { get; set; }
        
        [NotMapped]
        public int Count { get; set; }
        
        //Relationships

        public Blog Blog { get; set; }
        
        public IList<Post> Posts { get; set; }
    }
}