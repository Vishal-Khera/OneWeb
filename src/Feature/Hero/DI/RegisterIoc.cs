using Microsoft.Extensions.DependencyInjection;
using OneWeb.Feature.HeroBanner.Controllers;
using OneWeb.Feature.HeroBanner.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Feature.HeroBanner.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Feature.HeroBanner*");
            serviceCollection.AddTransient<IHeroBannerRepository, HeroBannerRepository>();
            serviceCollection.AddTransient<HeroBannerController>();
        }
    }
}