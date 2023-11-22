using OneWeb.Feature.PageContent.Models;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneWeb.Feature.PageContent.Repositories
{
    public interface ISalesTabRepository : IVariantsRepository
    {
        SalesTabListModel GetModel();
    }
}