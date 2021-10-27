using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Naif.Core.Models;

namespace Naif.Blog.ViewModels
{
    public class EditPostViewModel
    {
        public string ActionUrl { get; set; }
        
        public IList<Message> Messages { get; set; }

        public int PageIndex { get; set; }

        public PostViewModel Post { get; set; }

        public List<SelectListItem> Pages { get; set; }
                
        public string ReturnUrl { get; set; }
    }
}
