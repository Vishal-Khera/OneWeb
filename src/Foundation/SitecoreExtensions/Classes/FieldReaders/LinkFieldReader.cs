using Newtonsoft.Json;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Abstractions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.FieldReaders;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;

namespace OneWeb.Foundation.SitecoreExtensions.Classes.FieldReaders
{
    /// <summary>The lookup field reader.</summary>
    public class LinkFieldReader : FieldReader
    {
        /// <summary>Gets the field value.</summary>
        /// <param name="field">The field.</param>
        /// <returns> The field value. </returns>
        public override object GetFieldValue(IIndexableDataField indexableField)
        {
            Field field = (Field)(indexableField as SitecoreItemDataField);
            BaseFieldTypeManager instance = ContentSearchManager.Locator.GetInstance<BaseFieldTypeManager>();
            Assert.IsNotNull((object)instance, "fieldTypeManager != null");
            if (instance.GetField(field) is LinkField linkField && !string.IsNullOrWhiteSpace(linkField.Value))
            {
                return JsonConvert.SerializeObject(new LinkModel(field));
            }
            return (object)null;
        }
    }
}