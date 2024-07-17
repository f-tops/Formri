using Formri.Domain;
using Formri.Domain.Extensions;
using Formri.Domain.Services.DatabaseManager;
using Formri.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.AddServiceDefaults();

builder.ConfigureEmailService();
builder.ConfigureEmailOptions();

builder.ConfigurePersistenceOptions();

builder.ConfigurePersistence();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var databaseManagerService = scope.ServiceProvider.GetRequiredService<IDatabaseManagerService>();
    await databaseManagerService.InitializeDatabase(CancellationToken.None);
}

app.UseExceptionHandler();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();