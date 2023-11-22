using System.Collections.Generic;
using Sitecore.Shell.Framework.Commands;

namespace OneWeb.Foundation.Search.Models
{
    public class FilterMapping
    {
        public Filter PrimaryFilter { get; set; }
        public List<Filter> SecondaryFilter { get; set; }
    }

    public class Filter
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}