using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.Login.Models
{
    public class UserSearchBoxRenderingModel : VariantsRenderingModel
    {
        public string EmptyTextMessage { get; set; }
        public bool ShowUsersByDefault { get; set; }
        public int PageSize { get; set; }
    }
}