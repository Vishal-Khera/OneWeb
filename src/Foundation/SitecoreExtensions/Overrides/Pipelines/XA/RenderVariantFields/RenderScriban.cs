using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using OneWeb.Foundation.SitecoreExtensions.Models.Pipelines.XA;
using OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GenerateScribanContext;
using OneWeb.Foundation.SitecoreExtensions.Services;
using Scriban;
using Scriban.Runtime;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using Sitecore.Rules;
using Sitecore.XA.Foundation.RenderingVariants.Pipelines.RenderVariantField;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.RenderVariantField;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.RenderVariantFields
{
    public class RenderScriban : RenderRenderingVariantFieldProcessor
    {
        protected readonly IScribanTemplateRenderer ScribanTemplateRenderer;

        public RenderScriban(IScribanTemplateRenderer renderer)
        {
            ScribanTemplateRenderer = renderer;
        }

        public override Type SupportedType => typeof(VariantScriban);

        public override RendererMode RendererMode => RendererMode.Html;

        public override void RenderField(RenderVariantFieldArgs args)
        {
            if (!(args.VariantField is VariantScriban variantField))
                return;
            if (variantField.ScribanTemplate == null)
                variantField.ScribanTemplate = ScribanTemplateRenderer.Parse(variantField.Template, variantField.Path);
            var templateContext =
                ScribanTemplateRenderer.GetTemplateContext(args.IsControlEditable, args.RenderingWebEditingParams);
            templateContext.PushCulture(Context.Language.CultureInfo);
            string text;
            if (variantField.ScribanTemplate.HasErrors)
            {
                text = string.Join("<br/>",
                    variantField.ScribanTemplate.Messages.Select(m => HttpUtility.HtmlEncode(m.ToString()))) + "<br/>";
            }
            else
            {
                AddRuntimeScriptObjects(templateContext, variantField, args);
                try
                {
                    text = ScribanTemplateRenderer.Render(variantField.ScribanTemplate, templateContext);
                }
                catch (Exception ex)
                {
                    text = HttpUtility.HtmlEncode(ex.Message);
                }
            }

            if (variantField.IsHandlebarTemplate)
            {
                text = TransformHandlerBar(text);
                text = $"<script class='ow-template' type='text/x-handlebars-template'>{text}</script>";
            }

            Control control = new LiteralControl(text);
            if (!string.IsNullOrWhiteSpace(variantField.Tag))
            {
                var tag = new HtmlGenericControl(variantField.Tag);
                AddClass(tag, variantField.CssClass);
                AddWrapperDataAttributes(variantField, args, tag);
                args.ResultControl = tag;
            }

            args.ResultControl = control;
            args.Result = RenderControl(args.ResultControl);
        }

        protected virtual void AddRuntimeScriptObjects(
            TemplateContext templateContext,
            VariantScriban variantTemplate,
            RenderVariantFieldArgs args)
        {
            var scriptObject = new ScriptObject();
            scriptObject.Add("i_item", args.Item);
            scriptObject.Add("o_model", args.Model);
            scriptObject.Import("sc_placeholder", new RenderPlaceholder(RenderPlaceholderImpl));
            scriptObject.Import("sc_dynamic_placeholder", new RenderDynamicPlaceholder(RenderDynamicPlaceholderImpl));
            if (args.Parameters != null && args.Parameters.ContainsKey("geospatial"))
                scriptObject.Add("o_geospatial", args.Parameters["geospatial"]);
            AddChildExecutionFunction(variantTemplate, args, scriptObject);
            AddChildEvaluationFunction(variantTemplate, args, scriptObject);
            templateContext.PushGlobal(scriptObject);
        }

        public string RenderPlaceholderImpl(string placeholderKey, Item item = null)
        {
            var currentOrNull = PageContext.CurrentOrNull;
            return currentOrNull != null && currentOrNull.HtmlHelper != null
                ? GeneratePlaceholder(currentOrNull.HtmlHelper, item, placeholderKey, item != null)
                : string.Empty;
        }

        public string RenderDynamicPlaceholderImpl(string placeholderKey, Item item = null)
        {
            var dynamicPlaceholderId = RenderingContext.Current.Rendering.Parameters["DynamicPlaceholderId"];
            var currentOrNull = PageContext.CurrentOrNull;
            return currentOrNull != null && currentOrNull.HtmlHelper != null
                ? GeneratePlaceholder(currentOrNull.HtmlHelper, item, placeholderKey + "-" + dynamicPlaceholderId, item != null)
                : string.Empty;
        }

        public void Process(GenerateScribanContextPipelineArgs args)
        {
            args.GlobalScriptObject.Import("sc_raw", (StringDelegate)((item, fieldName) => item[fieldName]));
        }

        protected virtual void AddChildExecutionFunction(
            VariantScriban variantTemplate,
            RenderVariantFieldArgs args,
            ScriptObject scriptObject)
        {
            scriptObject.Import("sc_execute", (StringDelegate)((contextItem, childVariantName) =>
            {
                var baseVariantField =
                    variantTemplate.ChildItems.SingleOrDefault(item => item.ItemName.Is(childVariantName));
                if (baseVariantField != null)
                {
                    var args1 = new RenderVariantFieldArgs
                    {
                        VariantField = baseVariantField,
                        Item = contextItem ?? Sitecore.Context.Item,
                        HtmlHelper = args.HtmlHelper,
                        IsControlEditable = args.IsControlEditable,
                        IsFromComposite = args.IsFromComposite,
                        RenderingWebEditingParams = args.RenderingWebEditingParams,
                        RendererMode = args.RendererMode,
                        Model = args.Model
                    };
                    PipelineManager.Run("renderVariantField", args1);
                    if (!string.IsNullOrEmpty(args1.Result))
                        return args1.Result;
                    if (args1.ResultControl != null)
                        return RenderControl(args1.ResultControl);
                }

                return string.Empty;
            }));
        }

        protected virtual void AddChildEvaluationFunction(
            VariantScriban variantTemplate,
            RenderVariantFieldArgs args,
            ScriptObject scriptObject)
        {
            scriptObject.Import("sc_evaluate", (AddChildEvaluationDelegate)((item, childVariantName) =>
            {
                var baseVariantField =
                    variantTemplate.ChildItems.SingleOrDefault(childItem => childItem.ItemName.Is(childVariantName));
                if (baseVariantField == null)
                    return false;
                var rules = baseVariantField.Rules;
                var ruleContext = new RuleContext
                {
                    Item = item ?? args.Item
                };
                ruleContext.Parameters.Add("Model", args.Model);
                return rules != null && RulesService.EvaluateRules(rules, ruleContext);
            }));
        }

        private delegate string ContentImagesDelegate(Item item);

        private delegate string StringDelegate(Item item, string variantName);

        private delegate bool AddChildEvaluationDelegate(Item item, string variantName);

        private delegate string RenderPlaceholder(string placeholderKey, Item item = null);

        private delegate string RenderDynamicPlaceholder(string placeholderKey, Item item = null);

        private string TransformHandlerBar(string inputText)
        {
            inputText = inputText.Replace("[[[", "{{{").Replace("]]]", "}}}");
            inputText = inputText.Replace("[[", "{{").Replace("]]", "}}");
            return inputText;
        }
    }
}