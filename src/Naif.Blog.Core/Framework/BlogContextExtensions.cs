using Microsoft.AspNetCore.Builder;

namespace Naif.Blog.Framework
{
    public static class BlogContextExtensions
    {
        public static IApplicationBuilder UseBlogContext(this IApplicationBuilder app)
        {
            return app.UseMiddleware<BlogContextMiddleware>();
        }
    }
}