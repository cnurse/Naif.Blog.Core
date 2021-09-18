using Naif.Auth0.Models;

namespace Naif.Blog.Framework
{
    public interface IBlogContext
    {
        Models.Blog Blog { get; set; }
        
        Profile User { get; set; }
    }
}