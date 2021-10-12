using Naif.Blog.Models;

namespace Naif.Blog.Framework
{
    public class BlogContext : IBlogContext
    {
        public Models.Blog Blog { get; set; }
        public Profile User { get; set; }
    }
}
