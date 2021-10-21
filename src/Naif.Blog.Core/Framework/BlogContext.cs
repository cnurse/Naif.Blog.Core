using Naif.Blog.Models;
using Naif.Core.Models;

namespace Naif.Blog.Framework
{
    public class BlogContext : IBlogContext
    {
        public Models.Blog Blog { get; set; }
        public User User { get; set; }
    }
}
