using Microsoft.Extensions.DependencyInjection;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Comparers;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.StringExtensions;
using Sitecore.XA.Foundation.Multisite;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using Sitecore.XA.Foundation.Search.ComputedFields;

namespace OneWeb.Foundation.Search.ComputedFields
{
    public class ComputedAggregatedContent : AggregatedContent
    {
        private readonly MediaItemContentExtractor _mediaContentExtractor;

        public ComputedAggregatedContent() => this._mediaContentExtractor = new MediaItemContentExtractor();

        public ComputedAggregatedContent(XmlNode configurationNode) => this._mediaContentExtractor = new MediaItemContentExtractor(configurationNode);

        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item obj1 = (Item)(indexable as SitecoreIndexableItem);
            if (obj1 == null)
                return (object)null;
            if (obj1.Paths.IsMediaItem)
            {
                object obj2 = this._mediaContentExtractor.ComputeFieldValue(indexable) ?? (object)string.Empty;
                string str1 = FormattableString.Invariant(FormattableStringFactory.Create(" {0}", (object)obj1.Name));
                object fieldValue = (object)(obj2.ToString() + str1);
                if (!obj1.DisplayName.IsNullOrEmpty() && obj1.Name != obj1.DisplayName)
                {
                    object obj3 = fieldValue;
                    string str2 = FormattableString.Invariant(FormattableStringFactory.Create(" {0}", (object)obj1.DisplayName));
                    fieldValue = (object)(obj3.ToString() + str2);
                }
                return fieldValue;
            }

            ISet<Item> dataFolders = (ISet<Item>)new HashSet<Item>();
            List<Item> items = new List<Item>() { obj1 };
            if (!obj1.InheritsFrom(Sitecore.XA.Foundation.Search.Templates._SearchableWithoutRelatedItems.ID))
            {
                Item[] objArray = new Item[1]
                {
                    ServiceLocator.ServiceProvider.GetService<IMultisiteContext>().GetDataItem(obj1),
                };
                foreach (Item obj4 in objArray)
                {
                    if (obj4 != null)
                        dataFolders.Add(obj4);
                }
                items.AddRange(base.GetFieldReferences(obj1, dataFolders));
                items.AddRange(base.GetLayoutReferences(obj1, dataFolders));
            }
            int k = 0;
            while (k < items.Count)
            {
                for (; k < items.Count; k++)
                {
                    if (this.ChildrenGroupingTemplateIds.Any<ID>((Func<ID, bool>)(templateId => items[k].Template.DoesTemplateInheritFrom(templateId))))
                    {
                        IEnumerable<Item> unique = base.GetUnique((IEnumerable<Item>)items[k].Children, (IEnumerable<Item>)items);
                        items.AddRange(unique);
                    }
                    else if (AggregatedContent.CompositeTemplateIds.Any<ID>((Func<ID, bool>)(templateId => items[k].Template.DoesTemplateInheritFrom(templateId))))
                    {
                        IEnumerable<Item> unique = base.GetUnique(base.GetLayoutReferences(items[k], dataFolders), (IEnumerable<Item>)items);
                        items.AddRange(unique);
                    }
                }
            }

            var fieldValues = new List<object>();
            try
            {
                ProviderIndexConfiguration config = this.ContentSearchManager.GetIndex(indexable).Configuration;
                fieldValues = items.Distinct(new ItemIdComparer()).SelectMany(i => base.ExtractTextFields(i, config)).ToList() ?? new List<object>();
            }
            catch (Exception ex)
            {
                LogManager.LogError($"Error while parsing index for item - {indexable.AbsolutePath}! ", ex);
            }
            
            if (GetBusinessData(obj1, OneWebBusiness.Fields.Brands_FieldName) is List<object> computedBrands)
            {
                fieldValues.AddRange(computedBrands);
            }

            if (GetBusinessData(obj1, OneWebBusiness.Fields.Industries_FieldName) is List<object> computedIndustries)
            {
                fieldValues.AddRange(computedIndustries);
            }

            if (GetBusinessData(obj1, OneWebBusiness.Fields.Products_FieldName) is List<object> computedProducts)
            {
                fieldValues.AddRange(computedProducts);
            }

            var computedTaxonomy = new ComputedTaxonomy().ComputeFieldValue(indexable);
            if (computedTaxonomy != null)
            {
                fieldValues.Add(computedTaxonomy);
            }

            return fieldValues.Any() ? fieldValues.Select( x=> SitecoreExtensions.Extensions.StringExtensions.SanitizeSearchString((string)x)) : null;
        }

        private List<object> GetBusinessData(Item businessItem, string fieldName)
        {
            var fieldInfoList = new List<object>();
            var businessData = businessItem.GetMultiListItems(fieldName);
            foreach (var data in businessData)
            {
                if(!string.IsNullOrWhiteSpace(data.GetFieldValue(BaseContent.Fields.Title)))
                {
                    fieldInfoList.Add(data.GetFieldValue(BaseContent.Fields.Title));
                }

                if (!string.IsNullOrWhiteSpace(data.GetFieldValue(BaseContent.Fields.Content)))
                {
                    fieldInfoList.Add(data.GetFieldValue(BaseContent.Fields.Content));
                }
            }

            return fieldInfoList.Any() ? fieldInfoList.Distinct().ToList() : null;
        }
    }
}