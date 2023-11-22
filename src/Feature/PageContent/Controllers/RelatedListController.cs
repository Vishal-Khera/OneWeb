using OneWeb.Feature.PageContent.Repositories;
using OneWeb.Foundation.Search.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class RelatedListController : StandardController
    {
        public RelatedListController(IRelatedListRepository relatedListRepository)
        {
            RelatedListRepository = relatedListRepository;
        }
        protected IRelatedListRepository RelatedListRepository { get; }

        protected override object GetModel()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item ?? Sitecore.Context.Item;
            return RelatedListRepository.GetModel(datasourceItem);
        }
    }
}