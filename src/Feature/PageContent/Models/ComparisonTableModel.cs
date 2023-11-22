using System.Collections.Generic;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using OneWeb.Foundation.SitecoreExtensions.Models;

namespace OneWeb.Feature.PageContent.Models
{
    public class PropertyModel : VariantsRenderingModel
    {
        public string PropertyName { get; set; }
        public string PropertyId { get; set; }
    }
    public class CardModel : VariantsRenderingModel
    {
        public string CardTitle { get; set; }
        public string CardSubtext { get; set; }
        public string CardContent { get; set; }
        public LinkModel CardLink { get; set; }
        public MediaModel CardImage { get; set; }
        public Dictionary<string,string> CustomPropertyList { get; set; }
        public CardModel()
        {
            CustomPropertyList = new Dictionary<string,string> ();
        }
    }

    public class ComparisonTableModel : VariantsRenderingModel
    {
        public List<PropertyModel> PropertyList { get; set; }
        public List<CardModel> CardList { get; set; }
        public ComparisonTableModel()
        {
            PropertyList = new List<PropertyModel>();
            CardList = new List<CardModel>();
        }
    }
}