using System.Web.Mvc;
using OneWeb.Feature.PageContent.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class AccordionController : StandardController
    {
        public AccordionController(IAccordionRepository accordionRepository)
        {
            AccordionRepository = accordionRepository;
        }

        protected IAccordionRepository AccordionRepository { get; }

        protected override object GetModel()
        {
            return AccordionRepository.GetModel();
        }  
    }
}