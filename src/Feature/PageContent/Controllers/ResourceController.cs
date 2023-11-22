using OneWeb.Feature.PageContent.Repositories;
using OneWeb.Foundation.Search.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class ResourceController : StandardController
    {
        public ResourceController(IResourceRepository resourceRepository)
        {
            _ResourceRepository = resourceRepository;
        }
        protected IResourceRepository _ResourceRepository { get; }

       protected override object GetModel()
       {
           var datasourceItem = RenderingContext.Current.Rendering.Item ?? Sitecore.Context.Item;
           return _ResourceRepository.GetModel(datasourceItem);
       }      
    }
}