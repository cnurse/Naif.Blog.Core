using System;
using System.Collections.Generic;
using Naif.Auth0.Models;

namespace Naif.Blog.Framework
{
    public class BlogContext : IBlogContext
    {
        public Models.Blog Blog { get; set; }
        public Profile User { get; set; }
    }
}
