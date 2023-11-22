using OneWeb.Foundation.Search.Models.ContentSearch;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.PageContent.Models
{
    public class BusinessRenderingModel : VariantsRenderingModel
    {
        public ContentResponse<BusinessModel> BusinessResponse { get; set; }
    }
}