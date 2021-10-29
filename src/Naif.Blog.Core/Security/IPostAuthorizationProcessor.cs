using Naif.Blog.Models;
using Naif.Core.Models;

namespace Naif.Blog.Security
{
    public interface IPostAuthorizationProcessor
    {
        bool CanViewPost(Post post, User user);
    }
}