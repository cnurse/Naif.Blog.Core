using System;
using System.Collections.Generic;

namespace Naif.Blog.Framework
{
    public class BlogContext : IBlogContext
    {
        public Models.Blog Blog { get; set; }
    }
}
