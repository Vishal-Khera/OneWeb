using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.SolrProvider;
using Sitecore.Data;

namespace OneWeb.Foundation.SitecoreExtensions.Overrides.Solr
{
    public class OneWebFieldDocumentBuilder : SolrDocumentBuilder
    {
        private readonly SolrFieldNameTranslator _fieldNameTranslator;

        private readonly IFieldMap _fieldMap;

        private readonly CultureInfo _culture;
        public OneWebFieldDocumentBuilder(IIndexable indexable, IProviderUpdateContext context) : base(indexable, context)
        {
            this._fieldNameTranslator = context.Index.FieldNameTranslator as SolrFieldNameTranslator;
            this._fieldMap = context.Index.Configuration.FieldMap;
            this._culture = indexable.Culture;
        }
        protected override void AddComputedIndexField(IComputedIndexField computedIndexField, object fieldValue)
        {
            if (computedIndexField.ReturnType == "advancedTags")
            {
                this.AddMultipleComputedIndexFields(computedIndexField);
            }
            else
            {
                base.AddComputedIndexField(computedIndexField, fieldValue);
            }
        }

        private void AddMultipleComputedIndexFields(IComputedIndexField computedIndexField)
        {
            try
            {
                var fieldValue = computedIndexField.ComputeFieldValue(this.Indexable);
                var fieldValues = fieldValue as Dictionary<string, List<AdvancedTag>>;
                if (fieldValues == null)
                {
                    return;
                }
                foreach (var dictionaryEntry in fieldValues.Keys)
                {
                    this.AddFieldCustom(dictionaryEntry, fieldValues[dictionaryEntry], false);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Unable to tranform the field data" + ex.ToString(), this);
            }
        }
        protected void AddFieldCustom(string fieldName, object fieldValue, bool append = false)
        {
            var formattedFieldValue = fieldValue;
            var tagModelList = fieldValue as List<AdvancedTag>;
            if (tagModelList == null)
            {
                return;
            }

            fieldValue = StringExtensions.ParseAdvancedTags(tagModelList);
            AbstractSearchFieldConfiguration fieldConfiguration = _fieldMap.GetFieldConfiguration(fieldName);
            if (fieldConfiguration != null)
            {
                fieldValue = fieldConfiguration.FormatForWriting(fieldValue);
            }

            if (fieldValue == null)
            {
                VerboseLogging.CrawlingLogDebug(() => "Skipping field name:" + fieldName + " - Value is empty.");
            }
            else
            {
                //string indexFieldName = _fieldNameTranslator.GetIndexFieldName(fieldName, fieldValue.GetType(), _culture);
                string indexFieldName = fieldName.ToLower();
                StoreField(fieldName, indexFieldName+"_s", fieldValue, append, returnType: "string");
                StoreField(fieldName, indexFieldName + "_sm", tagModelList.Select(x=> x.Title), append, returnType: "stringCollection");
            }
        }
        private void StoreField(string unTranslatedFieldName, string fieldName, object fieldValue, bool append = false, string returnType = null)
        {
            object source = fieldValue;
            if (this.Index.Configuration.IndexFieldStorageValueFormatter != null)
                fieldValue = this.Index.Configuration.IndexFieldStorageValueFormatter.FormatValueForIndexStorage(fieldValue, unTranslatedFieldName);
            if (VerboseLogging.Enabled)
            {
                StringBuilder stringBuilder1 = new StringBuilder();
                stringBuilder1.AppendFormat("Field: {0}" + Environment.NewLine, (object)fieldName);
                StringBuilder stringBuilder2 = stringBuilder1;
                string format = " - value: {0}{1}" + Environment.NewLine;
                string str1 = source != null ? source.GetType().ToString() : "NULL";
                string str2;
                switch (source)
                {
                    case IEnumerable _:
                        str2 = " - count : " + (object)((IEnumerable)source).Cast<object>().Count<object>();
                        break;
                    default:
                        str2 = "";
                        break;
                }
                stringBuilder2.AppendFormat(format, (object)str1, (object)str2);
                stringBuilder1.AppendFormat(" - unformatted value: {0}" + Environment.NewLine, source ?? (object)"NULL");
                stringBuilder1.AppendFormat(" - formatted value:   {0}" + Environment.NewLine, fieldValue ?? (object)"NULL");
                stringBuilder1.AppendFormat(" - returnType: {0}" + Environment.NewLine, (object)returnType);
                stringBuilder1.AppendFormat(" - append: {0}" + Environment.NewLine, (object)append);
                VerboseLogging.CrawlingLogDebug(new Func<string>(((object)stringBuilder1).ToString));
            }
            if (append && this.Document.ContainsKey(fieldName) && fieldValue is string)
            {
                ConcurrentDictionary<string, object> document = this.Document;
                string key = fieldName;
                document[key] = (object)(document[key].ToString() + " " + (string)fieldValue);
            }
            if (this.Document.ContainsKey(fieldName))
                return;
            this.Document.GetOrAdd(fieldName, fieldValue);
            //if (!this._fieldNameTranslator.HasCulture(fieldName))
            //    return;
            //this.Document.GetOrAdd(this._fieldNameTranslator.GetIndexFieldNameByType(unTranslatedFieldName, "textCollection", CultureInfo.InvariantCulture), fieldValue);
        }
    }
}