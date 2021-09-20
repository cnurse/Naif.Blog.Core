using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Naif.Blog.Models
{
    public class Blog
    {
        public Blog()
        {
            UniqueId = Guid.NewGuid().ToString();
            ByLine = String.Empty;
            Disclaimer = String.Empty;
            GoogleAnalytics = String.Empty;
            HomeRedirectUrl = String.Empty;
            BlogId = String.Empty;
            LocalUrl = String.Empty;
            OwnerId = -1;
            Theme = String.Empty;
            Title = String.Empty;
            Url = String.Empty;
        }
        
        public string BlogId { get; set; }

        public string ByLine { get; set; }

        public string Disclaimer { get; set; }

        /// <summary>
        /// The OwnerId property is used for situations when the Blog is part of a an external system (eg. ModuleId for Oqtane)
        /// It is ignored in Naif.Blog itself
        /// </summary>
        [JsonIgnore]
        public int OwnerId { get; set; }

        public string Title { get; set; }

        public string UniqueId { get; set; }
        
        [NotMapped]
        public string GoogleAnalytics { get; set; }
        
        [NotMapped]
        public string HomeRedirectUrl { get; set; }

        [NotMapped]
        public string LocalUrl { get; set; }
        
        [NotMapped]
        public string Theme { get; set; }
        
        [NotMapped]
        public string Url { get; set; }
        
        //Relationships
        [JsonIgnore]
        public IList<Category> Categories { get; set; }

        [JsonIgnore]
        public IList<Tag> Tags { get; set; }
    }
}


