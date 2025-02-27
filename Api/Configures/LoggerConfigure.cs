using Serilog;

namespace Api.Configures
{
    public static class LoggerConfigure
    {
        public static Serilog.ILogger AddLogger
            (this WebApplicationBuilder webApplicationBuilder,
            IConfiguration configuration)
        {
            webApplicationBuilder.Logging.ClearProviders();

            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
