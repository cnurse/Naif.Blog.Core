using Naif.Blog.Models;
using System.Collections.Generic;

namespace Naif.Blog.Services
{
    public interface IPostRepository
    {
        /// <summary>
        /// Delete a Post
        /// </summary>
        /// <param name="post">The post to delete</param>
        void DeletePost(Post post);

        /// <summary>
        /// Get all the posts for a blog
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        IEnumerable<Post> GetAllPosts(string blogId);

        /// <summary>
        /// Save a post
        /// </summary>
        /// <param name="post">The post to save to the repository</param>
        void SavePost(Post post);
    }
}
