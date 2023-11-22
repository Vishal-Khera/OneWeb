using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OneWeb.Feature.PageContent.Models;
using OneWeb.Feature.PageStructure.Repositories;
using OneWeb.Foundation.Search.Models.ContentSearch;
using OneWeb.Foundation.Search.Repositories;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.PageContent.Repositories
{
    public class ProductListRepository : VariantsRepository, IProductListRepository
    {
        protected IPageStructureRepository _pageStructureRepository { get; set; }
        public ProductListRepository(ISearchRepository searchRepository, IPageStructureRepository pageStructureRepository)
        {
            _searchRepository = searchRepository;
            _pageStructureRepository = pageStructureRepository;
        }
        protected ISearchRepository _searchRepository { get; }
        public ProductListRenderingModel GetModel(Item item)
        {
            var productListModel = new ProductListRenderingModel();
            try
            {
                productListModel.SlideWrapperSettings = _pageStructureRepository.GetSlideWrapper();
                var parameter = Rendering.Parameters[OneWebProductListRenderingParameters.Fields.RelatedProductList_FieldName];
                if (parameter.Equals(Constants.OneWebCategory))
                {
                    productListModel.ProductList = GetOverrideProductListCategory(item);
                }

                else
                {
                    if (StringExtensions.IdEqualsGuid(item.TemplateID, OneWebProductList.TemplateIdString) || StringExtensions.IdEqualsGuid(item.TemplateID, ProductDetail.TemplateIdString))
                    {
                        productListModel.ProductList = GetOverrideProductList(item);
                    }
                }

                FillBaseProperties(productListModel);
                
            }
            catch (Exception ex)
            {
                LogManager.LogError("Error parsing in GetModel for One Web Product List", ex);
            }
            return productListModel;
        }

        private List<ProductListModel> GetOverrideProductList(Item item)
        {

            var productList = new List<ProductListModel>();
            try
            {
                var productModel = item.GetMultiListItems(OneWebProductList.Fields.ProductList_FieldName);
                if (productModel.Any())
                {
                    foreach (var productListItem in productModel)
                    {
                        var productListModel = new ProductListModel()
                        {
                            Title = productListItem.GetFieldValue(BaseContent.Fields.Title),
                            Image = new MediaModel(productListItem, BaseImage.Fields.Image_FieldName),
                            Link = string.IsNullOrWhiteSpace(
                            productListItem.GetFieldValue(BaseNavigation.Fields.NavigationUrl_FieldName))
                                ? new LinkModel()
                                {
                                    Url = productListItem.GetItemUrl(),
                                }
                                : new LinkModel(productListItem, BaseNavigation.Fields.NavigationUrl_FieldName),
                        };
                        productList.Add(productListModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError("Error parsing in override functionality for One Web Product List", ex);
            }

            return productList;
        }


        private List<ProductListModel> GetOverrideProductListCategory(Item item)
        {

            var productList = new List<ProductListModel>();
            try
            {
                var currentProduct = Sitecore.Context.Item.Parent;
                Sitecore.Collections.ChildList eventItems;
                eventItems = currentProduct.GetChildren();

                var PagesToSkip = item.Fields[BaseSearchRenderingParameters.Fields.PagesToSkip_FieldName].Value != "" ? Convert.ToInt32(item.Fields[BaseSearchRenderingParameters.Fields.PagesToSkip_FieldName].Value) : 0;
                var PageSize = item.Fields[BaseSearchRenderingParameters.Fields.PageSize_FieldName].Value != "" ? Convert.ToInt32(item.Fields[BaseSearchRenderingParameters.Fields.PageSize_FieldName].Value) : 0;
               
                foreach (Item productListItem in eventItems.Skip(PagesToSkip* PageSize).Take(PageSize).Where(x => x.ID != item.ID))
                {
                    var productListModel = new ProductListModel()
                    {
                        Title = productListItem.GetFieldValue(BaseContent.Fields.Title),
                        Image = new MediaModel(productListItem, BaseImage.Fields.Image_FieldName),
                        Link = string.IsNullOrWhiteSpace(
                            productListItem.GetFieldValue(BaseNavigation.Fields.NavigationUrl_FieldName))
                                ? new LinkModel()
                                {
                                    Url = productListItem.GetItemUrl(),
                                }
                                : new LinkModel(productListItem, BaseNavigation.Fields.NavigationUrl_FieldName),
                    };
                    productList.Add(productListModel);
                }
            
            }
            catch (Exception ex)
            {
                LogManager.LogError("Error parsing in rendering parameter Category for One Web Product List", ex);
            }

            return productList;
        }
    }
}