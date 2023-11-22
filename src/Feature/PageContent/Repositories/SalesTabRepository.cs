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
using System.Web;

namespace OneWeb.Feature.PageContent.Repositories
{
    public class SalesTabRepository : VariantsRepository, ISalesTabRepository
    {
        protected ISearchRepository _searchRepository { get; }
        public SalesTabRepository(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        public SalesTabListModel GetModel()
        {
            List<SalesTabTagModel> salesTabTagModels = new List<SalesTabTagModel>();
            var eventsModel = new SalesTabListModel();
            eventsModel.SalesTab = GetEvents();
            foreach (var salesTab in eventsModel.SalesTab)
            {
                string tagTitle = "";
                if (salesTab.Tags != null)
                {
                    foreach (var tag in salesTab.Tags)
                    {
                        Item Tags = ItemExtensions.GetItem(tag);
                        SalesTabTagModel objSalesTabTagModels = new SalesTabTagModel();
                        objSalesTabTagModels.TagTitle = Tags[OneWebTag.Fields.Title].ToString();
                        tagTitle += " " + Tags[OneWebTag.Fields.Title].ToString();
                        salesTabTagModels.Add(objSalesTabTagModels);
                    }
                    salesTab.Combined = tagTitle;
                }
            }
            eventsModel.SalesTabTag = salesTabTagModels.Distinct().ToList();
            FillBaseProperties(eventsModel);
            return eventsModel;
        }
        private List<SalesTabModel> GetEvents()
        {
            var contentSearchRequest = new ContentParameters<SalesTabModel>
            {
            };
            contentSearchRequest.SearchCondition.TemplateIds.Add(OneWebSalesTab.TemplateId);


            var filterPredicate = PredicateBuilder.True<SalesTabModel>();
            contentSearchRequest.SearchPredicate = filterPredicate;

            if (!(_searchRepository.ContentSearch(contentSearchRequest) is ContentResponse<SalesTabModel> searchResults))
            {
                return new List<SalesTabModel>();
            }
            if (searchResults == null || searchResults.Results == null || searchResults.Results.Count == 0)
                return new List<SalesTabModel>();
            return searchResults.Results.OrderBy(i => i.SortOrder).ThenBy(i => i.Title).ToList();
        }
    }
}