using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OneWeb.Feature.Navigation.Models;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;

namespace OneWeb.Feature.Navigation.Repositories
{
    public class NavigationRepository :
        VariantsRepository,
        INavigationRepository
    {
        private int? _levelFrom;
        private int? _levelTo;

        private Item _startItem;


        public NavigationRepository(
            IVariantsRepository variantsRespository)
        {
            VariantsRespository = variantsRespository;
        }

        protected IVariantsRepository VariantsRespository { get; set; }

        protected int[] Levels => new int[2]
        {
            ParseLevel(OneWebNavigationRenderingParameters.Fields.LevelTo_FieldName),
            ParseLevel(OneWebNavigationRenderingParameters.Fields.LevelFrom_FieldName)
        };


        protected virtual int LevelTo
        {
            get
            {
                if (!_levelTo.HasValue) _levelTo = Levels.Max();

                if (PageMode.IsExperienceEditor || PageMode.IsExperienceEditorEditing) return LevelFrom;
                return _levelTo.Value;
            }
        }

        protected virtual int LevelFrom
        {
            get
            {
                if (!_levelFrom.HasValue) _levelFrom = Levels.Min();

                return _levelFrom.Value;
            }
        }

        protected Item StartItem
        {
            get => _startItem ?? (_startItem = GetStartItem());
            set => _startItem = value;
        }

        public NavigationRenderingModel GetModel()
        {
            var navigationModel = new NavigationRenderingModel
            {
                Name = StartItem.Name,
                Children = GetChildItems(StartItem, LevelFrom).Select(x => GetNavigationItems(x, LevelFrom)).ToList(),
                IsFake = false,
                Levels = LevelTo,
                Current = StartItem.Fields[Templates.Navigable.Fields.NavigationTitle].Value
            };
            FillBaseProperties(navigationModel);

            return navigationModel;
        }

        public NavigationModel GetNavigationItems(Item item, int level)
        {
            var navigationModel = new NavigationModel();
            if (IsNavigationItem(item, level) && IsNavigable(item))
            {
                var childItems = GetChildItems(item, level + 1).Select(x => GetNavigationItems(x, level + 1)).ToList();
                navigationModel.Title = item.GetFieldValue(Templates.Navigable.Fields.NavigationTitle);
                navigationModel.Description = item.GetFieldValue(BaseNavigation.Fields.NavigationDescription);
                navigationModel.Image = item.GetMediaModel(BaseNavigation.Fields.NavigationImage);
                navigationModel.HasChildren = childItems.Any();
                navigationModel.IsActive = IsActive(item);
                navigationModel.Children = childItems;
                navigationModel.HasSubChildren = childItems.Any(x => x.HasChildren);
                AppendLinkAndCss(item, navigationModel);
            }

            return navigationModel;
        }

        protected void AppendLinkAndCss(Item item, NavigationModel incomingModel)
        {

            var navigationModel =
                item.GetLinkFieldModel(BaseNavigation.Fields.NavigationUrl);
            if (string.IsNullOrWhiteSpace(navigationModel?.Url))
            {
                if (StringExtensions.IdEqualsGuid(item.TemplateID, Redirect.TemplateIdString))
                {
                    navigationModel = item.GetLinkFieldModel(Templates.Redirect.Fields.RedirectUrl);
                    if (string.IsNullOrWhiteSpace(navigationModel?.Url))
                        navigationModel = new LinkModel
                        {
                            Url = "javascript:void(0)"
                        };
                }
                else
                {
                    navigationModel = new LinkModel
                    {
                        Url = item.GetItemUrl()
                    };
                }
            }

            incomingModel.Link = navigationModel;
            incomingModel.CssClass = navigationModel.Class;
        }

        protected virtual List<Item> GetChildItems(Item startItem, int level)
        {
            return startItem.Children.Where(c => c.Versions.Count > 0 && IsNavigationItem(c, level)).ToList();
        }

        protected virtual Item GetStartItem()
        {
            var rootItem = ContentRepository.GetItem(PageContext.StartPath);
            var parameter = Rendering.Parameters[OneWebNavigationRenderingParameters.Fields.NavigationRoot_FieldName];
            if (!string.IsNullOrEmpty(parameter)) rootItem = ContentRepository.GetItem(ID.Parse(parameter));

            return rootItem;
        }

        protected virtual bool IsFilteredOut(Item item)
        {
            var parameter = Rendering.Parameters[OneWebNavigationRenderingParameters.Fields.Filter_FieldName];
            var str = item[Templates.Navigable.Fields.NavigationFilter];
            return !string.IsNullOrEmpty(parameter) && str.Contains(parameter);
        }

        protected virtual int ParseLevel(string fieldName)
        {
            var enumValue = Rendering.Parameters.GetEnumValue(fieldName);
            return !string.IsNullOrWhiteSpace(enumValue) ? int.Parse(enumValue, CultureInfo.InvariantCulture) : 1;
        }

        protected virtual bool IsNavigationItem(Item item, int level)
        {
            return ((IsFilteredOut(item) ? 1 : 0) == 0) & IsFitsRange(level) & IsNavigable(item);
        }

        protected virtual bool IsNavigable(Item item)
        {
            return ContentRepository.GetBaseTemplates(item.TemplateID, item.Database)
                .Contains(Templates.Navigable.TemplateId);
        }

        protected virtual bool IsFitsRange(int level)
        {
            return level <= LevelTo;
        }
        protected virtual bool IsActive(Item item)
        {
            var contextItem = Sitecore.Context.Item;
            return contextItem !=null && (item.Paths.FullPath.Equals(contextItem.Paths.FullPath) || item.Paths.IsAncestorOf(Sitecore.Context.Item));
        }
    }
}