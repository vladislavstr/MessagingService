using Application.Providers;
using Application.Providers.Interfaces;

namespace Api.Configures
{
    public static class InfrastructureConfigure
    {
        public static IServiceCollection AddInfrastructureConfigure(this IServiceCollection services)
        {
            services.AddSingleton<IMessageProvider, MessageProvider>();
            services.AddSingleton<IDataBaseProvider, DataBaseProvider>();

            return services;
        }
    }
}
