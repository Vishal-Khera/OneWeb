using OneWeb.Feature.PageContent.Models;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Repositories;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.Data.Items;

namespace OneWeb.Feature.PageContent.Repositories
{
    public class RelatedListRepository : VariantsRepository, IRelatedListRepository
    {
        public RelatedListRepository(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        protected ISearchRepository _searchRepository { get; }
        public RelatedListRenderingModel GetModel(Item item)
        {
            var relatedModel = new RelatedListRenderingModel();
            relatedModel.RelatedListing = GetLatestRelatedListing();
            FillBaseProperties(relatedModel);
            return relatedModel;
        }

        private List<RelatedListModel> GetLatestRelatedListing()
        {
            var contentSearchRequest = new ContentParameters<RelatedListModel>
            {
                CurrentItem = Sitecore.Context.Item,
                PageSize = Convert.ToInt32(RenderingContext.Current.Rendering.Parameters[BaseSearchRenderingParameters.Fields.PageSize_FieldName]),
                PagesToSkip = Convert.ToInt32(RenderingContext.Current.Rendering.Parameters[BaseSearchRenderingParameters.Fields.PagesToSkip_FieldName]),
                OrderBy = new List<Tuple<Expression<Func<RelatedListModel, object>>, bool>>(){
                            new Tuple<Expression<Func<RelatedListModel, object>>, bool>(x=> x.Date, false)},
            };
            var currentTagString =
                RenderingContext.Current.Rendering.Parameters[
                    OneWebRelatedListingRenderingParameters.Fields.Tags_FieldName];
            if (string.IsNullOrWhiteSpace(currentTagString))
            {
                currentTagString = CurrentItem.GetFieldValue(Taxonomy.Fields.Tags_FieldName);
            }

            var pageType = RenderingContext.Current.Rendering.Parameters[BasePageCategory.Fields.PageType_FieldName];
            var templateString = RenderingContext.Current.Rendering.Parameters[OneWebRelatedListingRenderingParameters.Fields.Template_FieldName];

            if (!string.IsNullOrWhiteSpace(currentTagString))
            {
                var finalPredicate = PredicateBuilder.True<RelatedListModel>();
                var tagValuePredicate = PredicateBuilder.False<RelatedListModel>();
                foreach (var currentTagValue in currentTagString.Split('|'))
                {
                    ID tag = new ID(currentTagValue);
                    tagValuePredicate = tagValuePredicate.Or(x => x.Tags.Contains(tag));
                }

                finalPredicate = finalPredicate.And(tagValuePredicate);

                var pageTypePredicate = PredicateBuilder.False<RelatedListModel>();
                if (!string.IsNullOrEmpty(pageType))
                {
                    pageTypePredicate = pageTypePredicate.Or(x => x.PageType.Equals(pageType));
                }
                finalPredicate = finalPredicate.And(pageTypePredicate);


                if (!string.IsNullOrWhiteSpace(templateString))
                {
                    var templateIds = templateString.Split('|');
                    if (templateIds.Any())
                    {
                        var templatePredicate = PredicateBuilder.False<RelatedListModel>();
                        foreach (var templateId in templateIds)
                        {
                            if (ID.TryParse(templateId, out var template))
                            {
                                templatePredicate = templatePredicate.Or(x => x.Template.Equals(template));
                            }
                        }
                        finalPredicate = finalPredicate.And(templatePredicate);
                    }
                }
                finalPredicate = finalPredicate.And(x => !string.Equals(x.ItemFullPath, CurrentItem.Paths.FullPath, StringComparison.CurrentCultureIgnoreCase));

                contentSearchRequest.SearchPredicate = finalPredicate;

            }
            if (!(_searchRepository.ContentSearch(contentSearchRequest) is ContentResponse<RelatedListModel> contentSearchResults))
            {
                return new List<RelatedListModel>();
            }
            return contentSearchResults.Results.ToList();
        }
    }
}