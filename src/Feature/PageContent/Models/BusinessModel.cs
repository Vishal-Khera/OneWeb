using System.ComponentModel;
using Newtonsoft.Json.Linq;
using OneWeb.Foundation.Search.Models;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.SitecoreExtensions.Classes;
using OneWeb.Foundation.SitecoreExtensions.Classes.TypeConverters;
using Sitecore.ContentSearch;

namespace OneWeb.Feature.PageContent.Models
{
    public class BusinessModel : ContentModel
    {
        [IndexField("ow_business")]
        [TypeConverter(typeof(JsonTypeConverter))]
        public JObject Business { get; set; }
    }
}