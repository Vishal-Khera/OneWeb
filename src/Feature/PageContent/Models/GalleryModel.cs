using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Models;

namespace OneWeb.Feature.PageContent.Models
{

    public class GalleryModel
    {
        public string Date { get; set; }      
        public IEnumerable<GalleryMedia> Images { get; set; }
    }
    public class GalleryMedia
    {
        public string image { get; set; }
    }
}