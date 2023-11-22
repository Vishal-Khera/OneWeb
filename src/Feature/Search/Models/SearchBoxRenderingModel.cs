using System.Collections.Generic;
using System.Collections.Specialized;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.Search.Models
{
    public class SearchBoxRenderingModel : VariantsRenderingModel
    {
        public string ConfigurationId { get; set; }
        public string AutoSuggestConfigurationId { get; set; }
        public int AutoSuggestTriggerCount { get; set; }
        public string SearchResultPageUrl { get; set; }
        public string EmptyTextMessage { get; set; }
        public bool ShowResultsByDefault { get; set; }
        public List<Keyword> PopularKeywords { get; set; }
    }

    public class Keyword
    {
        public string Text { get; set; }
        public string Link { get; set; }
    }
}