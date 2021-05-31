using System;
using System.Collections.Generic;

namespace Naif.Blog.Framework
{
    public interface IBlogContext
    {
        Models.Blog Blog { get; set; }
    }
}