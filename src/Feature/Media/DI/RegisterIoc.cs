using Microsoft.Extensions.DependencyInjection;
using OneWeb.Feature.Media.Controllers;
using OneWeb.Feature.Media.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.IoC.ServiceCollection;

namespace OneWeb.Feature.Media.DI
{
    public class RegisterIoc : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMvcControllers("OneWeb.Feature.Media*");
            serviceCollection.AddTransient<IVideoRepository, VideoRepository>();
            serviceCollection.AddTransient<VideoController>();
        }
    }
}