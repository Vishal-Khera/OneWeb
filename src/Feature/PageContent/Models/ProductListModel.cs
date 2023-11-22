using System.Collections.Generic;
using OneWeb.Foundation.SitecoreExtensions.Models;

namespace OneWeb.Feature.PageContent.Models
{
    public class ProductListModel
    {
        public string Title { get; set; }
        public LinkModel Link  { get; set; }        
        public MediaModel Image { get; set; }    
    }
}