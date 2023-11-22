using System.Collections.Generic;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Foundation.SitecoreExtensions.Models
{
    public class HandleBarRenderingModel : VariantsRenderingModel
    {
        public string ConfigurationId { get; set; }
        public string Endpoint { get; set; }
        public string SourceId { get; set; }
        public string TemplateType { get; set; }
        public Dictionary<string, object> Parameters { get; set; }



        public HandleBarRenderingModel()
        {
            Parameters = new Dictionary<string, object>();
        }
    }
}