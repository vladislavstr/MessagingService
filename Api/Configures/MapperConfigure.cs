using Domain.Mappers.Message;

namespace Api.Configures
{
    public static class MapperConfigure
    {
        public static IServiceCollection AddMapperConfigure(this IServiceCollection services)
        {
            services.AddSingleton<IMessageMapper, MessageMapper>();

            return services;
        }
    }
}
