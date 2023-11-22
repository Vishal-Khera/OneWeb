using Newtonsoft.Json.Linq;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class NewtonsoftExtensions
    {
        public static void AddStringAttribute(this JObject jObject, string attributeName, Item item, string fieldName)
        {
            var fieldValue = item?.GetFieldValue(fieldName);
            AddStringAttribute(jObject, attributeName, fieldValue);
        }

        public static void AddStringAttribute(this JObject jObject, string attributeName, string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue)) return;

            var jProperty = new JProperty(attributeName, fieldValue);
            jObject.Add(jProperty);
        }

        public static void AddMediaModelAttribute(this JObject jObject, string attributeName, Item item, string fieldName)
        {
            var mediaModel = new MediaModel(item, fieldName);
            if (string.IsNullOrWhiteSpace(mediaModel.Url)) return;

            var mediaObject = new JObject();
            AddStringAttribute(mediaObject, "Url", mediaModel.Url);
            AddStringAttribute(mediaObject, "Title", mediaModel.Title);
            AddStringAttribute(mediaObject, "Alt", mediaModel.Alt);

            var mediaProperty = new JProperty(attributeName, mediaObject);
            jObject.Add(mediaProperty);
        }

        public static void AddLinkModelAttribute(this JObject jObject, string attributeName, Item item, string fieldName)
        {
            var linkModel = new LinkModel(item, fieldName);
            if (string.IsNullOrWhiteSpace(linkModel.Url)) return;

            var linkObject = new JObject();
            AddStringAttribute(linkObject, "Url", linkModel.Url);
            AddStringAttribute(linkObject, "Title", linkModel.Title);
            AddStringAttribute(linkObject, "Text", linkModel.Text);
            AddStringAttribute(linkObject, "Target", linkModel.Target);
            var linkProperty = new JProperty(attributeName, linkObject);
            jObject.Add(linkProperty);
        }
    }
}