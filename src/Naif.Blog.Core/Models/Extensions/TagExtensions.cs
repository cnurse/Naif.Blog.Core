using System;
using System.Collections.Generic;
using System.Linq;

namespace Naif.Blog.Models.Extensions
{
    public static class TagExtensions
    {
        public static string ToString(this IList<Tag> tags, string separator)
        {
            return String.Join(separator, tags.Select(c => c.Name));
        }
    }
}