using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Data.Fields;

namespace OneWeb.Feature.Navigation.Models
{
    public class LanguageModel
    {
        public string Title { get; set; }
        public LinkModel Link { get; set; }
        public bool Active { get; set; }
    }
}