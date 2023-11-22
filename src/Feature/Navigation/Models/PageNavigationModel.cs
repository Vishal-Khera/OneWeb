using System.Collections.Generic;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.Navigation.Models
{
    public class PageNavigationModel : VariantsRenderingModel
    {
        public string PageNavTitle { get; set; }
        public string PageNavId { get; set; }
    }
    public class PageNavigationRenderingModel : VariantsRenderingModel
    {
        public List<PageNavigationModel> PageNavigationList { get; set; }
        public string NavigationTitle { get; set; }
        public PageNavigationRenderingModel()
        {
            PageNavigationList = new List<PageNavigationModel>();
        }
    }
}