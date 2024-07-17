using Formri.Domain.Common.Configurations;
using Formri.Domain.Services.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Formri.Domain
{
    public static class DependencyInjection
    {
        public static WebApplicationBuilder ConfigureEmailService(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IEmailService, EmailService>();

            return builder;
        }

        public static WebApplicationBuilder ConfigureEmailOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection(EmailConfiguration.Section));

            return builder;
        }
    }
}
