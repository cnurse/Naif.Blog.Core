using System.Collections.Generic;
using Naif.Blog.Models;

namespace Naif.Blog.Services
{
    public interface ICategoryRepository
    {        
        /// <summary>
        /// Gets all the Categories for the Blog
        /// </summary>
        /// <param name="blogId">The Id of the blog</param>
        /// <returns>A list of Categories</returns>
        IList<Category> GetCategories(string blogId);
    }
}