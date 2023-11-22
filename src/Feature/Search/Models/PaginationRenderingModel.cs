using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.Search.Models
{
    public class PaginationRenderingModel : VariantsRenderingModel
    {
        public string ConfigurationId { get; set; }
        public string NextText { get; set; }
        public string PreviousText { get; set; }
        public int DisplayedPages { get; set; }
        public int VisibleEdges { get; set; }
    }
}