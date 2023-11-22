using System.Collections.Generic;
using Sitecore.Data.Items;

namespace OneWeb.Feature.Search.Models
{
    public class AjaxSearchParameters
    {
        public string Endpoint { get; set; }
        public string ConfigurationId { get; set; }
        public string Keyword { get; set; }
        public string SourceId { get; set; }
        public string PageSize { get; set; }
        public string PagesToSkip { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public List<AjaxSearchFilter> Filters { get; set; }
        public Item SourceItem { get; set; }
        public string Language { get; set; }

        public AjaxSearchParameters()
        {
            Parameters = new Dictionary<string, string>();
            Filters = new List<AjaxSearchFilter>();
        }
    }
}