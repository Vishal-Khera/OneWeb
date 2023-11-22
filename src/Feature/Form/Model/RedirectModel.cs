using System;

namespace OneWeb.Feature.Form.Model
{
    public partial class RedirectModel
    {
        public Guid InternalItemId { get; set; }
        public string ExternalLink { get; set; }
        public string Anchor { get; set; }
        public string QueryString { get; set; }
    }
}