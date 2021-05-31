using System.Collections.Generic;
using System.Linq;
using Naif.Blog.Models;

namespace Naif.Blog.Services
{
    public class FileTagRepository : ITagRepository
    {
        private readonly IPostRepository _postRepository;

        public FileTagRepository(IPostRepository postRepository)
        {
            _postRepository = postRepository;
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

    }
}