using OneWeb.Feature.PageContent.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using Sitecore.XA.Foundation.IoC;

namespace OneWeb.Feature.PageContent.Repositories
{
    public interface ICompositeComponentRepository : IModelRepository,IControllerRepository,IAbstractRepository<IRenderingModelBase>
    {       
    }
}