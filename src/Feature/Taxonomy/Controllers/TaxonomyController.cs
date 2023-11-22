using OneWeb.Feature.Taxonomy.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.Taxonomy.Controllers
{
    public class TaxonomyController : StandardController
    {
        public TaxonomyController(ITaxonomyRepository taxonomyRepository)
        {
            _taxonomyRepository = taxonomyRepository;
        }

        protected ITaxonomyRepository _taxonomyRepository { get; }

        protected override object GetModel()
        {
            return _taxonomyRepository.GetModel();
        }
    }
}