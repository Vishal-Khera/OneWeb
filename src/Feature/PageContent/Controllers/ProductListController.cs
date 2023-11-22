using OneWeb.Feature.PageContent.Repositories;
using OneWeb.Foundation.Search.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class ProductListController : StandardController
    {       

        public ProductListController(IProductListRepository productListRepository )
        {
            ProductListRepository = productListRepository;
        }
        protected IProductListRepository ProductListRepository { get; }

        protected override object GetModel()
        {
            var datasourceItem = RenderingContext.Current.Rendering.Item ?? Sitecore.Context.Item;
            return ProductListRepository.GetModel(datasourceItem);
        }
    }
}