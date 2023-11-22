using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Mvc.Models;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions.XA
{
    public class FieldWrapper : FieldRendererBase, IScriptObject
    {
        public FieldWrapper(
            Item item,
            Field field,
            string fieldName,
            RenderingWebEditingParams webEditingParams)
        {
            SitecoreItem = item;
            Field = field;
            Name = fieldName;
            RenderingWebEditingParams = webEditingParams;
        }

        public Item SitecoreItem { get; }

        public Field Field { get; }

        public string Name { get; }

        public string Raw => Field != null ? Field.Value : Name;

        public IEnumerable<Item> Targets => ItemExtensions.GetTargetItems(SitecoreItem, Name);

        public Item Target => ItemExtensions.GetTargetItems(SitecoreItem, Name).FirstOrDefault();

        public int Count => 0;

        public bool IsReadOnly
        {
            get => true;
            set { }
        }

        public IEnumerable<string> GetMembers()
        {
            return new List<string>(0);
        }

        public bool Contains(string member)
        {
            return false;
        }

        public bool TryGetValue(
            TemplateContext context,
            SourceSpan span,
            string member,
            out object value)
        {
            member = member.ToLower(CultureInfo.InvariantCulture);
            switch (member)
            {
                case "raw":
                    value = Raw;
                    return true;
                case "target":
                    value = Target;
                    return true;
                case "targets":
                    value = Targets;
                    return true;
                default:
                    var target = Target;
                    if (target != null)
                        return new ItemAccessor(RenderingWebEditingParams).TryGetValue(context, span, target, member,
                            out value);
                    value = new FieldFallback(Field.InnerItem, member);
                    return false;
            }
        }

        public bool CanWrite(string member)
        {
            return false;
        }

        public void SetValue(
            TemplateContext context,
            SourceSpan span,
            string member,
            object value,
            bool readOnly)
        {
            throw new InvalidOperationException("Unable to change field properties during the rendering process");
        }

        public bool Remove(string member)
        {
            throw new InvalidOperationException("Unable to change field properties during the rendering process");
        }

        public void SetReadOnly(string member, bool readOnly)
        {
            throw new InvalidOperationException("Unable to change field properties during the rendering process");
        }

        public IScriptObject Clone(bool deep)
        {
            return this;
        }

        public override string ToString()
        {
            if (Field == null)
                return Name;
            var fieldRenderer = CreateFieldRenderer(Field.Item, Field.Name);
            fieldRenderer.ID = Field.ID.ToString();
            return fieldRenderer.Render();
        }
    }
}