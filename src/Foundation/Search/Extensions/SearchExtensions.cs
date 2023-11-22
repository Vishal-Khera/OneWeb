using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Channels;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using OneWeb.Foundation.Search.Models;
using OneWeb.Foundation.Search.Models.AutoSuggest;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Models.SolrNetSearch;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using Sitecore.XA.Foundation.Search.Services;
using SolrNet;
using SolrNet.Commands.Parameters;
using StringExtensions = OneWeb.Foundation.SitecoreExtensions.Extensions.StringExtensions;

namespace OneWeb.Foundation.Search.Extensions
{
    public class SearchExtensions
    {
        private static readonly ISearchQueryTokenResolver QueryTokenResolver =
            ServiceLocator.ServiceProvider.GetService<ISearchQueryTokenResolver>();
        public static string ParseFieldName(ISearchIndex index, string fieldName)
        {
            return index != null && (fieldName != null && !fieldName.Contains("_")) ? index.FieldNameTranslator.GetIndexFieldName(fieldName) : fieldName;
        }
        public static List<Item> GetFacetItemBySolrField(IBaseParameters parameters, string solrField)
        {
            return parameters.CurrentItem
                .GetMultiListItems(BaseSearchConfiguration.Fields.Filters_FieldName).Where(y =>
                    y.GetFieldValue(OneWebSearchFilter.Fields.FieldName_FieldName)
                        .Equals(ParseFieldName(parameters.Index, solrField), StringComparison.InvariantCultureIgnoreCase) ||
                    y.GetFieldValue(OneWebSearchFilter.Fields.FieldName_FieldName).ToLower()
                        .Contains(ParseFieldName(parameters.Index, solrField).ToLower())).ToList();
        }
        public static void ParseContextIndex(IBaseParameters parameters)
        {
            if (parameters.Index != null)
            {
                return;
            }

            var indexName = string.Empty;
            if (!string.IsNullOrWhiteSpace(parameters.IndexName))
            {
                indexName = parameters.IndexName;
            }
            else
            {
                if (string.IsNullOrEmpty(indexName))
                {
                    var additionalParameter = Context.Database?.Name.ToLower() == "master" ? "_ow_cms" : "_ow";
                    var contextSiteName = Context.Site?.Name;
                    if (!string.IsNullOrEmpty(contextSiteName))
                    {
                        indexName = contextSiteName.Split('-')[0].ToLower() + additionalParameter;
                    }
                    else
                    {
                        if (parameters.CurrentItem != null)
                        {
                            var currentSite = Factory.GetSiteInfoList().Where(s =>
                                    s.RootPath != "" && parameters.CurrentItem.Paths.Path.ToLower()
                                        .StartsWith(s.RootPath.ToLower()))
                                .OrderByDescending(s => s.RootPath.Length)
                                .FirstOrDefault();
                            var siteName = currentSite?.Name;
                            if (!string.IsNullOrEmpty(siteName))
                                indexName = siteName.ToLower() + additionalParameter;
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(indexName))
            {
                try
                {
                    parameters.Index = ContentSearchManager.GetIndex(indexName);
                }
                catch (Exception ex)
                {
                    LogManager.LogError($"Index - {indexName} not found! ", ex );
                }
            }
            parameters.IndexName = indexName;
        }
        public static void ParseParameters(IBaseParameters parameters)
        {
            var scopeFieldValue = parameters.CurrentItem.GetFieldValue(BaseSearchConfiguration.Fields.Scope);
            if (!string.IsNullOrWhiteSpace(scopeFieldValue))
            {
                parameters.Scope = scopeFieldValue.Split('|').Select(ID.Parse).ToList();
            }

            if (parameters.PageSize == 0)
            {
                parameters.PageSize = int.TryParse(
                    parameters.CurrentItem.GetFieldValue(BaseSearchRenderingParameters.Fields.PageSize_FieldName),
                    out var pageSize)
                    ? pageSize
                    : DefaultExtensions.GetDefaultSearchPageSize();
            }

            parameters.Keyword = StringExtensions.SanitizeSearchString(parameters.Keyword);
        }
        public static void ContentParseParameters<T>(ContentParameters<T> parameters) where T : ContentModel
        {
            var overrideQuery = parameters.CurrentItem.GetFieldValue(OneWebContentSearchConfiguration.Fields.Query);
            if (!string.IsNullOrWhiteSpace(overrideQuery))
            {
                ContentSetQueryParameters(parameters, true);
                ContentSetQueryParameters(parameters, false);
            }

            var orderByFieldValue = WebUtil.ParseQueryString(parameters.CurrentItem.GetFieldValue(BaseSearchConfiguration.Fields.OrderBy));
            if (orderByFieldValue != null && orderByFieldValue.Any())
            {
                parameters.OrderBy = new List<Tuple<Expression<Func<T, object>>, bool>>();
                foreach (var searchFieldValue in orderByFieldValue)
                {
                    Tuple<Expression<Func<T, object>>, bool> orderCondition;
                    switch (searchFieldValue.Key)
                    {
                        case "Date":
                            orderCondition = new Tuple<Expression<Func<T, object>>, bool>(x => x.PublishedDate, searchFieldValue.Value.ToLower() == "asc");
                            break;
                        default:
                            orderCondition = new Tuple<Expression<Func<T, object>>, bool>(x => x[searchFieldValue.Key], searchFieldValue.Value.ToLower() == "asc");
                            break;
                    }
                    parameters.OrderBy.Add(orderCondition);
                }
            }

            var filterFieldValue = parameters.CurrentItem.GetMultiListItems(BaseSearchConfiguration.Fields.Filters_FieldName);
            if (filterFieldValue != null && filterFieldValue.Any())
            {
                parameters.FacetOn = new List<Tuple<Expression<Func<T, object>>, int>>();
                foreach (var filterValue in filterFieldValue)
                {
                    if (filterValue.GetCheckboxStatus(OneWebSearchFilter.Fields.Enabled_FieldName))
                    {
                        var facetCondition = new Tuple<Expression<Func<T, object>>, int>(x => x[filterValue.GetFieldValue(OneWebSearchFilter.Fields.FieldName_FieldName)], filterValue.GetInteger(OneWebSearchFilter.Fields.MinimumCount));
                        parameters.FacetOn.Add(facetCondition);
                    }
                }
            }
        }
        public static void ContentSetQueryParameters<T>(ContentParameters<T> parameters, bool enforce) where T : ContentModel
        {
            var overrideQuery = parameters.CurrentItem.GetFieldValue(OneWebContentSearchConfiguration.Fields.Query);
            var query = enforce ? PredicateBuilder.True<T>() : PredicateBuilder.False<T>();
            if (!string.IsNullOrWhiteSpace(overrideQuery))
            {
                var queryConditions = QueryTokenResolver.Resolve(SearchStringModel.ExtractSearchQuery(overrideQuery), parameters.CurrentItem).ToList();
                var keyword = parameters.Keyword;
                if (string.IsNullOrWhiteSpace(keyword) && parameters.CurrentItem.GetCheckboxStatus(BaseSearchConfiguration.Fields
                        .ShowResultsByDefault_FieldName))
                {
                    keyword = "*";
                }

                foreach (var queryCondition in queryConditions.Where(x => enforce ? x.Operation == "must" : x.Operation != "must"))
                {
                    switch (queryCondition.Type)
                    {
                        case "template":
                            query = enforce
                                ? query.And(x => x.Template == ID.Parse(queryCondition.Value))
                                : query.Or(x => x.Template == ID.Parse(queryCondition.Value));
                            break;
                        case "location":
                            query = enforce
                                ? query.And(x =>
                                    x.Paths.Contains(ID.Parse(queryCondition.Value)))
                                : query.Or(x =>
                                    x.Paths.Contains(ID.Parse(queryCondition.Value)));
                            break;
                        case "ow_field_boost":
                            if (string.IsNullOrWhiteSpace(keyword))
                                break;
                            var fieldBoostValues = queryCondition.Value.Split('|');
                            var fieldName = ParseFieldName(parameters.Index, fieldBoostValues[0]);
                            var fieldBoost = fieldBoostValues[1];
                            var boostValue = float.Parse(fieldBoost);
                            
                            query = enforce
                                ? query.And(x => x[fieldName]
                                    .Contains(keyword)).Boost(boostValue * boostValue)
                                : query.Or(x => x[fieldName]
                                    .Contains(keyword)).Boost(boostValue * boostValue);
                            break;
                        case "ow_field_value_boost":
                            var fieldValueBoostValues = queryCondition.Value.Split('|');
                            var fieldValueName = ParseFieldName(parameters.Index, fieldValueBoostValues[0]);
                            var fieldValueBoost
                                = fieldValueBoostValues[2];
                            var fieldValueValue = fieldValueBoostValues[1];
                            query = enforce
                                ? query.And(x =>
                                    x[fieldValueName].Contains(fieldValueValue)).Boost(float.Parse(fieldValueBoost))
                                : query.Or(x =>
                                    x[fieldValueName].Contains(fieldValueValue)).Boost(float.Parse(fieldValueBoost));
                            break;
                        case "ow_field_equal_value_boost":
                            if (string.IsNullOrWhiteSpace(keyword))
                                break;
                            var fieldEqualValueBoostValues = queryCondition.Value.Split('|');
                            var fieldEqualValueName = ParseFieldName(parameters.Index, fieldEqualValueBoostValues[0]);
                            var fieldEqualValueBoost = fieldEqualValueBoostValues[1];
                            var boostEqualValue = float.Parse(fieldEqualValueBoost);
                            query = enforce
                                        ? query.And(x => x[fieldEqualValueName]
                                            .Equals(keyword)).Boost(boostEqualValue * boostEqualValue)
                                        : query.Or(x => x[fieldEqualValueName]
                                            .Equals(keyword)).Boost(boostEqualValue * boostEqualValue);
                            break;
                        case "custom":
                            var customValues = queryCondition.Value.Split('|');
                            var customFieldName = ParseFieldName(parameters.Index, customValues[0]);
                            var customFieldValue = customValues[1];
                            query = enforce
                                ? query.And(x =>
                                x[customFieldName].Contains(customFieldValue))
                                : query.Or(x =>
                                    x[customFieldName].Contains(customFieldValue));
                            break;
                        case "text":
                            query = enforce
                                ? query.And(x =>
                                x.AllContent.Contains(queryCondition.Value))
                                : query.Or(x =>
                                    x.AllContent.Contains(queryCondition.Value)); ;
                            break;
                    }
                }
            }

            parameters.SearchPredicate = parameters.SearchPredicate.And(query);
        }
        public static void ContentExecuteParameters<T>(ContentParameters<T> parameters) where T : ContentModel
        {
            var searchConditions = parameters.SearchCondition;

            var queryPredicate = PredicateBuilder.True<T>();
            var itemIdPredicate = PredicateBuilder.True<T>();
            foreach (var itemId in searchConditions.ItemIds)
                itemIdPredicate = itemIdPredicate.And(x => x.ItemId == itemId);
            queryPredicate = queryPredicate.And(itemIdPredicate);

            var itemNamePredicate = PredicateBuilder.True<T>();
            foreach (var itemName in searchConditions.ItemNames)
                itemNamePredicate = itemNamePredicate.And(x => x.Name == itemName);
            queryPredicate = queryPredicate.And(itemNamePredicate);

            var templateIdPredicate = PredicateBuilder.True<T>();
            foreach (var templateId in searchConditions.TemplateIds)
                templateIdPredicate = templateIdPredicate.And(x => x.Template == templateId);
            queryPredicate = queryPredicate.And(templateIdPredicate);

            var templateNamePredicate = PredicateBuilder.True<T>();
            foreach (var templateName in searchConditions.TemplateNames)
                templateNamePredicate = templateNamePredicate.And(x => x.TemplateName == templateName);
            queryPredicate = queryPredicate.And(templateNamePredicate);

            var hideInSearchPredicate = PredicateBuilder.True<T>();
            hideInSearchPredicate = hideInSearchPredicate.And(x => !x.HideFromSearch);
            queryPredicate = queryPredicate.And(hideInSearchPredicate);

            if (searchConditions.FieldValueConditions.Any() && !string.IsNullOrWhiteSpace(parameters.Keyword))
            {
                var fieldValuePredicate = PredicateBuilder.False<T>();
                foreach (var fieldValueCondition in searchConditions.FieldValueConditions)
                {
                    fieldValuePredicate = fieldValuePredicate
                        .Or(x => x[ParseFieldName(parameters.Index, fieldValueCondition.FieldName)] == parameters.Keyword)
                        .Boost(50f);

                }
                queryPredicate = queryPredicate.And(fieldValuePredicate);
            }

            if (parameters.Scope != null)
            {
                var locationPredicate = PredicateBuilder.True<T>();
                locationPredicate = parameters.Scope.Aggregate(locationPredicate, (current, currentScope) => current.And(x => x.Paths.Contains(currentScope)));
                queryPredicate = queryPredicate.And(locationPredicate);
            }

            parameters.SearchPredicate = parameters.Language != null ?
                parameters.SearchPredicate.And(x => x.LatestVersion && x.ItemLanguage.Equals(parameters.Language.Name)) :
                parameters.SearchPredicate.And(x => x.LatestVersion && x.ItemLanguage.Equals(Context.Language.Name));

            parameters.SearchPredicate = parameters.SearchPredicate.And(queryPredicate);
        }
        public static NameValueCollection GetEnabledTemplates(Item item, string fieldName, bool isAutoSuggest = false)
        {
            var defaultResponse = new NameValueCollection();
            if (isAutoSuggest)
            {
                var fieldValue = item.GetFieldValue(fieldName);
                if (string.IsNullOrWhiteSpace(fieldValue))
                {
                    return defaultResponse;
                }

                var templateCollection = HttpUtility.ParseQueryString(fieldValue);

                foreach (var key in templateCollection.AllKeys)
                {
                    defaultResponse.Add(StringExtensions.RemoveSpecialCharacters(key, true), templateCollection[key]);
                }
            }

            var enabledTemplates = item.GetMultiListFieldValues(OneWebSolrNetSearchConfiguration.Fields.EnabledTemplates_FieldName);
            if (enabledTemplates.Any())
            {
                var boostValue = enabledTemplates.Length;
                foreach (var enabledTemplate in enabledTemplates)
                {
                    boostValue--;
                    defaultResponse.Add(StringExtensions.RemoveSpecialCharacters(enabledTemplate, true), (Math.Pow(10, boostValue)).ToString());
                }
            }

            return defaultResponse;
        }
        public static void SolrNetParseParameters<T>(SolrNetParameters<T> parameters) where T : SolrNetModel
        {
            var solrQueryValue = parameters.CurrentItem.GetFieldValue(BaseSolrNetSearchConfiguration.Fields.Query);
            if (string.IsNullOrWhiteSpace(solrQueryValue))
            {
                return;
            }

            var keywordQuery = new SolrQuery($"({SolrNetGetKeywordQuery(solrQueryValue, parameters.Keyword, parameters.CurrentItem.GetCheckboxStatus(BaseSolrNetSearchConfiguration.Fields.ExactSearch_FieldName), parameters.CurrentItem.GetCheckboxStatus(BaseSearchConfiguration.Fields.ShowResultsByDefault_FieldName))})");

            var orderByFieldValue = WebUtil.ParseQueryString(parameters.CurrentItem.GetFieldValue(BaseSearchConfiguration.Fields.OrderBy));
            if (orderByFieldValue != null && orderByFieldValue.Any())
            {
                parameters.QueryOptions.OrderBy =
                    orderByFieldValue.Select(x => SortOrder.Parse($"{x.Key} {x.Value}")).ToArray();
            }

            var filterFieldValue = parameters.CurrentItem.GetMultiListItems(BaseSearchConfiguration.Fields.Filters_FieldName);
            if (filterFieldValue != null && filterFieldValue.Any())
            {
                parameters.QueryOptions.Facet = new FacetParameters()
                {
                    MinCount = 1,
                    Limit = 5,
                    Queries = filterFieldValue
                        .Where(x => x.GetCheckboxStatus(OneWebSearchFilter.Fields.Enabled_FieldName))
                            .Select(y => (ISolrFacetQuery)new SolrFacetFieldQuery(y.GetFieldValue(OneWebSearchFilter.Fields.FieldName_FieldName))).ToList(),
                };
            }

            var groupingFieldValue = WebUtil.ParseQueryString(parameters.CurrentItem.GetFieldValue(BaseSolrNetSearchConfiguration.Fields.Grouping));
            if (groupingFieldValue != null && groupingFieldValue.Any())
            {
                parameters.QueryOptions.Grouping = new GroupingParameters()
                {
                    Fields = new[] { groupingFieldValue.FirstOrDefault().Key },

                    Limit = int.TryParse(groupingFieldValue.FirstOrDefault().Value, out var limit) ? limit : 10,
                };
            }

            var enabledTemplates = GetEnabledTemplates(parameters.CurrentItem, OneWebSolrNetSearchConfiguration.Fields.EnabledTemplates_FieldName);
            var templateQuery = new SolrQuery(string.Empty);
            if (enabledTemplates != null && enabledTemplates.HasKeys())
            {
                templateQuery = new SolrQuery("(" + string.Join(" OR ", enabledTemplates.AllKeys.Select(x =>
                    $"_template: ({x})^{long.Parse(enabledTemplates[x])}")) + ")");
            }

            var hideFromSearchQuery = new SolrQueryByField(FieldNames.HideFromSearch, "false");

            var presetQuery = new SolrQuery(string.Empty);
            if (!string.IsNullOrWhiteSpace(parameters.PresetQuery))
            {
                presetQuery = new SolrQuery(parameters.PresetQuery);
            }

            var latestVersionQuery = new SolrQueryByField(FieldNames.ItemIsLatestVersion, "true");
            var languageQuery = parameters.Language != null ?
     new SolrQueryByField(FieldNames.ItemLanguage, parameters.Language.Name) :
     new SolrQueryByField(FieldNames.ItemLanguage, Context.Language.Name);

            parameters.Query = keywordQuery && templateQuery && presetQuery && hideFromSearchQuery && latestVersionQuery && languageQuery;
        }
        private static string SolrNetGetKeywordQuery(string solrQuery, string keyword, bool isExactSearch, bool showResultsByDefault)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                if (showResultsByDefault)
                {
                    solrQuery = solrQuery.Replace("{{keyword}}", "*");
                }

                solrQuery = solrQuery.Replace("***", "*");
            }
            else
            {
                var defaultQuery = solrQuery;
                var splitKeywords = keyword.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                var completeKeywordQuery = "(" + solrQuery.Replace("{{keyword}}", keyword) + ")^100";
                solrQuery =
                    $"{completeKeywordQuery} OR {string.Join(isExactSearch ? " AND " : " OR ", splitKeywords.Select(x => defaultQuery.Replace("{{keyword}}", x)).ToList())}"; ;
            }

            return solrQuery;
        }
        public static void AutoSuggestCreatePredicate<T>(AutoSuggestParameters<T> parameters)
        {
            var autoSuggestTemplates = GetEnabledTemplates(parameters.CurrentItem, OneWebAutoSuggestSearchConfiguration.Fields.EnabledTemplates_FieldName, true);
            if (autoSuggestTemplates != null && autoSuggestTemplates.HasKeys())
            {
                var solrQueryValue = parameters.CurrentItem.GetFieldValue(BaseSolrNetSearchConfiguration.Fields.Query);
                if (string.IsNullOrWhiteSpace(solrQueryValue))
                {
                    return;
                }

                var keywordQuery = new SolrQuery($"({SolrNetGetKeywordQuery(solrQueryValue, parameters.Keyword, parameters.CurrentItem.GetCheckboxStatus(BaseSolrNetSearchConfiguration.Fields.ExactSearch_FieldName), parameters.CurrentItem.GetCheckboxStatus(BaseSearchConfiguration.Fields.ShowResultsByDefault_FieldName))})");

                var groupingFieldValue = parameters.CurrentItem.GetFieldValue(BaseSolrNetSearchConfiguration.Fields.Grouping_FieldName);
                if (!string.IsNullOrWhiteSpace(groupingFieldValue))
                {
                    var groupingDictionary = WebUtil.ParseQueryString(groupingFieldValue);
                    parameters.QueryOptions = new QueryOptions
                    {
                        Grouping = new GroupingParameters
                        {
                            Fields = groupingDictionary.Keys.ToArray(),
                            Limit = int.TryParse(groupingDictionary.FirstOrDefault().Value, out var limit) ? limit : 4
                        }
                    };
                }

                var templateQuery = new SolrQueryInList(FieldNames.ItemTemplate,
                    autoSuggestTemplates.AllKeys.Select(x => StringExtensions.RemoveSpecialCharacters(x).ToLower()));

                var languageQuery = new SolrQueryByField(FieldNames.ItemLanguage, Context.Language.Name);
                var hideFromAutoSuggest = new SolrQueryByField(FieldNames.HideFromAutoSuggest, "false");
                var latestVersionQuery = new SolrQueryByField(FieldNames.ItemIsLatestVersion, "true");

                var completeQuery = keywordQuery
                                    && templateQuery
                                    && languageQuery
                                    && hideFromAutoSuggest
                                    && latestVersionQuery;

                parameters.Query = completeQuery;
            }
        }

        public static Dictionary<string, string> GetSuggestedSearches(Item currentItem)
        {
            var dictionary = new Dictionary<string, string>();
            if (currentItem != null && !string.IsNullOrWhiteSpace(currentItem.GetFieldValue(OneWebAutoSuggestSearchConfiguration.Fields.SuggestedKeywords_FieldName)))
            {
                var nameValueCollection = Sitecore.Web.WebUtil.ParseUrlParameters(
                    currentItem.GetFieldValue(OneWebAutoSuggestSearchConfiguration.Fields.SuggestedKeywords_FieldName));
                foreach (var key in nameValueCollection.AllKeys)
                {
                    dictionary.Add(key, nameValueCollection[key]);   
                }
            }

            return dictionary;
        }
    }
}