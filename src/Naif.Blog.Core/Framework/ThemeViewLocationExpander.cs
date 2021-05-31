using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;
// ReSharper disable UsePatternMatching

namespace Naif.Blog.Framework
{
	public class ThemeViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var enumerable = viewLocations as string[] ?? viewLocations.ToArray();
            var themeLocations = enumerable.ToList();
            if (context.Values.ContainsKey("theme"))
            {
                themeLocations.InsertRange(0, enumerable.Select(f => f.Replace("/Views/", "/Views/Themes/" + context.Values["theme"] + "/")));
            }
            return themeLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var blogContext = context.ActionContext.HttpContext.RequestServices
                        .GetService(typeof(IBlogContext)) as IBlogContext;

            if (blogContext != null && !string.IsNullOrEmpty(blogContext.Blog.Theme))
            {
                context.Values["theme"] = blogContext.Blog.Theme;
            }
        }
    }
}

