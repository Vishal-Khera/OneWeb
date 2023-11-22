using OneWeb.Feature.PageContent.Repositories;
using OneWeb.Foundation.Search.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class PressReleaseController : StandardController
    {
        public PressReleaseController(IPressReleaseRepository pressReleaseRepository)
        {
            PressReleaseRepository = pressReleaseRepository;
        }
        protected IPressReleaseRepository PressReleaseRepository { get; }

       protected override object GetModel()
       {
           var datasourceItem = RenderingContext.Current.Rendering.Item ?? Sitecore.Context.Item;
           return PressReleaseRepository.GetModel(datasourceItem);
       }      
    }
}