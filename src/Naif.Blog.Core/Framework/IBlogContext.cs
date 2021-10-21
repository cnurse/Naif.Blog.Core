using Naif.Blog.Models;
using Naif.Core.Models;

namespace Naif.Blog.Framework
{
    public interface IBlogContext
    {
        Models.Blog Blog { get; set; }
        
        User User { get; set; }
    }
}