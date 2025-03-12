using Api.Configures;
using Api.Configures.Hub;
using Api.Extensions;
using Application.Common.Behaviors;
using Application.Handlers.Commands.Base;
using Domain.Models.Handlers.Commands.Base;
using MediatR;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

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

    #region Configures
    builder.Services.AddMapperConfigure();
    builder.Services.AddValidatorsConfigure();
    builder.Services.AddInfrastructureConfigure();
    builder.Services.AddSignalR();

    #region CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp",
            builder =>
            {
                builder.WithOrigins("http://localhost:60717", "http://localhost:60719", "http://localhost:60718")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
    });
    #endregion

    #region MediatR
    var assembly = Assembly.GetExecutingAssembly();
    builder.Services.AddMediatR
        (
            cfg =>
            {
                cfg.RegisterServicesFromAssemblies
                (
                    assembly,
                    typeof(BaseCommand).Assembly,
                    typeof(BaseCommandHandler<>).Assembly
                );

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            }
        );
    #endregion
    #endregion

    var app = builder.Build();

    app.MapDefaultEndpoints();
    app.UseCors("AllowReactApp");

    #region Pipelines
    #region Scalar
    if (app.Environment.IsDevelopment())
    {
        // Redirect from http://localhost:5120 to http://localhost:5120/scalar/v1
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/")
            {
                context.Response.Redirect("/scalar/v1");
                return;
            }

            await next();
        });

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

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
        // Redirect from http://localhost:5120 to http://localhost:5120/scalar/v1
        app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger");
                    return;
                }
                await next();
            });

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

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

    #region Hub
    app.MapHub<MessageHub>("/chat");
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