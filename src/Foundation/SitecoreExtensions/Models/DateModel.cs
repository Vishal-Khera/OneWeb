using System;
using System.Globalization;
using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
namespace OneWeb.Foundation.SitecoreExtensions.Models
{
    public class DateModel
    {
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public DateModel(string date)
        {
            if (string.IsNullOrWhiteSpace(date) || !DateTime.TryParse(date,
                                                            out var dt))
            {
                return;
            }

            Day = dt.ToString("dd");
            Month = dt.ToString("MMM");
            Year = dt.ToString("yy");

        }
    }
}