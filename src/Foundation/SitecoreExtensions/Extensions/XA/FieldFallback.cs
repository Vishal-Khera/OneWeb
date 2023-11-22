using System.Collections.Generic;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions.XA
{
    public class FieldFallback : IScriptObject
    {
        public FieldFallback(Item item, string member)
        {
            Item = item;
            FallbackFrom = member;
        }

        public Item Item { get; }

        public string FallbackFrom { get; }

        public int Count => 0;

        public bool IsReadOnly
        {
            get => true;
            set { }
        }

        public IEnumerable<string> GetMembers()
        {
            return new List<string>();
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
            value = new FieldFallback(Item, member);
            return false;
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
        }

        public bool Remove(string member)
        {
            return false;
        }

        public void SetReadOnly(string member, bool readOnly)
        {
        }

        public IScriptObject Clone(bool deep)
        {
            return this;
        }

        public override string ToString()
        {
            return null;
        }
    }
}