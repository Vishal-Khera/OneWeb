using System;
using System.Collections.Generic;
using OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Sitecore.XA.Foundation.Mvc.Models;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions.XA
{
    public class FieldFallbackAccessor : FieldRendererBase, IObjectAccessor
    {
        public FieldFallbackAccessor(RenderingWebEditingParams webEditingParams)
        {
            RenderingWebEditingParams = webEditingParams;
        }

        public int GetMemberCount(TemplateContext context, SourceSpan span, object target)
        {
            return 0;
        }

        public IEnumerable<string> GetMembers(
            TemplateContext context,
            SourceSpan span,
            object target)
        {
            return new List<string>(0);
        }

        public bool HasMember(TemplateContext context, SourceSpan span, object target, string member)
        {
            return false;
        }

        public bool TryGetValue(
            TemplateContext context,
            SourceSpan span,
            object target,
            string member,
            out object value)
        {
            var fieldFallback = target as FieldFallback;
            bool flag;
            if (fieldFallback.Item.Fields[member] != null)
            {
                var fieldRenderer = CreateFieldRenderer(fieldFallback.Item, member);
                value = fieldRenderer.Render();
                flag = true;
            }
            else
            {
                value = new FieldFallback(fieldFallback.Item, fieldFallback.FallbackFrom);
                flag = true;
            }

            return flag;
        }

        public bool TrySetValue(
            TemplateContext context,
            SourceSpan span,
            object target,
            string member,
            object value)
        {
            throw new InvalidOperationException("Unable to change field properties during the rendering process");
        }
    }
}