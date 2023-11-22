using System.Collections.Specialized;
using System.Globalization;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using Sitecore.XA.Foundation.Mvc.Models;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA
{
    public class FieldRendererBase
    {
        protected RenderingWebEditingParams RenderingWebEditingParams;

        protected FieldRenderer CreateFieldRenderer(
            Item item,
            string fieldName,
            NameValueCollection parameters = null)
        {
            var flag = !RenderingWebEditingParams.DisableWebEditing;
            var skipCommonButtons = RenderingWebEditingParams.SkipCommonButtons;
            if (parameters == null)
                parameters = new NameValueCollection();
            if (!Context.PageMode.IsNormal && skipCommonButtons & flag)
                parameters.Add("skipCommonButtons", true.ToString(CultureInfo.InvariantCulture));
            var fieldRenderer = new FieldRenderer();
            fieldRenderer.Item = item;
            fieldRenderer.FieldName = fieldName;
            fieldRenderer.Parameters = parameters.ToQueryString();
            fieldRenderer.DisableWebEditing = !flag;
            return fieldRenderer;
        }
    }
}