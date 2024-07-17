using Formri.Domain.Common.Configurations;
using Formri.Domain.Repositories.User;
using Formri.Domain.Services.DatabaseManager;
using Formri.Domain.Services.User;
using Formri.Domain.Services.UserCache;
using Formri.Persistence.Repositories;
using Formri.Persistence.Services.DatabaseManager;
using Formri.Persistence.Services.User;
using Formri.Persistence.Services.UserCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Formri.Persistence
{
    public static class DependencyInjection
    {
        public static WebApplicationBuilder ConfigurePersistence(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IUserCache, UserCache>();

            builder.Services.AddSingleton<IDatabaseManagerService, DatabaseManagerService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            return builder;
        }

        public static WebApplicationBuilder ConfigurePersistenceOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection(DatabaseConfiguration.Section));

            return builder;
        }
    }
}
