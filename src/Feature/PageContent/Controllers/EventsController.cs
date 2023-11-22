using OneWeb.Feature.PageContent.Repositories;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace OneWeb.Feature.PageContent.Controllers
{
    public class EventsController : StandardController
    {
        protected IEventsRepository EventsRepository { get; }
        public EventsController(IEventsRepository eventsRepository)
        {
            EventsRepository = eventsRepository;
        }
        protected override object GetModel()
        {
            var datasource = RenderingContext.Current.Rendering.Item;
            return EventsRepository.GetModel(datasource);
        }
    }
}