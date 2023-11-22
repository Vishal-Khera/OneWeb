using OneWeb.Feature.PageContent.Models;
using OneWeb.Feature.PageContent.Repositories;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class LocateDistributorController : StandardController
    {
        // GET: LocateDistributor
        protected ILocateDistributorRepository _locateDistributorRepository { get; }
        public LocateDistributorController(ILocateDistributorRepository locateDistributorsRepository)
        {
            _locateDistributorRepository = locateDistributorsRepository;
        }
        public JsonResult GetLocateDistributor(BusinessRequest parameters)
        {
            return Json(_locateDistributorRepository.GetModel(parameters));
        }
    }
}