namespace Formri.Domain.Models.JsonPlaceholder
{
    public class JsonPlaceholderAddress
    {
        public string? Street { get; set; }
        public string? Suite { get; set; }
        public string? City { get; set; }
        public string? Zipcode { get; set; }
        public JsonPlaceholderGeo? Geo { get; set; }
    }
}
