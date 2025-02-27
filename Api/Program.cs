using Api.Configures;
using Api.Extensions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Configuration
    string environment = builder.Environment.EnvironmentName;
    builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
    ConfigurationManager configuration = builder.Configuration;
    configuration.AddConfigurationExtension(environment);
    #endregion

    #region Serilog
    Log.Logger = builder.AddLogger(configuration).ForContext<Program>();
    #endregion

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    Log.Logger.Information("Environment: {Environment} Launch", environment);
    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "error message");
}
finally
{
    Log.Logger.Information("Shutdown is complete");
    await Log.CloseAndFlushAsync();
}