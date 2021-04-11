using System;
using System.Collections.Generic;
using Naif.Blog.Models;

namespace Naif.Blog.Services
{
    public interface IBlogManager
    {
        /// <summary>
        /// Delete a Post
        /// </summary>
        /// <param name="post">The post to delete</param>
        void DeletePost(Post post);

        /// <summary>
        /// Gets the Categories with the highest number of posts
        /// </summary>
        /// <param name="blogId">The Id of the blog</param>
        /// <param name="count">The maximum number of categories to return</param>
        /// <returns>A dictionary of categoreis</returns>
        Dictionary<string, int> GetCategories(string blogId, int count);

        /// <summary>
        /// Gets a single post
        /// </summary>
        /// <param name="blogId">The Id of the blog</param>
        /// <param name="predicate">A predicate used to search for the specific post</param>
        /// <returns>A Post</returns>
        Post GetPost(string blogId, Func<Post, bool> predicate);
        
        /// <summary>
        /// Gets the most recent posts
        /// </summary>
        /// <param name="blogId">The Id of the blog</param>
        /// <param name="count">The maximum number of posts to return</param>
        /// <returns>An enumerable list of posts</returns>
        IEnumerable<Post> GetRecentPosts(string blogId, int count);
        
        /// <summary>
        /// Gets a list of posts based on a specified predicate
        /// </summary>
        /// <param name="blogId">The Id of the blog</param>
        /// <param name="predicate">A predicate used to search for the specific posts</param>
        /// <returns>An enumerable list of posts</returns>
        IEnumerable<Post> GetPosts(string blogId, Func<Post, bool> predicate);

        /// <summary>
        /// Save a media object
        /// </summary>
        /// <param name="blogId">The Id of the blog</param>
        /// <param name="media">The media object to save</param>
        /// <returns></returns>
        string SaveMedia(string blogId, MediaObject media);

        /// <summary>
        /// Save a post
        /// </summary>
        /// <param name="post">The post to save</param>
        void SavePost(Post post);
    }
}