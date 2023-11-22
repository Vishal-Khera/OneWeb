using System.Collections.Generic;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models.Pipelines.XA;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions;
using Sitecore.XA.Foundation.Variants.Abstractions.Fields;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.ParseVariantFields;
using Sitecore.XA.Foundation.Variants.Abstractions.Services;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.ParseVariantFields
{
    public class ParseScriban : ParseVariantFieldProcessor
    {
        protected readonly IVariantFieldParser VariantFieldParser;

        public ParseScriban(IVariantFieldParser variantFieldParser)
        {
            VariantFieldParser = variantFieldParser;
        }

        public override ID SupportedTemplateId => Serialization.OneWebScriban.TemplateId;

        public override void TranslateField(ParseVariantFieldArgs args)
        {
            var variantFieldArgs = args;
            var variantScriban = new VariantScriban(args.VariantItem);
            variantScriban.ItemName = args.VariantItem.Name;
            variantScriban.Path = GetTemplatePath(args.VariantItem);
            variantScriban.Tag = args.VariantItem.Fields[Serialization.OneWebScriban.Fields.Tag].GetEnumValue();
            variantScriban.Template = args.VariantItem[Serialization.OneWebScriban.Fields.Template];
            variantScriban.CssClass = args.VariantItem[Serialization.OneWebScriban.Fields.CssClass];
            variantScriban.ChildItems = args.VariantItem.Children.Count > 0
                ? VariantFieldParser.ParseVariantFields(args.VariantItem, args.VariantRootItem, false)
                : (IEnumerable<BaseVariantField>)new List<BaseVariantField>();
            variantScriban.IsHandlebarTemplate =
                args.VariantItem.GetCheckboxStatus(Serialization.OneWebScriban.Fields.IsHandlebarTemplate_FieldName);
            variantFieldArgs.TranslatedField = variantScriban;
        }

        private string GetTemplatePath(Item variantItem)
        {
            return variantItem.Parent == null ||
                   variantItem.InheritsFrom(Templates.VariantsGrouping.ID)
                ? string.Empty
                : GetTemplatePath(variantItem.Parent) + "/" + variantItem.Name;
        }
    }
}