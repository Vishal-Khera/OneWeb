using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.Login.Models
{
    public class UserLoginModel : VariantsRenderingModel
    {
        public LinkModel  Link { get; set; }
        public bool ShowInPopup { get; set; }
        public string FormID { get; set; }
    }
}