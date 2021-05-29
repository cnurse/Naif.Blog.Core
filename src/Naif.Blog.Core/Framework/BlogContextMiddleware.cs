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
        private readonly IBlogRepository _blogRepository;

        public BlogContextMiddleware(RequestDelegate next, 
                                    IBlogRepository blogRepository)
        {
            _next = next;
            _blogRepository = blogRepository;
        }

        public async Task InvokeAsync(HttpContext context, IBlogContext blogContext)
        {
            blogContext.Blogs = _blogRepository.GetBlogs();
            if (context.Request.IsLocal())
            {
                blogContext.CurrentBlog = blogContext.Blogs.SingleOrDefault(b => b.LocalUrl == context.Request.Host.Value);
            }
            else
            {
                blogContext.CurrentBlog = blogContext.Blogs.SingleOrDefault(b => b.Url == context.Request.Host.Value);
            }

            //Eagerly load the Categories and Tags
            string blogId = blogContext.CurrentBlog.Id;
            blogContext.CurrentBlog.Categories = _blogRepository.GetCategories(blogId);
            blogContext.CurrentBlog.Tags = _blogRepository.GetTags(blogId);

            await _next.Invoke(context);
        }
    }
}

