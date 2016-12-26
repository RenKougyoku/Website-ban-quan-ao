using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline.Models
{
    public class RSSItem
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public DateTime PubDate { get; set; }
    }

    
}