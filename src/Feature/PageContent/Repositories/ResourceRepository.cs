using Newtonsoft.Json;
using OneWeb.Feature.PageContent.Models;
using OneWeb.Foundation.Search.Models;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Repositories;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
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
    public class ResourceRepository : VariantsRepository, IResourceRepository
    {
        public ResourceRepository(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        protected ISearchRepository _searchRepository { get; }
        public ResourceListingRenderingModel GetModel(Item item)
        {
            var cardModel = new ResourceListingRenderingModel();

            if (StringExtensions.IdEqualsGuid(item.TemplateID, ResourceListing.TemplateIdString))
            {
                cardModel.Resources = GetOverrideResources(item);
            }

            if (cardModel.Resources == null || !cardModel.Resources.Any())
            {
                cardModel.Resources = GetLatestResources();
            }

            FillBaseProperties(cardModel);
            return cardModel;
        }

        private List<ResourceModel> GetOverrideResources(Item item)
        {
            var ResourceList = new List<ResourceModel>();
            var overrideItems = item.GetMultiListItems(OneWebResourceListing.Fields.Resources_FieldName);
            if (overrideItems.Any())
            {
                foreach (var ResourceItem in overrideItems.Where(x=> StringExtensions.IdEqualsGuid(x.TemplateID, Resource.TemplateIdString)))
                {
                    var ResourceModel = new ResourceModel()
                    {
                        Title = ResourceItem.GetFieldValue(BaseContent.Fields.Title),
                        Content = ResourceItem.GetFieldValue(BaseContent.Fields.Content),
                        Date = ResourceItem.GetDateValue(OneWebResource.Fields.Date),
                        CoverImage = new MediaModel(BaseCoverImage.Fields.CoverImage_FieldName),
                        Image = new MediaModel(ResourceItem, BaseImage.Fields.Image_FieldName),
                        Link = string.IsNullOrWhiteSpace(
                            ResourceItem.GetFieldValue(BaseNavigation.Fields.NavigationUrl_FieldName))
                            ? new LinkModel()
                            {
                                Url = ResourceItem.GetItemUrl(),
                            }
                            : new LinkModel(ResourceItem, BaseNavigation.Fields.NavigationUrl_FieldName),
                        Taxonomy = new TaxonomyModel(ResourceItem),
                        CoverImageClass = CardExtensions.GetCoverImageClass(ResourceItem)
                    };

                    ResourceList.Add(ResourceModel);
                }
            }

            return ResourceList;
        }

        private List<ResourceModel> GetLatestResources()
        {
            var contentSearchRequest = new ContentParameters<ResourceModel>
            {
                CurrentItem = Sitecore.Context.Item,
                PageSize = Convert.ToInt32(RenderingContext.Current.Rendering.Parameters[BaseSearchRenderingParameters.Fields.PageSize_FieldName]),
                PagesToSkip = Convert.ToInt32(RenderingContext.Current.Rendering.Parameters[BaseSearchRenderingParameters.Fields.PagesToSkip_FieldName]),
                OrderBy = new List<Tuple<Expression<Func<ResourceModel, object>>, bool>>(){
                            new Tuple<Expression<Func<ResourceModel, object>>, bool>(x=> x.Date, false)},
            };
            contentSearchRequest.SearchCondition.TemplateIds.Add(Resource.TemplateId);
            string tagId = RenderingContext.Current.Rendering.Parameters[OneWebResourceListingRenderingParameters.Fields.Tag_FieldName];
            if (!string.IsNullOrEmpty(tagId))
            {
                ID tag = new ID(tagId);
                var filterPredicate = PredicateBuilder.True<ResourceModel>();
                contentSearchRequest.SearchPredicate = filterPredicate.And(x => x.Tags.Contains(tag));
            }
            if (!(_searchRepository.ContentSearch(contentSearchRequest) is ContentResponse<ResourceModel> contentSearchResults))
            {
                return new List<ResourceModel>();
            }
            return contentSearchResults.Results.ToList();
        }
    }
}