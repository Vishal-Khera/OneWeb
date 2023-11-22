using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OneWeb.Foundation.Search.Extensions;
using OneWeb.Foundation.Search.Models;
using OneWeb.Foundation.Search.Models.AutoSuggest;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Models.Facets;
using OneWeb.Foundation.Search.Models.SolrNetSearch;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Security;
using Sitecore.ContentSearch.SolrProvider.SolrNetIntegration;
using Sitecore.Web;
using SolrNet;

namespace OneWeb.Foundation.Search.Service
{
    public class SearchService
    {
        public List<T> InitiateSearch<T>(string indexName, Expression<Func<T, bool>> predicate)
        {
            ISearchIndex index = null;
            try
            {
                index = ContentSearchManager.GetIndex(indexName);
            }
            catch (Exception ex)
            {
                LogManager.LogError($"Index - {indexName} not found! ", ex);
            }

            if (index == null)
                return new List<T>();

            using (var context = index.CreateSearchContext(SearchSecurityOptions.EnableSecurityCheck))
            {
                return context.GetQueryable<T>().Where(predicate).ToList();
            }
        }

        public ContentResponse<T> InitiateSearch<T>(ContentParameters<T> parameters) where T : ContentModel
        {
            var contentResponse = new ContentResponse<T>();
            if (parameters.Index == null)
                return contentResponse;

            using (var context = parameters.Index.CreateSearchContext(SearchSecurityOptions.EnableSecurityCheck))
            {
                var queryResults = context.GetQueryable<T>().Where(parameters.SearchPredicate);

                if (parameters.OrderBy != null)
                {
                    queryResults = parameters.OrderBy.Aggregate(queryResults, (current, orderCondition) => orderCondition.Item2 ? current.OrderBy(orderCondition.Item1) : current.OrderByDescending(orderCondition.Item1));
                }

                if (parameters.FacetOn != null)
                {
                    queryResults = parameters.FacetOn.Aggregate(queryResults, (current, facetCondition) => facetCondition.Item2 > 0 ? current.FacetOn(facetCondition.Item1, facetCondition.Item2) : current.FacetOn(facetCondition.Item1, 1));
                }
                
                queryResults = queryResults.Skip(parameters.PageSize * (int) parameters.PagesToSkip).Take(parameters.PageSize);

                var searchResults = queryResults.GetResults();
                if (searchResults == null)
                    return contentResponse;
                contentResponse = new ContentResponse<T>
                {
                    Parameters = PopulateResponseParameters(searchResults, parameters.PageSize, (int)parameters.PagesToSkip),
                    Results = searchResults.Select(x => x.Document).ToList(),
                    Facets = searchResults.Facets.Categories.SelectMany(x =>
                    {
                        var facetList = new List<OneWebFacet>();
                        var facetItems = SearchExtensions.GetFacetItemBySolrField(parameters, x.Name);

                        foreach (var facetItem in facetItems)
                        {
                            var facet = new OneWebFacet(facetItem)
                            {
                                Values = x.Values.Select(y =>
                                    new OneWebFacetValue()
                                    {
                                        Name = y.Name,
                                        Count = y.AggregateCount,
                                    }).ToList()
                            };

                            facetList.Add(facet);
                        }

                        return facetList;
                    }).ToList(),
                };

                contentResponse.Parameters.SearchTerm = parameters.Keyword;
                contentResponse.Parameters.CurrentItemId = parameters.CurrentItem?.ID.ToShortID().ToString();
            }

            return contentResponse;
        }
        public SolrNetResponse<T> InitiateSearch<T>(SolrNetParameters<T> parameters) where T : SolrNetModel
        {
            if (parameters.Index == null)
                return new SolrNetResponse<T>();

            var solrNetResponse = new SolrNetResponse<T>();
            using (var context = parameters.Index.CreateSearchContext(SearchSecurityOptions.EnableSecurityCheck))
            {
                parameters.QueryOptions.Rows = parameters.PageSize;
                parameters.QueryOptions.StartOrCursor =
                    new StartOrCursor.Start(parameters.PagesToSkip * parameters.PageSize);
                var queryResult = context.Query<T>(parameters.Query, parameters.QueryOptions);
                if (queryResult != null)
                {
                    solrNetResponse = new SolrNetResponse<T>
                    {
                        Results = queryResult,
                        Facets = queryResult.FacetFields.SelectMany(x =>
                        {
                            var facetList = new List<OneWebFacet>();
                            var facetItems = SearchExtensions.GetFacetItemBySolrField(parameters, x.Key);

                            foreach (var facetItem in facetItems)
                            {
                                var facet = new OneWebFacet(facetItem)
                                {
                                    Values = x.Value.Select(y =>
                                        new OneWebFacetValue()
                                        {
                                            Name = y.Key,
                                            Count = y.Value,
                                        }).ToList()
                                };

                                facetList.Add(facet);
                            }

                            return facetList;
                        }).ToList(),
                        Parameters = PopulateResponseParameters(queryResult, parameters.PageSize, parameters.PagesToSkip),
                    };

                    solrNetResponse.Parameters.SearchTerm = parameters.Keyword;
                    solrNetResponse.Parameters.CurrentItemId = parameters.CurrentItem?.ID.ToShortID().ToString();
                }
            }

            return solrNetResponse;
        }

