﻿using Application.Providers;
using Application.Providers.Interfaces;

namespace Api.Configures
{
    public static class ApplicationConfigure
    {
        public static IServiceCollection AddApplicationConfigure(this IServiceCollection services)
        {
            services.AddScoped<IEventProvider, EventProvider>();
            services.AddScoped<IMessageProvider, MessageProvider>();

            return services;
        }
    }
}
