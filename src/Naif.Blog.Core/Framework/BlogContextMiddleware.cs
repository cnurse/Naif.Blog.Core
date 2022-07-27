using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Naif.Blog.Services;
using Naif.Core.Http;
using Naif.Core.Models;
using Newtonsoft.Json.Linq;

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

            var user = context.User;

            if (user != null)
            {
                var emailVerified = user.Claims.FirstOrDefault(c => c.Type == "email_verified")?.Value;
                var largePicture = user.Claims.FirstOrDefault(c => c.Type == "https://schemas.naifblog.com/picture_large")?.Value;
            
                var created =  user.Claims.FirstOrDefault(c => c.Type == "https://schemas.naifblog.com/created_at")?.Value;
                var lastUpdated = user.Claims.FirstOrDefault(c => c.Type == "updated_at")?.Value;

                blogContext.User = new User
                {
                    Name = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                    EmailAddress = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    EmailVerified = (emailVerified != null) && Boolean.Parse(emailVerified),
                    GivenName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                    Identifier = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                    IsAuthenticated = user.Identity.IsAuthenticated,
                    Locale = user.Claims.FirstOrDefault(c => c.Type == "locale")?.Value,
                    NickName = user.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value,
                    ProfileImage = (!String.IsNullOrEmpty(largePicture)) ? largePicture : user.Claims.FirstOrDefault(c => c.Type == "picture")?.Value,
                    Surname = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                    Created = (created == null)  ? DateTime.MinValue : DateTime.Parse(created.Trim('"')),
                    LastUpdated = (lastUpdated == null)  ? DateTime.MinValue : DateTime.Parse(lastUpdated.Trim('"'))
                };

                string metadata = user.Claims.FirstOrDefault(c => c.Type == "https://schemas.naifblog.com/meta_data")?.Value;
                JObject metadataObject = JObject.Parse(metadata);
                blogContext.User.Metadata = metadataObject.ToObject<Dictionary<string, string>>();
                
                foreach (var claim in user.Claims.Where(item => item.Type == "https://schemas.naifblog.com/roles"))
                {
                    blogContext.User.Roles.Add(new Role {Name = claim.Value });
                }
            }
            
            await _next.Invoke(context);
        }
    }
}

