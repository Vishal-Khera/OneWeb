using Microsoft.Extensions.DependencyInjection;
using OneWeb.Feature.Navigation.Controllers;
using OneWeb.Feature.Navigation.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Feature.Navigation.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Feature.Navigation*");
           serviceCollection.AddTransient<ILanguageSwitcherRepository, LanguageSwitcherRepository>();
            serviceCollection.AddTransient<LanguageSwitcherController>();
        }
    }
}