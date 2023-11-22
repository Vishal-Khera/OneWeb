using OneWeb.Feature.PageContent.Models;
using OneWeb.Foundation.Serialization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.DynamicPlaceholders.Repositories;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using Sitecore.XA.Foundation.IoC;
using System;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions; 
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace OneWeb.Feature.PageContent.Repositories
{
    public class AccordionRepository : CompositeComponentRepository,IAccordionRepository,IModelRepository,IControllerRepository,IAbstractRepository<IRenderingModelBase>
    {       

        private AccordionSettings _settings;

        protected AccordionSettings Settings => this._settings ?? (this._settings = this.GetAccordionSettings());      

        protected virtual string GetJsonDataProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (this.Settings != null)
            {
                dictionary.Add("expandOnHover", (object)this.Settings.ExpandOnHover);
                dictionary.Add("expandedByDefault", (object)this.Settings.ExpandedByDefault);
                dictionary.Add("speed", (object)this.Settings.Speed);
                dictionary.Add("easing", (object)this.Settings.Easing);
                dictionary.Add("canOpenMultiple", (object)this.Settings.CanOpenMultiple);
                dictionary.Add("canToggle", (object)this.Settings.CanToggle);
                dictionary.Add("isControlEditable", (object)this.IsControlEditable);
            }
            return new JavaScriptSerializer().Serialize((object)dictionary);
        }

        protected AccordionSettings GetAccordionSettings()
        {
            Dictionary<string, string> dictionary = this.Rendering.Parameters.ToDictionary<KeyValuePair<string, string>, string, string>((Func<KeyValuePair<string, string>, string>)(i => i.Key), (Func<KeyValuePair<string, string>, string>)(i => i.Value));
            string str1 = dictionary.GetValue<string>("EasingFunction");
            string str2 = string.IsNullOrEmpty(str1) || str1 == "Swing" ? "swing" : "easeInOut" + str1;
            return new AccordionSettings()
            {
                Easing = str2,
                Speed = dictionary.GetIntValue("Speed"),
                CanOpenMultiple = dictionary.GetBooleanValue("CanOpenMultiple"),
                CanToggle = dictionary.GetBooleanValue("CanToggle"),
                ExpandOnHover = dictionary.GetBooleanValue("ExpandOnHover"),
                ExpandedByDefault = dictionary.GetBooleanValue("ExpandedByDefault")
            };
        }

        public override IRenderingModelBase GetModel()
        {
            AccordionRenderingModel m = new AccordionRenderingModel();
            this.FillBaseProperties((object)m);
            m.JsonDataProperties = this.GetJsonDataProperties();
            return (IRenderingModelBase)m;
        }

    }
}
