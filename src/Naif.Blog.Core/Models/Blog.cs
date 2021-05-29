using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Naif.Blog.Models
{
    public class Blog
    {
        public Blog()
        {
            ByLine = String.Empty;
            Disclaimer = String.Empty;
            GoogleAnalytics = String.Empty;
            HomeRedirectUrl = String.Empty;
            Id = String.Empty;
            LocalUrl = String.Empty;
            OwnerId = -1;
            Theme = String.Empty;
            Title = String.Empty;
            Url = String.Empty;
        }
        
        public string Id { get; set; }

        public string ByLine { get; set; }

        public string Disclaimer { get; set; }

        /// <summary>
        /// The OwnerId property is used for situations when the Blog is part of a an external system (eg. ModuleId for Oqtane)
        /// It is ignored in Naif.Blog itself
        /// </summary>
        [JsonIgnore]
        public int OwnerId { get; set; }

        public string Title { get; set; }

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
        public IList<Category> Categories { get; set; }

        public IList<Tag> Tags { get; set; }
    }
}


