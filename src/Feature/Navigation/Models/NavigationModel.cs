using System.Collections.Generic;
using OneWeb.Foundation.SitecoreExtensions.Models;

namespace OneWeb.Feature.Navigation.Models
{
    public class NavigationModel
    {
        public string Title { get; set; }
        public LinkModel Link { get; set; }
        public string Description { get; set; }
        public MediaModel Image { get; set; }
        public bool HasChildren { get; set; }
        public bool IsActive { get; set; }
        public string CssClass { get; set; }
        public List<NavigationModel> Children { get; set; }
        public bool HasSubChildren { get; set; }
    }
}