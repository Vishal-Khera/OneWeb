using System.Collections.Generic;

namespace OneWeb.Foundation.Login.Models
{
    public class MailTemplate
    {
        public string From { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}