using Microsoft.Extensions.DependencyInjection;
using OneWeb.Foundation.Search.Repositories;
using OneWeb.Foundation.Search.Service;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Foundation.Search.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Foundation.Search*");
            serviceCollection.AddTransient<ISearchRepository, SearchRepository>();
            serviceCollection.AddSingleton<SearchService>();
        }
    }
}