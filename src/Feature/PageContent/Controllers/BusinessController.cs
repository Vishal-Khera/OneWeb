using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using OneWeb.Feature.PageContent.Models;
using OneWeb.Feature.PageContent.Repositories;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Models.SolrNetSearch;
using OneWeb.Foundation.Search.Repositories;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class BusinessController : StandardController
    {
        public BusinessController(IBusinessRepository businessRepository, ISearchRepository searchRepository)
        {
            _businessRepository = businessRepository;
            _searchRepository = searchRepository;
        }

        protected IBusinessRepository _businessRepository { get; }
        protected ISearchRepository _searchRepository { get; }

        protected override object GetModel()
        {
            var businessRequest = new BusinessRequest
            {
                Keyword = Request.QueryString["keyword"],
                Scope = Request.QueryString["scope"]
            };
            if (string.IsNullOrWhiteSpace(businessRequest.Scope))
            {
                var currentRendering = RenderingContext.Current.Rendering;
                if (currentRendering != null)
                {
                    var dataSourceItem = RenderingContext.Current.Rendering.Item;
                    //if (dataSourceItem != null) businessRequest.Scope = GetScopePath(dataSourceItem.ID.ToString());
                }
            }

            return _businessRepository.GetModel(businessRequest);
        }

        public string GetData(BusinessRequest businessRequest)
        {
            if (businessRequest == null)
                businessRequest = new BusinessRequest
                {
                    Keyword = Request.QueryString["keyword"],
                    Scope = Request.QueryString["scope"]
                };

            //var scopePath = GetScopePath(businessRequest.Scope);

            //if (!string.IsNullOrWhiteSpace(scopePath)) businessRequest.Scope = scopePath;

            return JsonConvert.SerializeObject(_businessRepository.GetData(businessRequest));
        }

        //private string GetScopePath(string scope)
        //{
        //    if (!string.IsNullOrWhiteSpace(scope))
        //    {
        //        var scopeItem = ItemExtensions.GetItem(StringExtensions.ParseId(scope));
        //        if (scopeItem != null)
        //            if (StringExtensions.IdEqualsGuid(scopeItem.TemplateID,
        //                    OneWebBusinessConfiguration.TemplateIdString))
        //            {
        //                var businessScope =
        //                    scopeItem.GetReferencedItem(OneWebBusinessConfiguration.Fields.Scope_FieldName);
        //                if (businessScope != null) return businessScope.Paths.FullPath;
        //            }
        //    }

        //    return string.Empty;
        //}

        //public string Test()
        //{
        //    var solrRequest = new SolrNetParameters<SolrNetModel>
        //    {
        //        IndexName = "sitecore_master_index",
        //        CurrentItem = Sitecore.Context.Item,
        //        Keyword = Request.QueryString["query"]
        //    };
        //    var autoSuggestResults = _searchRepository.AutoSuggestSearch(solrRequest);

        //    var contentSearchRequest = new ContentParameters<ContentModel>
        //    {
        //        IndexName = "sitecore_master_index",
        //        CurrentItem = Sitecore.Context.Item,
        //        Keyword = Request.QueryString["query"],
        //        PageSize = int.Parse(Request.QueryString["pageSize"]),
        //        PagesToSkip = int.Parse(Request.QueryString["pageSkip"]),
        //        OrderByDescending = bool.Parse(Request.QueryString["desc"])
        //    };

        //    var searchPredicate = PredicateBuilder.True<ContentModel>();
        //    searchPredicate = searchPredicate.And(x => x.Name.Contains(Request.QueryString["query"]));

        //    contentSearchRequest.SearchPredicate = searchPredicate;

        //    var contentSearchResults = _searchRepository.ContentSearch(contentSearchRequest) as ContentResponse<ContentModel>;

        //    return string.Empty;
        //}
    }
}