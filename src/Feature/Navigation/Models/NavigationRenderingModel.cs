using System.Collections.Generic;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;

namespace OneWeb.Feature.Navigation.Models
{
    public class NavigationRenderingModel : VariantsRenderingModel
    {
        public virtual IList<NavigationModel> Children { get; set; }
        public int Levels { get; set; }
        public virtual string Name { get; set; }
        public bool IsFake { get; set; }
        public string Current { get; set; }
    }
}