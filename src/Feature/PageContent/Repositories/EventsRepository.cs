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
    public class EventsRepository : VariantsRepository, IEventsRepository
    {
        protected ISearchRepository _searchRepository { get; }
        public EventsRepository(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        public EventsListModel GetModel(Item item)
        {
            var eventsModel = new EventsListModel();
            if (StringExtensions.IdEqualsGuid(item.TemplateID, OneWebEventsFolder.TemplateIdString))
            {
                eventsModel.Events = GetEvents(item);
            }
            FillBaseProperties(eventsModel);
            return eventsModel;
        }
        private List<EventsModel> GetEvents(Item item)
        {
            var contentSearchRequest = new ContentParameters<EventsModel>
            {
                CurrentItem = Sitecore.Context.Item,
                PagesToSkip = Convert.ToInt32(RenderingContext.Current.Rendering.Parameters[BaseSearchRenderingParameters.Fields.PagesToSkip_FieldName]),
                PageSize = Convert.ToInt32(RenderingContext.Current.Rendering.Parameters[BaseSearchRenderingParameters.Fields.PageSize_FieldName]),
            };
            contentSearchRequest.SearchCondition.TemplateIds.Add(OneWebEvents.TemplateId);
            var filterPredicate = PredicateBuilder.True<EventsModel>();
            filterPredicate = filterPredicate.And(x => x.StartDate >= DateTime.Now.Date || x.EndDate >= DateTime.Now.Date);
            contentSearchRequest.SearchPredicate = filterPredicate;
            if (!(_searchRepository.ContentSearch(contentSearchRequest) is ContentResponse<EventsModel> searchResults))
            {
                return new List<EventsModel>();
            }
            if (searchResults == null || searchResults.Results.Count == 0)
                return new List<EventsModel>();
            return searchResults.Results.OrderBy(i => i.StartDate).ThenBy(i => i.EndDate).ToList();
        }
    }
}