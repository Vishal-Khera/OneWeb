using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using OneWeb.Foundation.SitecoreExtensions.Overrides.Pipelines.XA.GetScribanItemMembers;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Sitecore.Abstractions;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Mvc.Models;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions.XA
{
    public class ItemAccessor : IObjectAccessor
    {
        protected readonly BaseCorePipelineManager BaseCorePipelineManager;
        protected readonly IContext Context;
        protected readonly RenderingWebEditingParams RenderingWebEditingParams;
        private List<string> _staticMembers;

        public ItemAccessor(RenderingWebEditingParams webEditingParams)
        {
            Context = ServiceLocator.ServiceProvider.GetService<IContext>();
            BaseCorePipelineManager = ServiceLocator.ServiceProvider.GetService<BaseCorePipelineManager>();
            RenderingWebEditingParams = webEditingParams;
        }

        protected List<string> StaticMembers
        {
            get
            {
                if (_staticMembers == null)
                {
                    var args = new GetScribanItemMembersPipelineArgs
                    {
                        Mode = MemberMode.Name
                    };
                    BaseCorePipelineManager.Run("getScribanItemMembers", args);
                    _staticMembers = args.Members;
                }

                return _staticMembers;
            }
        }

        public int GetMemberCount(TemplateContext context, SourceSpan span, object target)
        {
            return GetMembers(context, span, target).Count();
        }

        public IEnumerable<string> GetMembers(
            TemplateContext context,
            SourceSpan span,
            object target)
        {
            return GetMembers(target);
        }

        public bool HasMember(TemplateContext context, SourceSpan span, object target, string member)
        {
            return true;
        }

        public bool TryGetValue(
            TemplateContext context,
            SourceSpan span,
            object target,
            string member,
            out object value)
        {
            var obj = target as Item;
            return TryGetValue(member, out value, obj);
        }

        public bool TrySetValue(
            TemplateContext context,
            SourceSpan span,
            object target,
            string member,
            object value)
        {
            throw new InvalidOperationException("Unable to change item properties during the rendering process");
        }

        public IEnumerable<string> GetMembers(object target)
        {
            var members = new List<string>(StaticMembers);
            if (target is Item obj)
            {
                var collection = obj.Fields.Select(f => f.Name);
                members.AddRange(collection);
            }

            return members;
        }

        public bool TryGetValue(string member, out object value, Item item)
        {
            value = string.Empty;
            member = member.ToLower(CultureInfo.InvariantCulture);
            var args = new GetScribanItemMembersPipelineArgs
            {
                Mode = MemberMode.Value,
                Item = item,
                MemberName = member
            };
            BaseCorePipelineManager.Run("getScribanItemMembers", args);
            bool flag;
            if (args.MemberValue != null)
            {
                flag = true;
                value = args.MemberValue;
            }
            else
            {
                var field = item.Fields[member];
                if (field != null)
                {
                    value = new FieldWrapper(item, field, member, RenderingWebEditingParams);
                    flag = true;
                }
                else
                {
                    value = new FieldFallback(item, member);
                    flag = false;
                }
            }

            return flag;
        }
    }
}