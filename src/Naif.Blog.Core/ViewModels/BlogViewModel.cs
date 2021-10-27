using System.Collections.Generic;
using Naif.Blog.Models;
using Naif.Core.Models;

namespace Naif.Blog.ViewModels
{
    public class BlogViewModel
    {
        public string BaseUrl { get; set; }
        
        public Naif.Blog.Models.Blog Blog { get; set; }
        
        public IList<Message> Messages { get; set; }
        
        public int PageIndex { get; set; }
        
        public IEnumerable<Post> Pages { get; set; }

        public Post Post { get; set; }

        public IEnumerable<Post> Posts { get; set; }
        
        public string ReturnUrl { get; set; }
        
        public User User { get; set; }
    }
}