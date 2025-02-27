using Api.Configures;
using Api.Extensions;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;

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
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        // Swagger configuration
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MessagingService Api", Version = "v1" });

        // Search for the generated documentation file and add comments to the swager specification
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    var app = builder.Build();

    #region Pipelines
    #region Scalar
    if (app.Environment.IsDevelopment())
    {
        // Specification
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "openapi/{documentName}.json";
        });

        // UI
        app.MapScalarApiReference(options =>
        {
            options.Title = "MessagingService";
            options.Theme = ScalarTheme.Mars;
            options.ShowSidebar = true;
            options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }
    #endregion

    #region Swagger
    else
    {
        // Specification
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        // UI
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MessagingService Api v1");
        });
    }
    #endregion
    #endregion

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