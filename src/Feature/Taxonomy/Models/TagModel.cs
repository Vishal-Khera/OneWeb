
using System.Collections.Generic;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.Taxonomy.Models
{
    public class TagModel : VariantsRenderingModel
    {
        public int DisplayTagCount { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}