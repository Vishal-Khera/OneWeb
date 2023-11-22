using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OneWeb.Foundation.SitecoreExtensions.Models;
using Sitecore;
using Sitecore.Data;

namespace OneWeb.Foundation.SitecoreExtensions.Extensions
{
    public static class StringExtensions
    {
        public static ID ParseId(string stringId)
        {
            ID id = null;
            if (!string.IsNullOrWhiteSpace(stringId))
            {
                if (ID.TryParse(stringId, out id))
                {
                    return id;
                }

                if (ShortID.TryParse(stringId, out var shortId)) return shortId.ToID();
            }

            return id;
        }

        public static string Sanitize(this string input)
        {
            return Regex.Replace(input, "(\\B[A-Z])", " $1");
        }

        public static string ToCssUrlValue(this string url)
        {
            return string.IsNullOrWhiteSpace(url) ? "none" : $"url('{url}')";
        }

        public static string RemoveSpecialCharacters(string inputString, bool ensureLowercase = false)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;
            return ensureLowercase ? Regex.Replace(inputString, @"[^0-9a-zA-Z]+", string.Empty).ToLower() : Regex.Replace(inputString, @"[^0-9a-zA-Z]+", string.Empty);
        }

        public static bool IdEqualsGuid(ID id, string guidString)
        {
            var strId = GetTemplateGuidString(id);
            return strId.Equals(guidString, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string GetTemplateGuidString(ID id)
        {
            var strId = id.ToString().ToLower();
            return Regex.Replace(strId, @"[{}]+", string.Empty);
        }

        public static string SplitString(string inputString, int charLimit, char splittingCharacter = ' ',
            string trailingChars = "")
        {
            if (inputString.Length > charLimit)
            {
                var extraString = inputString.Substring(charLimit);
                var splittingCharIndex = extraString.IndexOf(splittingCharacter);
                if (splittingCharIndex != -1)
                    return inputString.Substring(0, charLimit + splittingCharIndex) + trailingChars;
            }

            return inputString;
        }

        public static string TruncateAtWord(string value, int maxLength = 100, string trailingChars = "")
        {
            if (value == null || value.Length < maxLength ||
                value.IndexOf(" ", maxLength, StringComparison.Ordinal) == -1) return value;

            return value.Substring(0, value.LastIndexOf(" ", maxLength, StringComparison.Ordinal)) + trailingChars;
        }
        public static string StripAnchorTags(string fieldValue)
        {
            var regex = @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>";
            var stripedContent = Regex.Replace(fieldValue, regex, "$1");
            return stripedContent;
        }

        public static string SanitizeSearchString(string inputString)
        {
            if(inputString == null) return null;

            return inputString.Replace("-","xxx");
        }

        public static string DeSanitizeSearchString(string inputString)
        {
            if (inputString == null) return null;

            return inputString.Replace("xxx", "-");
        }

        public static List<AdvancedTag> ParseAdvancedTags(string tagString)
        {
            var tagModel = new List<AdvancedTag>();
            var tagList =tagString.Split('|');
            if (tagList.Length > 0)
            {
                foreach (var tag in tagList)
                {
                    tagModel.Add(new AdvancedTag()
                    {
                        Id = tag.Split(';')[0],
                        Title = tag.Split(';')[1],
                    });
                }
            }

            return tagModel;
        }
        
        public static string ParseAdvancedTags(List<AdvancedTag> advancedTags)
        {
            string outString = null;
            foreach (var tag in advancedTags)
            {
                if (outString == null)
                {
                    outString = $"{tag.Id};{tag.Title}";
                }
                else
                {
                    outString = string.Join("|", new[] { outString, $"{tag.Id};{tag.Title}" });
                }
                
            }

            return outString;
        }
    }
}