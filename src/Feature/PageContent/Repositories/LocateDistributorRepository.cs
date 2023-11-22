using OneWeb.Feature.PageContent.Models;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Repositories;
using OneWeb.Foundation.Serialization;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace OneWeb.Feature.PageContent.Repositories
{
    public class LocateDistributorRepository : VariantsRepository, ILocateDistributorRepository
    {
        protected ISearchRepository _searchRepository { get; }
        public LocateDistributorRepository(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        public LocateDistributorListModel GetModel(BusinessRequest businessRequest)
        {
            var locateDistributorModel = new LocateDistributorListModel();
            if (businessRequest == null || string.IsNullOrWhiteSpace(businessRequest.Keyword))
            {
                return locateDistributorModel;
            }

            locateDistributorModel.Events = GetDistributor(businessRequest);
            return locateDistributorModel;
        }
        private List<LocateDistributorModel> GetDistributor(BusinessRequest businessRequest)
        {
            if (businessRequest == null || string.IsNullOrWhiteSpace(businessRequest.Keyword))
            {
                return new List<LocateDistributorModel>();
            }
            var contentSearchRequest = new ContentParameters<LocateDistributorModel>
            {
            };
            contentSearchRequest.SearchCondition.TemplateIds.Add(OneWebLocateDistributor.TemplateId);
            if (int.TryParse(businessRequest.Keyword, out int zipCode))
            {
                var filterPredicate = PredicateBuilder.True<LocateDistributorModel>();
                filterPredicate = filterPredicate.And(x => x.ZipCodeMaxRange >= zipCode && x.ZipCodeMinRange <= zipCode);
                contentSearchRequest.SearchPredicate = filterPredicate;
            }
            else
            {
                var filterPredicate = PredicateBuilder.True<LocateDistributorModel>();
                filterPredicate = filterPredicate.And(x => x.Country.Contains(businessRequest.Keyword) || x.City.Contains(businessRequest.Keyword));
                contentSearchRequest.SearchPredicate = filterPredicate;
            }
            if (!(_searchRepository.ContentSearch(contentSearchRequest) is ContentResponse<LocateDistributorModel> searchResults))
            {
                return new List<LocateDistributorModel>();
            }
            if (searchResults == null || searchResults.Results.Count == 0)
                return new List<LocateDistributorModel>();
            return searchResults.Results.ToList();
        }
    }
}