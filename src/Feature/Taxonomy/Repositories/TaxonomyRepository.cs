using System;
using System.Globalization;
using System.Linq;
using OneWeb.Feature.Taxonomy.Models;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using Sitecore.XA.Foundation.Mvc.Wrappers;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.Taxonomy.Repositories
{
    public class TaxonomyRepository :
        VariantsRepository, ITaxonomyRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var tagsModel = new TagModel();
            base.FillBaseProperties(tagsModel);
            var resultPage = this.Rendering.Parameters[OneWebTaxonomy.Fields.SearchResultPage_FieldName];
            var itemUrl = string.Empty;
            if (resultPage != null)
            {
                itemUrl = ItemExtensions.GetItem(resultPage).GetItemUrl();
            }

            tagsModel.Tags = Sitecore.Context.Item
                .GetMultiListItems(Foundation.Serialization.Taxonomy.Fields.Tags_FieldName).Where(x=> !string.IsNullOrWhiteSpace(x.GetFieldValue(BaseContent.Fields.Title_FieldName)))
                .Select(x => new Tag()
                {
                    Title = x.GetFieldValue(BaseContent.Fields.Title_FieldName),
                    Url = itemUrl + "?filters=ow_taglist_sm:" + x.ID.ToShortID().ToString().ToLower()
                });

            tagsModel.DisplayTagCount = DefaultExtensions.GetDefaultDisplayTagCount();
            return tagsModel;
        }
    }
}
