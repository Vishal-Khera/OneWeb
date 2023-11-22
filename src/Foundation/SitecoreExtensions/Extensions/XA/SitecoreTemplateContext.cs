// Decompiled with JetBrains decompiler
// Type: Sitecore.XA.Foundation.Scriban.ContextExtensions.SitecoreTemplateContext
// Assembly: Sitecore.XA.Foundation.Scriban, Version=6.10.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11AE1DEC-A15D-4D6E-9649-838ED84316F4
// Assembly location: C:\inetpub\wwwroot\onewebsc.dev.local\bin\Sitecore.XA.Foundation.Scriban.dll

using System;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Scriban.Syntax;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Mvc.Models;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions.XA
{
    public class SitecoreTemplateContext : TemplateContext
    {
        protected FieldFallbackAccessor FieldFallbackAccessor;
        protected ItemAccessor ItemAccessor;

        public SitecoreTemplateContext(RenderingWebEditingParams webEditingParams)
        {
            ItemAccessor = new ItemAccessor(webEditingParams);
            FieldFallbackAccessor = new FieldFallbackAccessor(webEditingParams);
        }

        protected override IObjectAccessor GetMemberAccessorImpl(object target)
        {
            switch (target)
            {
                case Item _:
                    return ItemAccessor;
                case FieldFallback _:
                    return FieldFallbackAccessor;
                default:
                    return base.GetMemberAccessorImpl(target);
            }
        }

        public override object IsEmpty(SourceSpan span, object against)
        {
            switch (against)
            {
                case FieldFallback _:
                    return true;
                case FieldWrapper _:
                    return string.IsNullOrEmpty(((FieldWrapper)against).Field.Value);
                default:
                    return base.IsEmpty(span, against);
            }
        }

        public override string ToString(SourceSpan span, object value)
        {
            switch (value)
            {
                case FieldFallback _:
                    return null;
                case Item _:
                    return ((Item)value).Paths.FullPath;
                default:
                    return base.ToString(span, value);
            }
        }

        public override object ToObject(SourceSpan span, object value, Type destinationType)
        {
            return base.ToObject(span, value, destinationType);
        }

        protected override object EvaluateImpl(ScriptNode scriptNode)
        {
            return base.EvaluateImpl(scriptNode);
        }

        public override bool ToBool(SourceSpan span, object value)
        {
            return base.ToBool(span, value);
        }
    }
}