using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;
using OneWeb.Foundation.SitecoreExtensions.Logging;
using OneWeb.Foundation.SitecoreExtensions.Models;

namespace OneWeb.Foundation.SitecoreExtensions.Classes.TypeConverters
{
    [Description("Converter with as core type System.String, for mapping a field with a MediaModel")]
    public class MediaModelTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            switch (sourceType.FullName)
            {
                case "System.string":
                case "System.String":
                    return true;
                default:
                    return false;
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            switch (destinationType.FullName)
            {
                case "OneWeb.Foundation.SitecoreExtensions.Models.MediaModel":
                    return true;
                default:
                    return false;
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            MediaModel toReturn = null;
            if (value != null)
                switch (value.GetType().FullName)
                {
                    case "System.string":
                    case "System.String":
                        try
                        {
                            var stringValue = (string)value;
                            toReturn = string.IsNullOrWhiteSpace(stringValue) ? new MediaModel() : JsonConvert.DeserializeObject<MediaModel>(stringValue);
                        }
                        catch (Exception e)
                        {
                            LogManager.LogError("Conversion from a value of type '" + value.GetType() + "' to MediaModel isn't supported", e);
                        }
                        
                        break;
                    default:
                        LogManager.LogError("Conversion from a value of type '" + value.GetType() + "' to MediaModel isn't supported");
                        break;
                }

            return toReturn;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (value == null) throw new ArgumentNullException("value", "Value can't be null");

            switch (destinationType.FullName)
            {
                case "OneWeb.Foundation.SitecoreExtensions.Models.MediaModel":
                    return JsonConvert.SerializeObject(value);
                default:
                    throw new NotSupportedException("Conversion to a value of type '" + destinationType +
                                                    "' isn't supported");
            }
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return true;
        }
    }
}