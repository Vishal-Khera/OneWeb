using Microsoft.Extensions.DependencyInjection;
using OneWeb.Foundation.Login.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Foundation.Login.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Foundation.Login*");
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IApiService, ApiService>();
        }
    }
}