using Microsoft.AspNetCore.Mvc;
using Naif.Blog.Framework;
using Naif.Blog.Models;
using Naif.Blog.Services;
using Naif.Blog.XmlRpc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Naif.Blog.Models.Entities;
using Naif.Blog.Models.Extensions;
using Naif.Blog.Security;
// ReSharper disable UnusedParameter.Local
// ReSharper disable PossibleNullReferenceException
// ReSharper disable ExpressionIsAlwaysNull

namespace Naif.Blog.Controllers
{
    [Route("MetaWeblog")]
    public class MetaWeblogController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly XmlRpcSecurityOptions _securityOptions;
        private readonly IBlogManager _blogManager;
        private readonly IBlogContext _blogContext;
        
        public MetaWeblogController(IWebHostEnvironment environment,
            IBlogContext blogContext,
            IBlogManager blogManager,
            IOptions<XmlRpcSecurityOptions> optionsAccessor) 
        {
            _environment = environment;
            _securityOptions = optionsAccessor.Value;
            _blogContext = blogContext;
            _blogManager = blogManager;
        }

        public IActionResult Index()
        {
            var methodName = ControllerContext.HttpContext.Items["Xml-Rpc-MethodName"] as string;
            var methodParams = ControllerContext.HttpContext.Items["Xml-Rpc-Parameters"] as List<object>;

            IActionResult result;
            
            switch (methodName)
            {
                case "getUsersBlogs":
                    result = GetUsersBlogs(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString());
                    break;
                case "getCategories":
                    result = GetCategories(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString());
                    break;
                case "getRecentPosts":
                    result = GetRecentPosts(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString(), (int)methodParams[3]);
                    break;
                case "getPost":
                    result = GetPost(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString());
                    break;
                case "deletePost":
                    result = DeletePost(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString(), methodParams[3].ToString());
                    break;
                case "editPost":
                    result = EditPost(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString(), (XmlRpcPost)methodParams[3], (bool)methodParams[4]);
                    break;
                case "newPost":
                    result = NewPost(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString(), (XmlRpcPost)methodParams[3], (bool)methodParams[4]);
                    break;
                case "newMediaObject":
                    result = NewMediaObject(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString(), (MediaObject)methodParams[3]);
                    break;
                default:
                    object nullObject = null;
                    result = new XmlRpcResult(nullObject);
                    break;
            }

            return result;
        }

        private IActionResult GetUsersBlogs(string key, string userName, string password)
        {
            return CheckSecurity(userName, password, () =>
            {
                var blog = _blogContext.Blog;
                var blogUrl = (_environment.IsDevelopment()) ? blog.LocalUrl : blog.Url;
                var blogs = new[]
                {
                    new
                    {
                        blogid = blog.Id,
                        blogName = blog.Title,
                        url = $"http://{blogUrl}"  
                    }
                };
                return new XmlRpcResult(blogs);
            });
        }

        private IActionResult DeletePost(string key, string postId, string userName, string password)
        {
            return CheckSecurity(userName, password, () =>
            {
                var blog = _blogContext.Blog;
                Post post = _blogManager.GetPost(blog.Id, p => p.PostId == postId);

                if (post != null)
                {
                    post.BlogId = blog.Id;
                    _blogManager.DeletePost(post);
                }

                return new XmlRpcResult(post != null);
            });
        }

        private IActionResult EditPost(string postId, string userName, string password, XmlRpcPost xmlRpcPost, bool publish)
        {
            return CheckSecurity(userName, password, () =>
            {
                var blog = _blogContext.Blog;
                Post match = _blogManager.GetPost(blog.Id, p => p.PostId == postId);

                if (match != null)
                {
                    //Merge XmlRpcPost into matched Post
                    xmlRpcPost.ToPost(match);
                    
                    if (!string.Equals(match.Slug, xmlRpcPost.Slug, StringComparison.OrdinalIgnoreCase)  && !string.IsNullOrWhiteSpace(xmlRpcPost.Slug))
                    {
                        match.Slug = CreateSlug(xmlRpcPost.Slug);
                    }

                    match.LastModified = DateTime.UtcNow;
                    
                    if (publish && !match.IsPublished)
                    {
                        match.PubDate = DateTime.UtcNow;
                    }
                    match.IsPublished = publish;

                    _blogManager.SavePost(match);
                }

                return new XmlRpcResult(match != null);
            });
        }

        private IActionResult GetPost(string postId, string userName, string password)
        {
            return CheckSecurity(userName, password, () =>
            {
                var blog = _blogContext.Blog;
                var post = _blogManager.GetPost(blog.Id, p => p.PostId == postId);

                return new XmlRpcResult(post.ToXmlRpcPost());
            });
        }

        private IActionResult GetRecentPosts(string blogId, string userName, string password, int numberOfPosts)
        {
            return CheckSecurity(userName, password, () =>
            {
                var posts = _blogManager.GetRecentPosts(blogId, numberOfPosts);

                return new XmlRpcResult(posts.ToXmlRpcPosts());
            });
        }

        private IActionResult NewPost(string blogId, string userName, string password, XmlRpcPost xmlRpcPost, bool publish)
        {
            return CheckSecurity(userName, password, () =>
            {
                Post post = null;
                try
                {
                    post = xmlRpcPost.ToPost(blogId);
                    
                    if (!string.IsNullOrWhiteSpace(post.Slug))
                    {
                        post.Slug = CreateSlug(post.Slug);
                    }
                    else
                    {
                        post.Slug = CreateSlug(post.Title);
                    }
                }
                catch (Exception exc)
                {
                    return new XmlRpcResult(exc);
                }

                post.LastModified = DateTime.UtcNow;
                
                if (publish)
                {
                    post.PubDate = DateTime.UtcNow;
                }
                post.IsPublished = publish;

                _blogManager.SavePost(post);

                return new XmlRpcResult(post.PostId);
            });
        }
        
        private IActionResult GetCategories(string blogId, string userName, string password)
        {
            return CheckSecurity(userName, password, () =>
            {
                var categories = _blogManager.GetCategories(blogId, -1);

                var list = new List<object>();

                foreach (string category in categories.Keys)
                {
                    list.Add(new {title = category});
                }

                return new XmlRpcResult(list.ToArray());
            });
        }

        private IActionResult NewMediaObject(string blogId, string userName, string password, MediaObject media)
        {
            return CheckSecurity(userName, password, () =>
            {
                string relative = _blogManager.SaveMedia(blogId, media);

                return new XmlRpcResult(new {url = $"{Request.Scheme}://{Request.Host}{relative}"});
            });
        }

        private IActionResult CheckSecurity(string userName, string password, Func<IActionResult> secureFunc)
        {
            if (_securityOptions.Username == userName && _securityOptions.Password == password)
            {
                return secureFunc();
            }

            return new UnauthorizedResult();
        }
    }
}
