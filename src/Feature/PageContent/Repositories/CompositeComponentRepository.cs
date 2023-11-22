using OneWeb.Feature.PageContent.Models;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.DynamicPlaceholders.Repositories;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using Sitecore.XA.Foundation.IoC;
using System.Collections.Generic;
using System.Linq;
using Sitecore.XA.Feature.Composites.Services;

namespace OneWeb.Feature.PageContent.Repositories
{
    public class CompositeComponentRepository :
    DynamicPlaceholdersRepository,
    ICompositeComponentRepository,
    IModelRepository,
    IControllerRepository,
    IAbstractRepository<IRenderingModelBase>
    {
        public ICompositeService CompositeService { get; }

        protected virtual bool HasLoopInDatasource => !string.IsNullOrEmpty(this.Rendering.Properties["cmps-loop"]);

        public CompositeComponentRepository()
          : this((ICompositeService)new Sitecore.XA.Feature.Composites.Services.CompositeService())
        {
        }

        public CompositeComponentRepository(ICompositeService compositeService) => this.CompositeService = compositeService;

        public override IRenderingModelBase GetModel()
        {
            CompositeComponentRenderingModel m = new CompositeComponentRenderingModel();
            this.FillBaseProperties((object)m);
            return (IRenderingModelBase)m;
        }

        protected override void FillBaseProperties(object m)
        {
            base.FillBaseProperties(m);
            CompositeComponentRenderingModel componentRenderingModel = (CompositeComponentRenderingModel)m;
            if (this.Rendering.DataSourceItem != null)
            {
                SortedDictionary<int, Item> compositeItems = this.CompositeItems;
                componentRenderingModel.CompositeItems = compositeItems;
                componentRenderingModel.CompositeCount = compositeItems.Count;
            }
            else
                componentRenderingModel.CompositeCount = 0;
            componentRenderingModel.HasCompositeLoop = this.HasLoopInDatasource;
        }

        protected override bool ShouldShowMessage()
        {
            if (!this.IsControlEditable)
                return false;
            return this.Rendering.DataSourceItem == null || this.CompositeItems.Count == 0;
        }

        protected virtual SortedDictionary<int, Item> CompositeItems
        {
            get
            {
                SortedDictionary<int, Item> compositeItems = new SortedDictionary<int, Item>();
                List<Item> list = this.CompositeService.GetCompositeItems(this.Rendering.DataSourceItem).ToList<Item>();
                for (int index = 0; index < list.Count; ++index)
                    compositeItems.Add(index + 1, list[index]);
                return compositeItems;
            }
        }
    }
}
