using System.Collections.Generic;
using Naif.Blog.Models;
using Naif.Core.Models;

namespace Naif.Blog.Security
{
    public class PostAuthorizationProcessor : IPostAuthorizationProcessor
    {
        private readonly IEnumerable<IPostAuthorize> _authorizors;  
  
        public PostAuthorizationProcessor(IEnumerable<IPostAuthorize> authorizors)  
        {  
            _authorizors = authorizors;  
        }

        public bool CanViewPost(Post post, User user)
        {
            bool canView = true;
            foreach (var authorizor in _authorizors)
            {
                canView = authorizor.CanViewPost(post, user);
                if (!canView)
                {
                    break;
                }
            }

            return canView;
        }
    }
}