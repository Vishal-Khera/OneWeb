using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace OneWeb.Foundation.Search.Models.ContentSearch
{
    public class ContentParameters<TBaseSearchModel> : IBaseParameters
    {
        public ContentParameters()
        {
            PageSize = int.MaxValue;
            PagesToSkip = 0;
            SearchPredicate = PredicateBuilder.True<TBaseSearchModel>();
            SearchCondition = new SearchCondition();
            Scope = new List<ID>();
        }
        public SearchCondition SearchCondition { get; set; }
        public Expression<Func<TBaseSearchModel, bool>> SearchPredicate { get; set; }
        public List<Tuple<Expression<Func<TBaseSearchModel, object>>, bool>> OrderBy { get; set; }
        public List<Tuple<Expression<Func<TBaseSearchModel, object>>, int>> FacetOn { get; set; }
        public bool OrderByDescending { get; set; }
        public Item CurrentItem { get; set; }
        public string OverrideQuery { get; set; }
        public string IndexName { get; set; }
        public ISearchIndex Index { get; set; }
        public string Keyword { get; set; }
        public int PageSize { get; set; }
        public int PagesToSkip { get; set; }
        public List<ID> Scope { get; set; }
        public Language Language { get; set; }
    }

    public class SearchCondition
    {
        public SearchCondition()
        {
            TemplateIds = new List<ID>();
            ItemIds = new List<ID>();
            TemplateNames = new List<string>();
            ItemNames = new List<string>();
            FieldValueConditions = new List<FieldCondition>();
        }

        public List<ID> TemplateIds { get; set; }
        public List<ID> ItemIds { get; set; }
        public List<string> TemplateNames { get; set; }
        public List<string> ItemNames { get; set; }
        public List<FieldCondition> FieldValueConditions { get; set; }
    }

    public class FieldCondition
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public FieldConditionComparer Condition { get; set; }
        public float Boost { get; set; }
    }

    public enum FieldConditionComparer
    {
        Equals,
        Contains
    }
}