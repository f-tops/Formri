namespace Formri.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }

        public string? Street { get; set; }

        public string? Suite { get; set; }

        public string? City { get; set; }

        public string? Zipcode { get; set; }

        public string? Lat { get; set; }

        public string? Lng { get; set; }
    }
}
