using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.MarkupDecorator.Pipelines.DecorateRendering;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA
{
    public class GetRenderingCssClasses : RenderingDecorator
    {
        public override void Process(RenderingDecoratorArgs args)
        {
            var stringList = LookupFieldValues(args.Rendering.Parameters["Styles"], "Value");
            var owCustomClassList = LookupFieldValues(args.Rendering.Parameters["OwClass"]);
            if (!stringList.Any() && !owCustomClassList.Any())
                return;

            var aggregateList = new List<string>();
            aggregateList.AddRange(stringList);
            aggregateList.AddRange(owCustomClassList);

            args.AddAttribute("class", aggregateList);
        }

        protected virtual IList<string> LookupFieldValues(string itemIds, string fieldName)
        {
            return LookupItems(itemIds).Select(i => i[fieldName]).ToList();
        }

        protected virtual IList<string> LookupFieldValues(string classNames)
        {
            return classNames.Split(',', '|');
        }

        protected virtual IList<Item> LookupItems(string itemIds)
        {
            var sitecoreRepository = ServiceLocator.ServiceProvider.GetService<IContentRepository>();
            return (itemIds ?? string.Empty).Split(',', '|').Where(ID.IsID).Select(ID.Parse)
                .Select(id => sitecoreRepository.GetItem(id)).Where(i => i != null).ToList();
        }
    }
}