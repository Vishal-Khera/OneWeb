using OneWeb.Feature.PageContent.Models;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using System.Collections.Generic;

namespace OneWeb.Feature.PageContent.Repositories
{
    public interface IGalleryRepository : IModelRepository
    {
        List<GalleryModel> GetGalleryList();
    }
}