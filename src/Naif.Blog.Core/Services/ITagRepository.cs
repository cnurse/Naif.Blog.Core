using System.Collections.Generic;
using Naif.Blog.Models;

namespace Naif.Blog.Services
{
    public interface ITagRepository
    {
        /// <summary>
        /// Gets all the Tags for the Blog
        /// </summary>
        /// <param name="blogId">The Id of the blog</param>
        /// <returns>A list of Tags</returns>
        IList<Tag> GetTags(string blogId);
    }
}