﻿using Infrastructure.Contexts;
using Infrastructure.Services;
using Serilog;

namespace Api.Configures
{
    public static class InfrastructureConfigure
    {
        public static WebApplicationBuilder AddInfrastructureConfigure(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetSection("ConnectionStrings:PG").TestConfiguration();
            builder.Services.AddScoped(_ => new MessageContext(connectionString));
            builder.Services.InitializeDatabase(connectionString);

            builder.Services.AddScoped<IDatabaseService, DatabaseService>();

            return builder;
        }

        private static void InitializeDatabase(this IServiceCollection services, string connectionString)
        {
            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MessageContext>();
                context.InitializeDatabaseAsync().GetAwaiter().GetResult();
            }
        }

        private static string TestConfiguration<T>(this T section) where T : IConfigurationSection
        {
            if (section.Value is null)
            {
                Log.Logger.Information("Shutdown is complete");
                throw new Exception("Fail in appsettings");
            }
            return section.Value;
        }
    }
}
