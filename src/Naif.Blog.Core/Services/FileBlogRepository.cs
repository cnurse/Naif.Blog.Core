using System;
using System.Collections.Generic;
using Naif.Blog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Naif.Core.Cache;
using Newtonsoft.Json;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Naif.Blog.Services
{
    public class FileBlogRepository : FileRepositoryBase, IBlogRepository
    {
        private string _blogsCacheKey = "blogs";
        private readonly string _blogsFile;
        private readonly string _filesFolder;
        private readonly string _fileUrl;
        
        public FileBlogRepository(IWebHostEnvironment env, 
                                    IMemoryCache memoryCache, 
                                    ILoggerFactory loggerFactory) :base(env, memoryCache)
        {
            Logger = loggerFactory.CreateLogger<FileBlogRepository>();
            _filesFolder = "{0}/posts/{1}/files/";
            _fileUrl = "/posts/{0}/files/{1}";
            _blogsFile = env.WebRootPath + @"\blogs.json";
        }

        protected override string FileExtension { get; }

        public Models.Blog GetBlog(Func<Models.Blog, bool> predicate)
        {
            return GetBlogs().SingleOrDefault(predicate);
        }

        public IEnumerable<Models.Blog> GetBlogs()
        {
            return MemoryCache.GetObject(_blogsCacheKey, 
                Logger,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(2)),
                () =>
                {
                    IList<Models.Blog> blogs;
                    
                    // fetch the value from the source
                    using (StreamReader reader = File.OpenText(_blogsFile))
                    {
                        var json = reader.ReadToEnd();
                        blogs = JsonConvert.DeserializeObject<IList<Models.Blog>>(json);
                    }

                    return blogs;
                });
        }

        public IEnumerable<Models.Blog> GetBlogs(Func<Models.Blog, bool> predicate)
        {
            return GetBlogs().Where(predicate);
        }

        public void SaveBlog(Models.Blog blog)
        {
            var blogs = GetBlogs();
            var match = blogs.SingleOrDefault(b => b.UniqueId == blog.UniqueId);
            if (match != null)
            {
                match.BlogId = blog.BlogId;
                match.ByLine = blog.ByLine;
                match.Disclaimer = blog.Disclaimer;
                match.GoogleAnalytics = blog.GoogleAnalytics;
                match.HomeRedirectUrl = blog.HomeRedirectUrl;
                match.LocalUrl = blog.LocalUrl;
                match.Theme = blog.Theme;
                match.Title = blog.Title;
                match.Url = blog.Url;
            }
            
            using (StreamWriter w = File.CreateText(_blogsFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(w, blogs);
            }

            MemoryCache.Remove(_blogsCacheKey);
        }

        public string SaveMedia(string blogId, MediaObject media)
        {
            var filesFolder = GetFolder(_filesFolder, blogId);

            if (!Directory.Exists(filesFolder))
            {
                Directory.CreateDirectory(filesFolder);
            }

            string extension = Path.GetExtension(media.Name);

            string fileName = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".bin";
            }
            else
            {
                extension = "." + extension.Trim('.');
            }

            fileName += extension;

            string file = Path.Combine(filesFolder, fileName);

            File.WriteAllBytes(file, media.Bits);

            return String.Format(_fileUrl, blogId, fileName);
        }
    }
}
