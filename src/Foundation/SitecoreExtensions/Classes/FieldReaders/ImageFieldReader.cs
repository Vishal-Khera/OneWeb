using Newtonsoft.Json;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.Abstractions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.FieldReaders;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;

namespace OneWeb.Foundation.SitecoreExtensions.Classes.FieldReaders
{
    public class ImageFieldReader : FieldReader
    {
        /// <summary>Gets the field value.</summary>
        /// <param name="field">The field.</param>
        /// <returns>The field value.</returns>
        public override object GetFieldValue(IIndexableDataField indexableField)
        {
            Field field1 = (Field)(indexableField as SitecoreItemDataField);
            BaseFieldTypeManager instance = ContentSearchManager.Locator.GetInstance<BaseFieldTypeManager>();
            Assert.IsNotNull((object)instance, "fieldTypeManager != null");
            if (!(instance.GetField(field1) is ImageField field2))
                return (object)null;
            if(field2.MediaItem == null)
                return (object)null;
            return JsonConvert.SerializeObject(new MediaModel(field2.MediaItem));
        }
    }
}