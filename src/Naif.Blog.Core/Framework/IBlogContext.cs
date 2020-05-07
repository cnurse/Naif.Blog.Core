using System;
using System.Collections.Generic;

namespace Naif.Blog.Framework
{
    public interface IBlogContext
    {
        Guid Id { get; }

        IEnumerable<Models.Blog> Blogs { get; set; }

        Models.Blog CurrentBlog { get; set; }
    }
}