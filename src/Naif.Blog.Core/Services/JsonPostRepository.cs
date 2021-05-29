using Naif.Blog.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Naif.Blog.Models.Entities;
using Naif.Blog.Models.Extensions;
using Newtonsoft.Json;
// ReSharper disable ClassNeverInstantiated.Global

namespace Naif.Blog.Services
{
    public class JsonPostRepository : FilePostRepository
    {
        public JsonPostRepository(IWebHostEnvironment env, IMemoryCache memoryCache, ILoggerFactory loggerFactory) : base(env, memoryCache)
        {
            Logger = loggerFactory.CreateLogger<JsonPostRepository>();
        }

        protected override string FileExtension => "json";

        protected override Post GetPost(string file, string blogId)
        {
            JsonPost jsonPost;
            using (StreamReader r = File.OpenText(file))
            {
                string json = r.ReadToEnd();
                jsonPost =  JsonConvert.DeserializeObject<JsonPost>(json);
                jsonPost.BlogId = blogId;
            }
            return jsonPost.ToPost();
        }

        protected override void SavePost(Post post, string file)
        {
            JsonPost jsonPost = post.ToJsonPost();
            using (StreamWriter w = File.CreateText(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(w, jsonPost);
            }
        }
    }
}