        public AutoSuggestResponse<T> InitiateSearch<T>(AutoSuggestParameters<T> parameters) where T : AutoSuggestModel
        {
            if (parameters.Index == null)
                return new AutoSuggestResponse<T>();

            var autoSuggestResponse = new AutoSuggestResponse<T>();
            using (var context = parameters.Index.CreateSearchContext(SearchSecurityOptions.EnableSecurityCheck))
            {
                parameters.QueryOptions.Rows = parameters.PageSize;
                parameters.QueryOptions.StartOrCursor =
                    new StartOrCursor.Start(parameters.PagesToSkip * parameters.PageSize);
                var queryResult = context.Query<T>(parameters.Query, parameters.QueryOptions);
                if (queryResult != null)
                {
                    autoSuggestResponse = new AutoSuggestResponse<T>();
                    autoSuggestResponse.Keyword = parameters.Keyword;
                    var searchGroups = queryResult?.Grouping?.Values?.FirstOrDefault()?.Groups;
                    var autoSuggestTemplates = SearchExtensions.GetEnabledTemplates(parameters.CurrentItem, OneWebAutoSuggestSearchConfiguration.Fields.EnabledTemplates_FieldName, true);
                    var groupingParameter = WebUtil.ParseQueryString(
                        parameters.CurrentItem.GetFieldValue(BaseSolrNetSearchConfiguration.Fields.Grouping_FieldName));
                    if (searchGroups != null)
                    {
                        foreach (var group in searchGroups)
                        {
                            if (group.Documents.Any())
                            {
                                var autoSuggestGroup = new AutoSuggestGroup<T>()
                                {
                                    GroupIdentifier = groupingParameter.FirstOrDefault().Key,
                                    GroupLimit = groupingParameter.FirstOrDefault().Value,
                                    Identifier = group.GroupValue,
                                    Name = autoSuggestTemplates[group.GroupValue],
                                    Count = group.NumFound,
                                    Results = group.Documents.ToList(),
                                };
                                autoSuggestResponse.Groups.Add(autoSuggestGroup);
                                autoSuggestResponse.SuggestedSearches =
                                    SearchExtensions.GetSuggestedSearches(parameters.CurrentItem);
                            }
                        }
                    }
                }
            }

            return autoSuggestResponse;
        }

        private static ResponseParameters PopulateResponseParameters<T>(SearchResults<T> searchResults, int pageSize, int pagesToSkip)
        {
            var searchResultsIterated = searchResults.TotalSearchResults > pageSize * (pagesToSkip + 1)
                ? pageSize * (pagesToSkip + 1)
                : searchResults.TotalSearchResults;

            return new ResponseParameters()
            {
                TotalResultCount = searchResults.TotalSearchResults,
                TotalPageCount = (searchResults.TotalSearchResults + pageSize - 1) / pageSize,
                CurrentResultCount = searchResults.Count(),
                CurrentPageCount = searchResults.Count() / pageSize,
                IteratedResultCount = searchResultsIterated,
                IteratedPageCount = (int)Math.Ceiling((double)searchResultsIterated / pageSize),
                RemainingResultCount = searchResults.TotalSearchResults - searchResultsIterated,
                RemainingPageCount = (searchResults.TotalSearchResults - searchResultsIterated + pageSize - 1) / pageSize,
            };
        }
        private static ResponseParameters PopulateResponseParameters<T>(SolrQueryResults<T> searchResults, int pageSize, int pagesToSkip)
        {
            var searchResultsIterated = searchResults.NumFound > pageSize * (pagesToSkip + 1)
                ? pageSize * (pagesToSkip + 1)
                : searchResults.NumFound;

            return new ResponseParameters()
            {
                TotalResultCount = searchResults.NumFound,
                TotalPageCount = (searchResults.NumFound + pageSize - 1) / pageSize,
                CurrentResultCount = searchResults.Count(),
                CurrentPageCount = searchResults.Count() / pageSize,
                IteratedResultCount = searchResultsIterated,
                IteratedPageCount = (int)Math.Ceiling((double)searchResultsIterated / pageSize),
                RemainingResultCount = searchResults.NumFound - searchResultsIterated,
                RemainingPageCount = (searchResults.NumFound - searchResultsIterated + pageSize - 1) / pageSize,
            };
        }

    }
}