using Naif.Blog.Models;
using System.Collections.Generic;

namespace Naif.Blog.Services
{
    public interface IBlogRepository
    {
        IEnumerable<Models.Blog> GetBlogs();

        IList<Category> GetCategories(string blogId);

        IList<Tag> GetTags(string blogId);

        /*
        IEnumerable<string> GetTemplates(string blogId);
        IEnumerable<string> GetThemes();
        void SaveBlogs(IEnumerable<Models.Blog> blogs);
        */

        string SaveMedia(string blogid, MediaObject media);
    }
}
