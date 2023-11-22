using Microsoft.Extensions.DependencyInjection;
using OneWeb.Feature.PageStructure.Controllers;
using OneWeb.Feature.PageStructure.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Feature.PageStructure.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Feature.PageContent*");
            serviceCollection.AddTransient<IPageStructureRepository, PageStructureRepository>();
            serviceCollection.AddTransient<PageStructureController>();
        }
    }
}