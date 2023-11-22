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
    public class PressReleaseRepository : VariantsRepository, IPressReleaseRepository
    {
        public PressReleaseRepository(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        protected ISearchRepository _searchRepository { get; }
        public PressReleaseListingRenderingModel GetModel(Item item)
        {
            var cardModel = new PressReleaseListingRenderingModel();

            if (StringExtensions.IdEqualsGuid(item.TemplateID, PressReleaseListing.TemplateIdString))
            {
                cardModel.PressReleases = GetOverridePressReleases(item);
            }

            if (cardModel.PressReleases == null || !cardModel.PressReleases.Any())
            {
                cardModel.PressReleases = GetLatestPressReleases();
            }

            FillBaseProperties(cardModel);
            return cardModel;
        }

        private List<PressReleaseModel> GetOverridePressReleases(Item item)
        {
            var pressReleaseList = new List<PressReleaseModel>();
            var overrideItems = item.GetMultiListItems(OneWebPressReleaseListing.Fields.PressReleases_FieldName);
            if (overrideItems.Any())
            {
                foreach (var pressReleaseItem in overrideItems.Where(x=> StringExtensions.IdEqualsGuid(x.TemplateID, PressRelease.TemplateIdString)))
                {
                    var pressReleaseModel = new PressReleaseModel()
                    {
                        Title = pressReleaseItem.GetFieldValue(BaseContent.Fields.Title),
                        Content = pressReleaseItem.GetFieldValue(BaseContent.Fields.Content),
                        Date = pressReleaseItem.GetDateValue(OneWebPressRelease.Fields.Date),
                        CoverImage = new MediaModel(BaseCoverImage.Fields.CoverImage_FieldName),
                        Image = new MediaModel(pressReleaseItem, BaseImage.Fields.Image_FieldName),
                        Link = string.IsNullOrWhiteSpace(
                            pressReleaseItem.GetFieldValue(BaseNavigation.Fields.NavigationUrl_FieldName))
                            ? new LinkModel()
                            {
                                Url = pressReleaseItem.GetItemUrl(),
                            }
                            : new LinkModel(pressReleaseItem, BaseNavigation.Fields.NavigationUrl_FieldName),
                        Taxonomy = new TaxonomyModel(pressReleaseItem),
                        CoverImageClass = CardExtensions.GetCoverImageClass(pressReleaseItem)
                    };

                    pressReleaseList.Add(pressReleaseModel);
                }
            }

            return pressReleaseList;
        }

        private List<PressReleaseModel> GetLatestPressReleases()
        {
            var contentSearchRequest = new ContentParameters<PressReleaseModel>
            {
                CurrentItem = Sitecore.Context.Item,
                PageSize = Convert.ToInt32(RenderingContext.Current.Rendering.Parameters[BaseSearchRenderingParameters.Fields.PageSize_FieldName]),
                PagesToSkip = Convert.ToInt32(RenderingContext.Current.Rendering.Parameters[BaseSearchRenderingParameters.Fields.PagesToSkip_FieldName]),
                OrderBy = new List<Tuple<Expression<Func<PressReleaseModel, object>>, bool>>(){
                            new Tuple<Expression<Func<PressReleaseModel, object>>, bool>(x=> x.Date, false)},
            };
            contentSearchRequest.SearchCondition.TemplateIds.Add(PressRelease.TemplateId);
            string tagId = RenderingContext.Current.Rendering.Parameters[OneWebPressReleaseListingRenderingParameters.Fields.Tag_FieldName];
            if (!string.IsNullOrEmpty(tagId))
            {
                ID tag = new ID(tagId);
                var filterPredicate = PredicateBuilder.True<PressReleaseModel>();
                contentSearchRequest.SearchPredicate = filterPredicate.And(x => x.Tags.Contains(tag));
            }
            if (!(_searchRepository.ContentSearch(contentSearchRequest) is ContentResponse<PressReleaseModel> contentSearchResults))
            {
                return new List<PressReleaseModel>();
            }
            return contentSearchResults.Results.ToList();
        }
    }
}