using Naif.Blog.Models;

namespace Naif.Blog.Framework
{
    public interface IBlogContext
    {
        Models.Blog Blog { get; set; }
        
        Profile User { get; set; }
    }
}