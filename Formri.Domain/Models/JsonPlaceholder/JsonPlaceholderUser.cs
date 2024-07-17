namespace Formri.Domain.Models.JsonPlaceholder
{
    public class JsonPlaceholderUser
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public JsonPlaceholderAddress? Address { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public JsonPlaceholderCompany? Company { get; set; }
    }
}
