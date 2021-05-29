using System;
using System.Collections.Generic;
using Naif.Blog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Naif.Core.Cache;
using Newtonsoft.Json;
// ReSharper disable ClassNeverInstantiated.Global

namespace Naif.Blog.Services
{
    public class FileBlogRepository : FileRepositoryBase, IBlogRepository
    {
        private string _blogsCacheKey = "blogs";
        private readonly string _blogsFile;
        private readonly IPostRepository _postRepository;
        private readonly string _filesFolder;
        private readonly string _fileUrl;
        private string _templatesCacheKey = "templates";
        private string _themesCacheKey = "themes";
        private readonly string _themesFile;
        private readonly MvcRazorRuntimeCompilationOptions _razorRuntimeCompilationOptions;
        
        public FileBlogRepository(IWebHostEnvironment env, 
                                    IOptions<MvcRazorRuntimeCompilationOptions> optionsAccessor,
                                    IMemoryCache memoryCache, 
                                    ILoggerFactory loggerFactory, 
                                    IPostRepository postRepository) :base(env, memoryCache)
        {
            Logger = loggerFactory.CreateLogger<FileBlogRepository>();
            _postRepository = postRepository;
            _filesFolder = "{0}/posts/{1}/files/";
            _fileUrl = "/posts/{0}/files/{1}";
            _blogsFile = env.WebRootPath + @"\blogs.json";
            _themesFile = env.WebRootPath + @"\themes.json";
            _razorRuntimeCompilationOptions = optionsAccessor.Value;
        }

        protected override string FileExtension { get; }

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

        public IList<Category> GetCategories(string blogId)
        {
            var result = _postRepository.GetAllPosts(blogId).Where(Post.SearchPredicate)
                .SelectMany(post => post.Categories)
                .GroupBy(category => category.Name, (categoryName, items) => new Category { Name = categoryName, Count = items.Count() })
                .OrderBy(x => x.Name)
                .ToList();

            return result;
        }

        public IList<Tag> GetTags(string blogId)
        {
            var result = _postRepository.GetAllPosts(blogId).Where(Post.SearchPredicate)
                .SelectMany(post => post.Tags)
                .GroupBy(tag => tag.Name, (tagName, items) => new Tag { Name = tagName, Count = items.Count() })
                .OrderBy(x => x.Name)
                .ToList();

            return result;
        }

        /*
        public IEnumerable<string> GetTemplates(string blogId)
        {
            return MemoryCache.GetObject(_templatesCacheKey, 
                Logger,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(2)),
                () =>
                {
                    IList<string> templates = new List<string>();

                    var blog = GetBlogs().SingleOrDefault(b => b.Id == blogId);

                    if (blog != null)
                    {

                        foreach (var fileProvider in _razorRuntimeCompilationOptions.FileProviders)
                        {
                            var files = fileProvider.GetDirectoryContents($"/Views/Themes/{blog.Theme}/Templates");
                            if (files.Exists)
                            {
                                foreach (var file in files)
                                {
                                    templates.Add(file.Name.Replace(".cshtml", ""));
                                }
                            }
                        }
                    }

                    return templates;
                });
        }

        public IEnumerable<string> GetThemes()
        {
            return MemoryCache.GetObject(_themesCacheKey, 
                Logger,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(2)),
                () =>
                {
                    IList<string> themes;

                    // fetch the value from the source
                    using (StreamReader reader = File.OpenText(_themesFile))
                    {
                        var json = reader.ReadToEnd();
                        themes = JsonConvert.DeserializeObject<IList<string>>(json);
                    }

                    return themes;
                });
        }

        public void SaveBlogs(IEnumerable<Models.Blog> blogs)
        {
            using (StreamWriter w = File.CreateText(_blogsFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(w, blogs);
            }
            
            MemoryCache.Remove(_blogsCacheKey);

            Logger.LogInformation($"Blog Settings updated.");

            Logger.LogInformation($"{_blogsCacheKey} cleared.");
        }
        */
        
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
