using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.SitecoreExtensions.Models
{
    public class LinkModel
    {
        public LinkModel()
        {
        }

        public LinkModel(Item item, string fieldName)
        {
            var linkField = (LinkField)item.Fields[fieldName];
            GetModel(linkField);
        }

        public LinkModel(Item item, ID fieldId)
        {
            if (item == null || fieldId.IsNull)
                new LinkModel();
            var linkField = (LinkField)item.Fields[fieldId];
            GetModel(linkField);
        }

        public LinkModel(LinkField linkField)
        {
            GetModel(linkField);
        }

        public string Url { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }

        public string Anchor { get; set; }

        public string Target { get; set; }

        public string Class { get; set; }

        private void GetModel(LinkField linkField)
        {
            if (linkField != null)
            {
                Url = FieldExtensions.GetLinkFieldUrl(linkField);
                Text = linkField.Text;
                Title = linkField.Title;
                Anchor = linkField.Anchor;
                Target = linkField.Target == "|Custom" ? "_self" : linkField.Target;
                Class = linkField.Class;
            }
        }
    }
}