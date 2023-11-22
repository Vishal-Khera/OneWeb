using OneWeb.Feature.PageContent.Models;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Repositories;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneWeb.Feature.PageContent.Repositories
{
    public class ComparisonTableRepository : VariantsRepository, IComparisonTableRepository
    {
        public ComparisonTableModel GetModel(Item item)
        {
           // Item item = Sitecore.Context.Item;
            ComparisonTableModel comparisonTable = new ComparisonTableModel();
            CardModel cardData = null;
            PropertyModel propertyData = null;
            if (item != null)
            {
                var propertyValues = item.GetMultiListFieldValues(OneWebComparisonTable.Fields.PropertyList_FieldName);
                var cardValues = item.GetMultiListFieldValues(OneWebComparisonTable.Fields.CardList_FieldName);
                if (propertyValues.Any())
                {
                    foreach (var propertyValue in propertyValues)
                    {
                        Item propertyDataValue = ItemExtensions.GetItem(propertyValue);
                        propertyData = new PropertyModel
                      {
                        PropertyName = propertyDataValue.GetFieldValue(ParameterHeading.Fields.Name_FieldName),
                        PropertyId  = propertyValue.ToString()            
                      };
                        if (propertyData != null)
                            comparisonTable.PropertyList.Add(propertyData);
                    }
                }
                if (cardValues.Any())
                {
                    foreach (var cardValue in cardValues)
                    {
                        Item cardDataValue = ItemExtensions.GetItem(cardValue);
                        cardData = new CardModel
                        {
                            CardTitle = cardDataValue.GetFieldValue(BaseContent.Fields.Title_FieldName),
                            CardSubtext = cardDataValue.GetFieldValue(BaseContent.Fields.Subtext_FieldName),
                            CardContent = cardDataValue.GetFieldValue(BaseContent.Fields.Content_FieldName),
                            CardImage = cardDataValue.GetMediaModel(BaseImage.Fields.Image_FieldName),
                            CardLink = cardDataValue.GetLinkFieldModel(BaseCta.Fields.PrimaryLink_FieldName),
                            CustomPropertyList = cardDataValue.GetOneWebNameValueList(OneWebComparisonCards.Fields.Property_FieldName)
                        };
                        comparisonTable.CardList.Add(cardData);
                    }
                }
            }
            if (comparisonTable != null)
                FillBaseProperties(comparisonTable);
            return comparisonTable;
        }
    }
}