using Microsoft.Extensions.DependencyInjection;
using OneWeb.Feature.Login.Controllers;
using OneWeb.Feature.Login.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Feature.Login.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Feature.Login*");
            serviceCollection.AddTransient<IVirtualUserRepository, VirtualUserRepository>();
            serviceCollection.AddTransient<IUserSearchBox, UserSearchBox>();
            serviceCollection.AddTransient<UserController>();
        }
    }
}