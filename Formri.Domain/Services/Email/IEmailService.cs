using Formri.Domain.Models.User;

namespace Formri.Domain.Services.Email
{
    public interface IEmailService
    {
        public Task SendEmail(EmailUser user);
    }
}
