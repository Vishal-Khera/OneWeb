using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using OneWeb.Feature.Search.Models;
using OneWeb.Foundation.Search.Extensions;
using OneWeb.Foundation.Search.Models.AutoSuggest;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Models.Facets;
using OneWeb.Foundation.Search.Models.SolrNetSearch;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using NameValueCollectionExtensions = Sitecore.XA.Foundation.SitecoreExtensions.Extensions.NameValueCollectionExtensions;

namespace OneWeb.Feature.Search.Repositories
{
    public class SearchRepository :
        VariantsRepository,
     ISearchRepository
    {
        public IRenderingModelBase GetModel()
        {
            throw new System.NotImplementedException();
        }
        public SearchBoxRenderingModel RenderSearchBox(Item datasourceItem)
        {
            var searchBoxModel = new SearchBoxRenderingModel();
            this.FillBaseProperties(searchBoxModel);

            var configurationItem = datasourceItem.GetReferencedItem(OneWebSearchBox.Fields.Configuration_FieldName);
            var autoSuggestConfigurationItem = datasourceItem.GetReferencedItem(OneWebSearchBox.Fields.AutoSuggestConfiguration_FieldName);
            searchBoxModel.ConfigurationId = configurationItem.ShortId();
            searchBoxModel.AutoSuggestConfigurationId = datasourceItem.GetReferencedItem(OneWebSearchBox.Fields.AutoSuggestConfiguration_FieldName)?.ShortId();
            searchBoxModel.AutoSuggestTriggerCount =
                int.TryParse(datasourceItem.GetFieldValue(OneWebSearchBox.Fields.AutoSuggestTriggerCount_FieldName),
                    out var triggerCount)
                    ? triggerCount
                    : 3;
            searchBoxModel.EmptyTextMessage = datasourceItem.GetFieldValue(OneWebSearchBox.Fields.EmptyTextMessage);
            searchBoxModel.ShowResultsByDefault = configurationItem.GetCheckboxStatus(BaseSearchConfiguration.Fields.ShowResultsByDefault_FieldName);
            var popularKeywords = HttpUtility.ParseQueryString(
                autoSuggestConfigurationItem.GetFieldValue(OneWebAutoSuggestSearchConfiguration.Fields
                    .PopularKeywords));
            if (popularKeywords.HasKeys())
            {
                searchBoxModel.PopularKeywords = popularKeywords.AllKeys.Select(x => new Keyword()
                {
                    Text = x,
                    Link = popularKeywords[x]
                }).ToList();
            }
            
            if (!string.IsNullOrWhiteSpace(
                    datasourceItem.GetFieldValue(OneWebSearchBox.Fields.SearchResultPage_FieldName)))
            {
                searchBoxModel.SearchResultPageUrl = datasourceItem
                    .GetReferencedItem(OneWebSearchBox.Fields.SearchResultPage_FieldName).GetItemUrl();
            }

            return searchBoxModel;
        }
        public HandleBarRenderingModel RenderSearchTemplate(Item datasourceItem)
        {
            var model = new HandleBarRenderingModel();
            base.FillBaseProperties(model);
            if (datasourceItem == null)
            {
                return model;
            }

            var configurationId = RenderingContext.Current.Rendering?.Parameters[
                OneWebSearchTemplateRenderingParameters.Fields.Configuration_FieldName];

            if (string.IsNullOrWhiteSpace(configurationId))
            {
                return model;
            }
            var configId = CoreExtensions.TriggerResolveTokenPipeline(configurationId, datasourceItem);
            model.ConfigurationId = StringExtensions.RemoveSpecialCharacters(configId).ToUpper();

            model.SourceId = datasourceItem.ShortId();
            if (StringExtensions.IdEqualsGuid(datasourceItem.TemplateID, OneWebSearchFilter.TemplateIdString) || StringExtensions.IdEqualsGuid(datasourceItem.TemplateID, OneWebAdvancedSearchFilter.TemplateIdString))
            {
                if (!datasourceItem.GetCheckboxStatus(OneWebSearchFilter.Fields.Enabled))
                {
                    return model;
                }
                model.TemplateType = "is-filter ";

                if (!datasourceItem.GetCheckboxStatus(OneWebSearchFilter.Fields.IsDynamicFilter))
                {
                    model.Endpoint = "/oneweb/search/getfilter";
                    model.TemplateType += "is-static-filter ";
                }

                var filterType = datasourceItem.GetFieldValue(OneWebSearchFilter.Fields.FilterType);
                if (!string.IsNullOrWhiteSpace(filterType))
                {
                    switch (filterType.ToLower())
                    {
                        case "alphabet":
                            model.TemplateType += "is-alphabet-filter ";
                            break;
                        case "link":
                            model.TemplateType += "is-link-filter ";
                            break;
                        default:
                            model.TemplateType += "is-checkbox-filter ";
                            break;
                    }
                }

                if (StringExtensions.IdEqualsGuid(datasourceItem.TemplateID, OneWebAdvancedSearchFilter.TemplateIdString))
                {
                    if (!string.IsNullOrWhiteSpace(datasourceItem.GetFieldValue(OneWebAdvancedSearchFilter.Fields.MappingApi_FieldName)))
                    {
                        model.Parameters["ow-mapping-api"] = datasourceItem.GetFieldValue(OneWebAdvancedSearchFilter.Fields.MappingApi_FieldName);
                        model.Parameters["ow-primary-facet"] = datasourceItem.GetFieldValue(OneWebAdvancedSearchFilter.Fields.PrimaryFacet_FieldName);
                        model.Parameters["ow-secondary-facet"] = datasourceItem.GetFieldValue(OneWebAdvancedSearchFilter.Fields.SecondaryFacet_FieldName);
                        model.TemplateType += "is-advanced-filter ";
                    }
                }
            }
            else if (StringExtensions.IdEqualsGuid(datasourceItem.TemplateID, OneWebContentSearchConfiguration.TemplateIdString)
                     || StringExtensions.IdEqualsGuid(datasourceItem.TemplateID, OneWebSolrNetSearchConfiguration.TemplateIdString))
            {
                model.Endpoint = "/oneweb/search/getsearchresults";
                model.TemplateType = "is-result";
                model.Parameters["PageSize"] =
                    datasourceItem.GetInteger(BaseSearchRenderingParameters.Fields.PageSize);
                model.Parameters["PagesToSkip"] =
                    datasourceItem.GetInteger(BaseSearchRenderingParameters.Fields.PagesToSkip);
            }
            
            return model;
        }
        public JsonResult GetFilter(Item datasourceItem)
        {
            var model = new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            if (datasourceItem == null)
            {
                return model;
            }

            var filterModel = new OneWebFacet(datasourceItem)
            {
                Values = datasourceItem.Children.Select(x => new OneWebFacetValue()
                {
                    Name = x.GetFieldValue(OneWebSearchFilterItem.Fields.Name_FieldName),
                    Value = x.GetFieldValue(OneWebSearchFilterItem.Fields.Value_FieldName)
                }).ToList()
            };

            filterModel.IsDynamicFilter = datasourceItem.GetCheckboxStatus(OneWebSearchFilter.Fields.IsDynamicFilter);
            return new JsonResult() { Data = filterModel, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetSearchResults(Foundation.Search.Repositories.ISearchRepository searchRepository, AjaxSearchParameters parameters)
        {
            ContentModel xyz = new ContentModel();
            var pageSize = 0;
            var pagesToSkip = 0;

            if (!string.IsNullOrWhiteSpace(parameters.PagesToSkip))
            {
                pagesToSkip = int.Parse(parameters.PagesToSkip);
            }

            if (!string.IsNullOrWhiteSpace(parameters.PageSize))
            {
                pageSize = int.Parse(parameters.PageSize);
            }

            if (StringExtensions.IdEqualsGuid(parameters.SourceItem.TemplateID, OneWebContentSearchConfiguration.TemplateIdString))
            {
                var contentSearchParameters = new ContentParameters<ContentModel>
                {
                    CurrentItem = parameters.SourceItem,
                    Keyword = parameters.Keyword,
                    PageSize = pageSize,
                    PagesToSkip = pagesToSkip,
                    IndexName = parameters.SourceItem.GetFieldValue(BaseSearchConfiguration.Fields.IndexName_FieldName),
                    Language = !string.IsNullOrWhiteSpace(parameters.Language) ? Sitecore.Globalization.Language.Parse(parameters.Language) : null,
                };

                SearchExtensions.ParseContextIndex(contentSearchParameters);

                if (parameters.Filters.Any())
                {
                    var aggregatedFilterQuery = PredicateBuilder.True<ContentModel>();
                    foreach (var filter in parameters.Filters)
                    {
                        var filterQuery = PredicateBuilder.False<ContentModel>();
                        foreach (var filterValue in filter.Values)
                        {
                            filterQuery = filterQuery.Or(x => x[SearchExtensions.ParseFieldName(contentSearchParameters.Index, filter.Key)] == filterValue);
                        }

                        aggregatedFilterQuery = aggregatedFilterQuery.And(filterQuery);
                    }

                    contentSearchParameters.SearchPredicate = aggregatedFilterQuery;
                }

                if (searchRepository.ContentSearch(contentSearchParameters) is ContentResponse<ContentModel> searchResults)
                {
                    return new JsonResult() { Data = searchResults, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            else if (StringExtensions.IdEqualsGuid(parameters.SourceItem.TemplateID, OneWebSolrNetSearchConfiguration.TemplateIdString))
            {
                var solrSearchParameters = new SolrNetParameters<SolrNetModel>()
                {
                    CurrentItem = parameters.SourceItem,
                    Keyword = parameters.Keyword,
                    PageSize = pageSize,
                    PagesToSkip = pagesToSkip,
                    IndexName = parameters.SourceItem.GetFieldValue(BaseSearchConfiguration.Fields.IndexName_FieldName),
                    Language = !string.IsNullOrWhiteSpace(parameters.Language) ? Sitecore.Globalization.Language.Parse(parameters.Language) : null,
                };
                SearchExtensions.ParseContextIndex(solrSearchParameters);

                if (parameters.Filters.Any())
                {
                    solrSearchParameters.PresetQuery = string.Join(" AND ", parameters.Filters.Select(
                            filter => $"({SearchExtensions.ParseFieldName(solrSearchParameters.Index, filter.Key)}:({string.Join(" OR ", filter.Values.Select(x => $"\"{x}\""))}))"));
                }
                if (searchRepository.SolrNetSearch(solrSearchParameters) is SolrNetResponse<SolrNetModel> searchResults)
                {
                    return new JsonResult() { Data = searchResults, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetAutoSuggestResults(Foundation.Search.Repositories.ISearchRepository searchRepository, AjaxSearchParameters parameters)
        {
            if (StringExtensions.IdEqualsGuid(parameters.SourceItem.TemplateID, OneWebAutoSuggestSearchConfiguration.TemplateIdString))
            {
                var solrSearchParameters = new AutoSuggestParameters<AutoSuggestModel>()
                {
                    CurrentItem = parameters.SourceItem,
                    Keyword = parameters.Keyword,
                    IndexName = parameters.SourceItem.GetFieldValue(BaseSearchConfiguration.Fields.IndexName_FieldName),
                    Language = !string.IsNullOrWhiteSpace(parameters.Language) ? Sitecore.Globalization.Language.Parse(parameters.Language) : null,
                };
                if (searchRepository.AutoSuggestSearch(solrSearchParameters) is AutoSuggestResponse<AutoSuggestModel> searchResults)
                {
                    return new JsonResult() { Data = searchResults, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult() { Data = string.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public PaginationRenderingModel RenderPagination(Item datasourceItem)
        {
            var paginationModel = new PaginationRenderingModel();
            this.FillBaseProperties(paginationModel);

            var configurationItem = datasourceItem.GetReferencedItem(OneWebPagination.Fields.Configuration_FieldName);
            paginationModel.ConfigurationId = configurationItem.ShortId();
            paginationModel.NextText = datasourceItem.GetFieldValue(OneWebPagination.Fields.NextText);
            paginationModel.PreviousText = datasourceItem.GetFieldValue(OneWebPagination.Fields.PreviousText);
            paginationModel.DisplayedPages = datasourceItem.GetInteger(OneWebPagination.Fields.DisplayedPages);
            paginationModel.VisibleEdges = datasourceItem.GetInteger(OneWebPagination.Fields.VisibleEdges);

            return paginationModel;
        }
    }
}
