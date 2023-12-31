﻿namespace OneWeb.Feature.Form.Model
{
    public class EmailModel
    {
        public virtual string From { get; set; }

        public virtual string To { get; set; }

        public virtual string Cc { get; set; }

        public virtual string Bcc { get; set; }

        public virtual string Subject { get; set; }

        public virtual string Message { get; set; }

        public virtual bool IsHtml { get; set; }

        public virtual string CustomSmtpConfig { get; set; }
    }
}