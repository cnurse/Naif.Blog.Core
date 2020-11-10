using Microsoft.AspNetCore.Mvc;
using Naif.Blog.Framework;
using Naif.Blog.Models;
using Naif.Blog.Services;
using Naif.Blog.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Naif.Blog.Security;

namespace Naif.Blog.Controllers
{
    [Route("MetaWeblog")]
    public class MetaWeblogController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly XmlRpcSecurityOptions _securityOptions;
        private readonly IPostRepository _postRepository;

        public MetaWeblogController(IWebHostEnvironment environment,
            IBlogRepository blogRepository, 
            IBlogContext blogContext, 
            IPostRepository postRepository, 
            IOptions<XmlRpcSecurityOptions> optionsAccessor) 
            :base(blogRepository, blogContext)
        {
            _environment = environment;
            _securityOptions = optionsAccessor.Value;
            _postRepository = postRepository;
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
                    result = EditPost(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString(), (Post)methodParams[3], (bool)methodParams[4]);
                    break;
                case "newPost":
                    result = NewPost(methodParams[0].ToString(), methodParams[1].ToString(), methodParams[2].ToString(), (Post)methodParams[3], (bool)methodParams[4]);
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

                var blogUrl = (_environment.IsDevelopment()) ? Blog.LocalUrl : Blog.Url;
                var blogs = new[]
                {
                    new
                    {
                        blogid = Blog.Id,
                        blogName = Blog.Title,
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
                Post post = _postRepository.GetAllPosts(Blog.Id).FirstOrDefault(p => p.PostId == postId);

                if (post != null)
                {
                    post.BlogId = Blog.Id;
                    _postRepository.DeletePost(post);
                }

                return new XmlRpcResult(post != null);
            });
        }

        private IActionResult EditPost(string postId, string userName, string password, Post post, bool publish)
        {
            return CheckSecurity(userName, password, () =>
            {
                Post match = _postRepository.GetAllPosts(Blog.Id).FirstOrDefault(p => p.PostId == postId);

                if (match != null)
                {
                    match.Title = post.Title;
                    match.Excerpt = post.Excerpt;
                    match.Content = post.Content;

                    if (!string.Equals(match.Slug, post.Slug, StringComparison.OrdinalIgnoreCase)  && !string.IsNullOrWhiteSpace(post.Slug))
                    {
                        match.Slug = CreateSlug(post.Slug);
                    }

                    match.Categories = post.Categories;
                    match.Keywords = post.Keywords;
                    match.IsPublished = publish;

                    //Custom Fields
                    match.Author = post.Author;
                    match.Markdown = post.Markdown;
                    match.ParentPostId = post.ParentPostId;
                    match.PostType = post.PostType;
                    match.PostTypeDetail = post.PostTypeDetail;
                    match.RelatedPosts = post.RelatedPosts;
                    match.SubTitle = post.SubTitle;
                    match.Template = post.Template;

                    _postRepository.SavePost(match);
                }

                return new XmlRpcResult(match != null);
            });
        }

        private IActionResult GetPost(string postId, string userName, string password)
        {
            return CheckSecurity(userName, password, () =>
            {
                var post = _postRepository.GetAllPosts(Blog.Id).FirstOrDefault(p => p.PostId == postId);

                return new XmlRpcResult(post);
            });
        }

        private IActionResult GetRecentPosts(string blogId, string userName, string password, int numberOfPosts)
        {
            return CheckSecurity(userName, password, () =>
            {
                var posts = _postRepository.GetAllPosts(blogId).Take(numberOfPosts);

                return new XmlRpcResult(posts);
            });
        }

        private IActionResult NewPost(string blogId, string userName, string password, Post post, bool publish)
        {
            return CheckSecurity(userName, password, () =>
            {

                try
                {
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

                post.IsPublished = publish;
                post.BlogId = blogId;
                _postRepository.SavePost(post);

                return new XmlRpcResult(post.PostId);
            });
        }
        
        private IActionResult GetCategories(string blogId, string userName, string password)
        {
            return CheckSecurity(userName, password, () =>
            {

                var categories = BlogRepository.GetCategories(blogId);

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

                string relative = BlogRepository.SaveMedia(blogId, media);

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
