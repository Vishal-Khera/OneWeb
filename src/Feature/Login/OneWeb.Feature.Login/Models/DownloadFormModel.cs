using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.Login.Models
{
    public class DownloadFormModel : VariantsRenderingModel
    {
        public string Title { get; set; }

        public string Media { get; set; }

        public string FormId { get; set; }

        public bool VerifyUserIdentity { get; set; }

    }
}