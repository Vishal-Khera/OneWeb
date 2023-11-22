using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneWeb.Feature.PageContent.Models;
using OneWeb.Feature.PageContent.Repositories;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class SalesTabController : StandardController
    {

        protected ISalesTabRepository _salesTabRepository { get; }
        public SalesTabController(ISalesTabRepository salesTabRepository)
        {
            _salesTabRepository = salesTabRepository;
        }
        protected override object GetModel()
        {
            return _salesTabRepository.GetModel();
        }
    }
}