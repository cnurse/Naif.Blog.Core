using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Naif.Blog.Services;
using Naif.Core.Http;

namespace Naif.Blog.Framework
{
    /// <summary>
    /// The BlogContextMiddleware component processes the request to build the BlogContext object used
    /// for the duration of the request.  
    /// The IBlogContext object is passed in by Dependency Injection with a "scoped" lifetime, ensuring that a new object
    /// is created for each request.
    /// </summary>
    public class BlogContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBlogManager _blogManager;

        public BlogContextMiddleware(RequestDelegate next, 
                                    IBlogManager blogManager)
        {
            _next = next;
            _blogManager = blogManager;
        }

        public async Task InvokeAsync(HttpContext context, IBlogContext blogContext)
        {
            if (context.Request.IsLocal())
            {
                blogContext.Blog = _blogManager.GetBlog(b => b.LocalUrl == context.Request.Host.Value, true);
            }
            else
            {
                blogContext.Blog = _blogManager.GetBlog(b => b.Url == context.Request.Host.Value, true);
            }

            await _next.Invoke(context);
        }
    }
}

