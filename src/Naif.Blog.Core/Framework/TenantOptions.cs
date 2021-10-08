using System.Collections.Generic;
using Naif.Blog.Authentication;
// ReSharper disable ClassNeverInstantiated.Global

namespace Naif.Blog.Framework
{
    public class TenantOptions
    {
        public Dictionary<string, Auth0Options> Auth0 { get; set; }
    }
}