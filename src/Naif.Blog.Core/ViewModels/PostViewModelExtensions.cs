using System;
using System.Collections.Generic;
using Naif.Blog.Models;
using Naif.Blog.Models.Extensions;

namespace Naif.Blog.ViewModels
{
    public static class PostViewModelExtensions
    {
        public static PostViewModel ToPostViewModel(this Post post)
        {
            return new PostViewModel
            {
                PostId = post.PostId,
                BlogId = post.BlogId,
                Title = post.Title,
                SubTitle = post.SubTitle,
                Author = post.Author,
                Slug = post.Slug,
                Excerpt = post.Excerpt,
                Content = post.Content,
                Categories = post.Categories.ToString(","),
                Tags = post.Tags.ToString(","),
                IncludeInLists = post.IncludeInLists,
                IsPublished = post.IsPublished,
                PostType = post.PostType,
                PostTypeDetail = post.PostTypeDetail,
                ParentPostId = post.ParentPostId,
                PageOrder = post.PageOrder
            };
        }

        public static Post ToPost(this PostViewModel postViewModel, string blogId)
        {
            var post = new Post()
            {
                BlogId = blogId
            };

            UpdatePost(post, postViewModel);

            return post;

        }

        public static void ToPost(this PostViewModel postViewModel, Post post)
        {
            UpdatePost(post, postViewModel);
        }

        private static void UpdatePost(Post post, PostViewModel postViewModel)
        {
            post.PostId = postViewModel.PostId;
            post.Title = postViewModel.Title;
            post.SubTitle = postViewModel.SubTitle;
            post.Author = postViewModel.Author;
            post.Slug = postViewModel.Slug;
            post.Excerpt = postViewModel.Excerpt;
            post.Content = postViewModel.Content;
            post.IncludeInLists = postViewModel.IncludeInLists;
            post.IsPublished = postViewModel.IsPublished;
            post.PostType = postViewModel.PostType;
            post.PostTypeDetail = postViewModel.PostTypeDetail;
            post.ParentPostId = postViewModel.ParentPostId;
            post.PageOrder = postViewModel.PageOrder;

            post.Categories = new List<Category>();
            if (!string.IsNullOrEmpty(postViewModel.Categories))
            {
                foreach (var category in postViewModel.Categories.TrimStart('[').TrimEnd(']').Split(new[] { ',' }))
                {
                    if (!String.IsNullOrEmpty(category))
                    {
                        post.Categories.Add(new Category()
                        {
                            Name = category
                        });
                    }
                }
            }

            post.Tags = new List<Tag>();
            if (!string.IsNullOrEmpty(postViewModel.Tags))
            {
                foreach (var tag in postViewModel.Tags.Split(new[] { ',' }))
                {
                    if (!String.IsNullOrEmpty(tag))
                    {
                        post.Tags.Add(new Tag()
                        {
                            Name = tag
                        });
                    }
                }
            }
        }
    }
}
