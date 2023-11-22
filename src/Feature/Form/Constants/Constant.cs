namespace OneWeb.Feature.Form.Constants
{
    public class Constant
    {
        public static class LogsProperties
        {
            public static readonly string CC = "CC";
            public static readonly string Bcc = "Bcc";
        }

        public static class Settings
        {
            public static readonly string SmtpServerDetails = "";
        }
        public static class FormContainer
        {
            public static readonly string Source = "Source";
            public static readonly string Title = "Title";
            public static readonly string Description = "Description";
            public static readonly string ReferenceNumber = "reference number";
            public static readonly string RefNumber = "ref number";
            public static readonly string DefaultValue = "Default Value";
        }
        public enum ReferenceNumberAccessType
        {
            IncrementAndLock,
            Read,
            ReadAndRelease
        }
    }
}