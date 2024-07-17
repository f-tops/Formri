using System.ComponentModel.DataAnnotations;

namespace Formri.Domain.Models.ContactForm
{
    public class ContactFormModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
