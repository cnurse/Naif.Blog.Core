using System.Collections.Generic;
using Naif.Blog.Models.Entities;

namespace Naif.Blog.Models.Extensions
{
    internal static class JsonPostExtensions
    {
        internal static Post ToPost(this JsonPost jsonPost)
        {
            var post = new Post
            {
                PostId = jsonPost.PostId,
                BlogId = jsonPost.BlogId,
                Title = jsonPost.Title,
                Excerpt = jsonPost.Excerpt,
                Content = jsonPost.Content,
                Slug = jsonPost.Slug,
                LastModified = jsonPost.LastModified,
                PubDate = jsonPost.PubDate,
                IsPublished = jsonPost.IsPublished,
                Author = jsonPost.Author,
                IncludeInLists = jsonPost.IncludeInLists,
                Markdown = jsonPost.Markdown,
                ParentPostId = jsonPost.ParentPostId,
                PostType = jsonPost.PostType,
                PostTypeDetail = jsonPost.PostTypeDetail,
                SubTitle = jsonPost.SubTitle,
                Template = jsonPost.Template,
                Categories = new List<Category>()
            };

            foreach (var category in jsonPost.Categories)
            {
                post.Categories.Add(new Category()
                {
                    Name = category
                });
            }

            post.Tags = new List<Tag>();
            foreach (var tag in jsonPost.Keywords.Split(','))
            {
                post.Tags.Add(new Tag()
                {
                    Name = tag
                });
            }

            return post;
        }

        internal static JsonPost ToJsonPost(this Post post)
        {
            var jsonPost = new JsonPost
            {
                PostId = post.PostId,
                BlogId = post.BlogId,
                Title = post.Title,
                Excerpt = post.Excerpt,
                Content = post.Content,
                Slug = post.Slug,
                Categories = post.Categories.ToStringArray(),
                Keywords = post.Tags.ToString(","),
                
                LastModified = post.LastModified,
                PubDate = post.PubDate,
                IsPublished = post.IsPublished,

                Author = post.Author,
                IncludeInLists = post.IncludeInLists,
                Markdown =  post.Markdown,
                ParentPostId = post.ParentPostId,
                PostType = post.PostType,
                PostTypeDetail = post.PostTypeDetail,
                SubTitle = post.SubTitle,
                Template = post.Template
            };

            return jsonPost;
        }
    }
}