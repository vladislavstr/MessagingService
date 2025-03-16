using Application.Providers;
using Application.Providers.Interfaces;

namespace Api.Configures
{
    public static class ApplicationConfigure
    {
        public static IServiceCollection AddApplicationConfigure(this IServiceCollection services)
        {
            services.AddSingleton<IMessageProvider, MessageProvider>();
            services.AddSingleton<IDataBaseProvider, DataBaseProvider>();

            return services;
        }
    }
}
