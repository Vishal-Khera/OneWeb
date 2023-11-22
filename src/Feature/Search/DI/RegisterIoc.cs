using Microsoft.Extensions.DependencyInjection;
using OneWeb.Feature.Search.Controllers;
using OneWeb.Feature.Search.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Feature.Search.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Feature.Search*");
            serviceCollection.AddTransient<ISearchRepository, SearchRepository>();
            serviceCollection.AddTransient<SearchController>();
        }
    }
}