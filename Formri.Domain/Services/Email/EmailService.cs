using Formri.Domain.Common.Configurations;
using Formri.Domain.Models.User;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Formri.Domain.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<EmailConfiguration> options,
            ILogger<EmailService> logger)
        {
            _emailConfiguration = options.Value;
            _logger = logger;
        }

        public async Task SendEmail(EmailUser user)
        {
            var parsedToMail = new MailAddress(user.Email);
            var parsedFromMail = new MailAddress(_emailConfiguration.EmailFrom);

            var message = new MailMessage(parsedFromMail, parsedToMail)
            {
                Subject = "New Contact Form Submission",
                Body = $"Name: {user.Name}\n" +
                       $"Email: {user.Email}\n" +
                       $"Phone: {user.Phone}\n" +
                       $"Website: {user.Website}\n" +
                       $"Address: {user.Address}"
            };

            using (var client = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailConfiguration.SmtpUser, _emailConfiguration.SmtpPass),
                EnableSsl = true
            })
            {
                try
                {
                    _logger.LogInformation($"Trying to send user {user.Name} email..");
                    await client.SendMailAsync(message);
                }
                catch (Exception)
                {
                    _logger.LogError("Unable to send user email at this moment.");
                }
            }
        }
    }
}
