using System;
using System.Collections.Generic;
using System.Linq;

namespace Naif.Blog.Models.Extensions
{
    public static class CategoryExtensions
    {
        public static string ToString(this IList<Category> categories, string separator)
        {
            return $"[{String.Join(separator, categories.Select(c => c.Name))}]";
        }

        public static string[] ToStringArray(this IList<Category> categories)
        {
            return categories.Select(c => c.Name).ToArray();
        }
    }
}