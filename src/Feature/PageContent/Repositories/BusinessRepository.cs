using OneWeb.Feature.PageContent.Models;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Repositories;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.PageContent.Repositories
{
    public class BusinessRepository : VariantsRepository, IBusinessRepository
    {
        public BusinessRepository(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        protected ISearchRepository _searchRepository { get; }

        public BusinessRenderingModel GetModel(BusinessRequest request)
        {
            var businessModel = new BusinessRenderingModel
            {
                BusinessResponse = (ContentResponse<BusinessModel>)GetData(request)
            };

            FillBaseProperties(businessModel);

            return businessModel;
        }

        public object GetData(BusinessRequest request)
        {
            var searchParameters = new ContentParameters<BusinessModel>
            {
                Keyword = request.Keyword,
                IndexName = Constants.OneWebBusinessIndex
            };

            if (!string.IsNullOrWhiteSpace(request.Scope))
            {
                var scopePredicate = PredicateBuilder.True<BusinessModel>();
                scopePredicate = scopePredicate.And(x => x.FullPath.StartsWith(request.Scope));

                searchParameters.SearchPredicate = scopePredicate;
            }

            return _searchRepository.ContentSearch(searchParameters);
        }
    }
}