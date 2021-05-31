using System.Collections.Generic;
using System.Linq;
using Naif.Blog.Models;

namespace Naif.Blog.Services
{
    public class FileCategoryRepository : ICategoryRepository
    {
        private readonly IPostRepository _postRepository;

        public FileCategoryRepository(IPostRepository postRepository)
        {
            _postRepository = postRepository;
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
    }
}