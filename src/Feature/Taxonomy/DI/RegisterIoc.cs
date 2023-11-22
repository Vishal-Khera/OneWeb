using Microsoft.Extensions.DependencyInjection;
using OneWeb.Feature.Taxonomy.Controllers;
using OneWeb.Feature.Taxonomy.Repositories;
using OneWeb.Foundation.Serialization;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Feature.Taxonomy.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Feature.Taxonomy*");
            serviceCollection.AddTransient<ITaxonomyRepository, TaxonomyRepository>();
            serviceCollection.AddTransient<TaxonomyController>();
        }
    }
}