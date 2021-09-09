using System;
using System.Collections.Generic;
using Naif.Blog.Models.Entities;

namespace Naif.Blog.Models.Extensions
{
    internal static class XmlRpcPostExtensions
    {
        internal static void ToPost(this XmlRpcPost xmlRpcPost, Post post)
        {
            UpdatePost(post, xmlRpcPost, false);
        }

        internal static Post ToPost(this XmlRpcPost xmlRpcPost, string blogId)
        {
            var post = new Post()
            {
                BlogId = blogId
            };

            UpdatePost(post, xmlRpcPost, true);

            return post;
        }

        internal static XmlRpcPost ToXmlRpcPost(this Post post)
        {
            var xmlRpcPost = new XmlRpcPost
            {
                PostId = post.PostId,
                BlogId = post.BlogId,
                LastModified = post.LastModified,
                PubDate = post.PubDate,
                Title = post.Title,
                Excerpt = post.Excerpt,
                Content = post.Content,
                Slug = post.Slug,
                Categories = post.Categories.ToStringArray(),
                Keywords = post.Tags.ToString(","),
                CustomFields =
                {
                    ["mt_author"] = post.Author,
                    ["mt_includeinlists"] = post.IncludeInLists.ToString(),
                    ["mt_markdown"] = post.Markdown,
                    ["mt_parentpostid"] = post.ParentPostId,
                    ["mt_posttype"] = post.PostType.ToString(),
                    ["mt_posttypedetail"] = post.PostTypeDetail,
                    ["mt_subtitle"] = post.SubTitle,
                    ["mt_template"] = post.Template
                }
            };



            //xmlRpcPost.CustomFields["mt_related"] = post.RelatedPosts;

            return xmlRpcPost;
        }

        internal static IEnumerable<XmlRpcPost> ToXmlRpcPosts(this IEnumerable<Post> posts)
        {
            var xmlRpcPosts = new List<XmlRpcPost>();

            foreach (var post in posts)
            {
                xmlRpcPosts.Add(post.ToXmlRpcPost());
            }
            return xmlRpcPosts;
        }

        private static void UpdatePost(Post post, XmlRpcPost xmlRpcPost, bool includeSlug)
        {
            post.Title = xmlRpcPost.Title;
            post.Excerpt = xmlRpcPost.Excerpt;
            post.Content = xmlRpcPost.Content;

            if (includeSlug)
            {
                post.Slug = xmlRpcPost.Slug;
            }

            post.Categories = new List<Category>();
            if (xmlRpcPost.Categories != null)
            {
                foreach (var category in xmlRpcPost.Categories)
                {
                    post.Categories.Add(new Category()
                    {
                        Name = category
                    });
                }
            }

            post.Tags = new List<Tag>();
            if (!String.IsNullOrEmpty(xmlRpcPost.Keywords))
            {
                foreach (var tag in xmlRpcPost.Keywords.Split(','))
                {
                    post.Tags.Add(new Tag()
                    {
                        Name = tag
                    });
                }
            }

            //Custom Fields
            post.Author = xmlRpcPost.GetCustomField("mt_author");
            post.IncludeInLists = bool.Parse(xmlRpcPost.GetCustomField("mt_includeinlists"));
            post.Markdown = xmlRpcPost.GetCustomField("mt_markdown");
            post.ParentPostId = xmlRpcPost.GetCustomField("mt_parentpostid");
            post.PostType = (PostType) Enum.Parse(typeof(PostType),xmlRpcPost.GetCustomField("mt_posttype", "Post"));
            post.PostTypeDetail = xmlRpcPost.GetCustomField("mt_posttypedetail");
            post.SubTitle = xmlRpcPost.GetCustomField("mt_subtitle");
            post.Template = xmlRpcPost.GetCustomField("mt_template");
            
            //post.RelatedPosts = xmlRpcPost.GetCustomField("mt_related");
        }
        
        private static string GetCustomField(this XmlRpcPost xmlRpcPost, string field, string defaultValue = "")
        {
            var customField = defaultValue;
            if (xmlRpcPost.CustomFields.ContainsKey(field))
            {
                customField = xmlRpcPost.CustomFields[field];
            }

            return customField;
        }

    }
}