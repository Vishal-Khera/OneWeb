using Sitecore.Data.Items;
using Sitecore.XA.Foundation.DynamicPlaceholders.Models;
using System.Collections.Generic;

namespace OneWeb.Feature.PageContent.Models
{
    public class AccordionSettings
    {
        public int Speed { get; set; }

        public string Easing { get; set; }

        public bool CanOpenMultiple { get; set; }

        public bool CanToggle { get; set; }

        public bool ExpandOnHover { get; set; }

        public bool ExpandedByDefault { get; set; }
    }
}