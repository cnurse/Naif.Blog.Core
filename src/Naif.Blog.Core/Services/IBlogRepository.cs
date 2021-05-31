using System;
using Naif.Blog.Models;
using System.Collections.Generic;

namespace Naif.Blog.Services
{
    public interface IBlogRepository
    {
        /// <summary>
        /// Get a single Blog.
        /// </summary>
        /// <param name="predicate">The predicate used to determine which blog to return</param>
        /// <returns>A Blog object</returns>
        Models.Blog GetBlog(Func<Models.Blog, bool> predicate);
        
        /// <summary>
        /// Gets all the currently defined Blogs
        /// </summary>
        /// <returns>A collection of Blogs</returns>
        IEnumerable<Models.Blog> GetBlogs();

        /// <summary>
        /// Gets al collection of Blogs
        /// </summary>
        /// <param name="predicate">The predicate used to determine which blogs to return</param>
        /// <returns>A collection of Blogs</returns>
        IEnumerable<Models.Blog> GetBlogs(Func<Models.Blog, bool> predicate);

        /// <summary>
        /// Save a Blog object in the repository
        /// </summary>
        /// <param name="blog">The blog object to save</param>
        void SaveBlog(Models.Blog blog);

        /// <summary>
        /// Save a media file
        /// </summary>
        /// <param name="blogid">The Id of the Blog to which the media file belonds</param>
        /// <param name="media">The media object to save</param>
        /// <returns></returns>
        string SaveMedia(string blogid, MediaObject media);
    }
}
