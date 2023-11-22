using System;
using System.Collections.Generic;

namespace OneWeb.Feature.Form.Model
{
    public partial class SendDataModel
    {
        public Guid InternalItemId { get; set; }
        public string ExternalLink { get; set; }
        public string Anchor { get; set; }
        public string QueryString { get; set; }
    }
    public class ApiResponse
    {
        public string Token { get; set; }

        public string EmailAddress { get; set; }

        public string UserId { get; set; }
        public bool Success { get; set; }
        public int ResponseCode { get; set; }
        public string Message { get; set; }
    }
}
