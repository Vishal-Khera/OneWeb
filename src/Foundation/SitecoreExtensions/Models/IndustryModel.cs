using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.SitecoreExtensions.Models
{
    public class IndustryModel
    {

        public IndustryModel(Item item)
        {
            if(StringExtensions.IdEqualsGuid(item.TemplateID, OneWebBusinessIndustry.TemplateIdString))
            {
                Name = item.GetFieldValue(BaseContent.Fields.Title);
                Id = item.ID.ToShortID().ToString();
            }
        }

        public string Name { get; set; }
        public string Id { get; set; }
    }
}