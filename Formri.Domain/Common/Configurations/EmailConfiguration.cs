namespace Formri.Domain.Common.Configurations
{
    public class EmailConfiguration
    {
        public const string Section = "EmailServer";

        public string EmailFrom { get; set; }

        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUser { get; set; }

        public string SmtpPass { get; set; }
    }
}
