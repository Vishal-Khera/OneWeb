using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using System;
using System.Linq;
using OneWeb.Foundation.SitecoreExtensions.Models;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedBusiness : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var businessItem = (Item)(indexable as SitecoreIndexableItem);
            if (!StringExtensions.IdEqualsGuid(businessItem.TemplateID, OneWebBusiness.TemplateIdString))
            {
                return null;
            }
            try
            {
                if (businessItem != null)
                {
                    var jBusiness = new JObject();
                    jBusiness.AddStringAttribute("Title", businessItem, BaseContent.Fields.Title_FieldName);
                    jBusiness.AddStringAttribute("Content", businessItem, BaseContent.Fields.Content_FieldName);
                    jBusiness.Add(new JProperty("Taxonomy", JToken.FromObject(new TaxonomyModel(businessItem))));
                    jBusiness.AddMediaModelAttribute("Image", businessItem, BaseImage.Fields.Image_FieldName);
                    jBusiness.AddMediaModelAttribute("Logo", businessItem, OneWebBusiness.Fields.Logo_FieldName);
                    jBusiness.AddLinkModelAttribute("Link", businessItem, OneWebBusiness.Fields.Link_FieldName);
                    jBusiness.AddStringAttribute("FooterNote", businessItem, OneWebBusiness.Fields.FooterNote_FieldName);
                    jBusiness.AddStringAttribute("CoverImageClass", CardExtensions.GetCoverImageClass(businessItem));

                    GetBusinessBrands(jBusiness, businessItem);
                    GetBusinessProducts(jBusiness, businessItem);
                    GetBusinessIndustries(jBusiness, businessItem);
                    GetBusinessSupports(jBusiness, businessItem);

                    return JsonConvert.SerializeObject(jBusiness);
                }

                return string.Empty;
            }
            catch (Exception exception)
            {
                LogManager.LogError(
                    "Computed Field >> Business" + businessItem.ID + exception.Message, exception);
            }

            return null;
        }

        private void GetBusinessIndustries(JObject jBusiness, Item businessItem)
        {
            var industries = businessItem.GetMultiListItems(OneWebBusiness.Fields.Industries_FieldName);
            if (!industries.Any())
            {
                return;
            }

            var jIndustryArray = new JArray();
            foreach (var industry in industries)
            {
                var jIndustryObject = new JObject();
                jIndustryObject.AddStringAttribute("Title", industry, BaseContent.Fields.Title_FieldName);
                jIndustryObject.AddStringAttribute("Content", industry, BaseContent.Fields.Content_FieldName);
                jIndustryObject.AddStringAttribute("Icon", FieldExtensions.FetchImageAsSVG(industry, BaseIconLink.Fields.Icon_FieldName));
                jIndustryObject.AddLinkModelAttribute("Link", industry, BaseIconLink.Fields.Link_FieldName);
                jIndustryArray.Add(jIndustryObject);
            }

            jBusiness.Add("Industries",
                new JObject(
                    new JProperty("Title", businessItem.GetFieldValue(OneWebBusiness.Fields.IndustriesTitle_FieldName)),
                    new JProperty("List", jIndustryArray)
                ));
        }

        private void GetBusinessBrands(JObject jBusiness, Item businessItem)
        {
            var brands = businessItem.GetMultiListItems(OneWebBusiness.Fields.Brands_FieldName);
            if (!brands.Any())
            {
                return;
            }

            var jBrandArray = new JArray();
            foreach (var brand in brands)
            {
                var jBrandObject = new JObject();
                jBrandObject.AddStringAttribute("Title", brand, BaseContent.Fields.Title_FieldName);
                jBrandObject.AddStringAttribute("Content", brand, BaseContent.Fields.Content_FieldName);
                jBrandObject.AddStringAttribute("Icon", FieldExtensions.FetchImageAsSVG(brand, BaseIconLink.Fields.Icon_FieldName));
                jBrandObject.AddLinkModelAttribute("Link", brand, BaseIconLink.Fields.Link_FieldName);
                jBrandArray.Add(jBrandObject);
            }

            jBusiness.Add("Brands",
                new JObject(
                    new JProperty("Title", businessItem.GetFieldValue(OneWebBusiness.Fields.BrandsTitle_FieldName)),
                    new JProperty("List", jBrandArray)
                ));
        }

        private void GetBusinessProducts(JObject jBusiness, Item businessItem)
        {
            var products = businessItem.GetMultiListItems(OneWebBusiness.Fields.Products_FieldName);
            if (!products.Any())
            {
                return;
            }

            var jProductArray = new JArray();
            foreach (var product in products)
            {
                var jProductObject = new JObject();
                jProductObject.AddStringAttribute("Title", product, BaseContent.Fields.Title_FieldName);
                jProductObject.AddStringAttribute("Content", product, BaseContent.Fields.Content_FieldName);
                jProductObject.AddStringAttribute("Icon", FieldExtensions.FetchImageAsSVG(product, BaseIconLink.Fields.Icon_FieldName));
                jProductObject.AddLinkModelAttribute("Link", product, BaseIconLink.Fields.Link_FieldName);
                jProductArray.Add(jProductObject);
            }

            jBusiness.Add("Products",
                new JObject(
                    new JProperty("Title", businessItem.GetFieldValue(OneWebBusiness.Fields.ProductsTitle_FieldName)),
                    new JProperty("List", jProductArray)
                ));
        }

        private void GetBusinessSupports(JObject jBusiness, Item businessItem)
        {
            var supportGroups = businessItem.GetMultiListItems(OneWebBusiness.Fields.SupportGroups_FieldName).Where(x => StringExtensions.IdEqualsGuid(x.TemplateID, OneWebSupport.TemplateIdString));
            if (!supportGroups.Any())
            {
                return;
            }

            var jSupportArray = new JArray();
            foreach (var support in supportGroups)
            {
                var jSupportObject = new JObject();
                jSupportObject.AddStringAttribute("Title", support, BaseContent.Fields.Title_FieldName);
                jSupportObject.AddStringAttribute("Identifier", support.ShortId());
                var jSupportLinkArray = new JArray();
                foreach (var supportLink in support.Children.Where(x => StringExtensions.IdEqualsGuid(x.TemplateID, OneWebSupportLink.TemplateIdString)))
                {
                    var jSupportLinkObject = new JObject();
                    jSupportLinkObject.AddStringAttribute("Title", supportLink, OneWebSupportLink.Fields.Title_FieldName);
                    jSupportLinkObject.Add(new JProperty("IconClass", supportLink.GetReferencedItem(OneWebSupportLink.Fields.SupportType_FieldName)?.GetFieldValue(OneWebSupportMedia.Fields.Value_FieldName)));
                    jSupportLinkObject.AddLinkModelAttribute("Link", supportLink, BaseIconLink.Fields.Link_FieldName);
                    jSupportLinkArray.Add(jSupportLinkObject);
                }
                jSupportObject.Add(new JProperty("Links", jSupportLinkArray));
                jSupportArray.Add(jSupportObject);
            }

            jBusiness.Add("SupportGroups",
                new JObject(
                    new JProperty("Title", businessItem.GetFieldValue(OneWebBusiness.Fields.SupportGroupsTitle)),
                    new JProperty("List", jSupportArray)
                    ));

            var featuredSupportGroup =
                businessItem.GetReferencedItem(OneWebBusiness.Fields.FeaturedSupportGroup_FieldName);
            if (featuredSupportGroup != null)
            {
                var featuredSupportLinkArray = new JArray();
                foreach (var supportLink in featuredSupportGroup.Children.Where(x => StringExtensions.IdEqualsGuid(x.TemplateID, OneWebSupportLink.TemplateIdString)))
                {
                    var jFeaturedSupportLinkOject = new JObject();
                    jFeaturedSupportLinkOject.AddStringAttribute("Title", supportLink, OneWebSupportLink.Fields.Title_FieldName);
                    jFeaturedSupportLinkOject.Add(new JProperty("IconClass", supportLink.GetReferencedItem(OneWebSupportLink.Fields.SupportType_FieldName)?.GetFieldValue(OneWebSupportMedia.Fields.Value_FieldName)));
                    jFeaturedSupportLinkOject.AddLinkModelAttribute("Link", supportLink, BaseIconLink.Fields.Link_FieldName);
                    featuredSupportLinkArray.Add(jFeaturedSupportLinkOject);
                }
                jBusiness.Add("FeaturedSupportGroup",
                    new JObject(
                        new JProperty("Title", businessItem.GetFieldValue(OneWebBusiness.Fields.FeaturedSupportGroupTitle)),
                        new JProperty("List", featuredSupportLinkArray)
                    ));
            }
        }
    }
}