using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace OneWeb.Foundation.SitecoreExtensions.Models
{
    public class IndustryListModel
    {

        public IndustryListModel(Item item)
        {
            if (StringExtensions.IdEqualsGuid(item.TemplateID, OneWebBusinessIndustry.TemplateIdString))
            {
                MainIndustry = new IndustryModel(item)
                {
                    Name = item.GetFieldValue(BaseContent.Fields.Title),
                    Id = item.ID.ToShortID().ToString(),
                };                
            }

            GetSubIndustries(item);
        }

        public IndustryModel MainIndustry { get; set; }
        public List<IndustryModel> SubIndustries { get; set; }

        private void GetSubIndustries(Item item)
        {
            SubIndustries = new List<IndustryModel>();
            var industries = item.Children.Where(x=> StringExtensions.IdEqualsGuid(x.TemplateID, OneWebBusinessIndustry.TemplateIdString)).ToList();
            foreach (var industry in industries)
            {
                SubIndustries.Add(new IndustryModel(industry));
            }
        }
    }
}