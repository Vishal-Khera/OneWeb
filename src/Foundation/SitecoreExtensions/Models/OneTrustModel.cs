using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneWeb.Foundation.SitecoreExtensions.Models
{
    public class OneTrustModel
    {
        public string Id { get; set; }
        public bool DisableCookieAutoBlocking { get; set; }
        public bool ReloadOnConsentChange { get; set; }
    }
}