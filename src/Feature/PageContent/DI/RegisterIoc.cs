using Microsoft.Extensions.DependencyInjection;
using OneWeb.Feature.PageContent.Controllers;
using OneWeb.Feature.PageContent.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Feature.PageContent.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Feature.PageContent*");
            serviceCollection.AddTransient<IPageContentRepository, PageContentRepository>();
            serviceCollection.AddTransient<PageContentController>();
        }
    }
}