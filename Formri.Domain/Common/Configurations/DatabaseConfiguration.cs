namespace Formri.Domain.Common.Configurations
{
    public class DatabaseConfiguration
    {
        public const string Section = "Database";

        public required string ConnectionString { get; set; }
    }
}
