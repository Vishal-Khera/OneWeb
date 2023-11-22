using System.Collections.Generic;

namespace OneWeb.Feature.PageContent.Models
{
    public class BusinessRequest
    {
        public string Keyword { get; set; }
        public string Scope { get; set; }
        public Dictionary<string, List<string>> Filters { get; set; }
    }
}