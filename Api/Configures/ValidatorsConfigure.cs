using Domain.Models.Handlers.Commands.Message;
using Domain.Models.Handlers.Commands.Message.Validators;
using FluentValidation;

namespace Api.Configures
{
    public static class ValidatorsConfigure
    {
        public static IServiceCollection AddValidatorsConfigure(this IServiceCollection services)
        {
            services.AddScoped<IValidator<SaveAndSendMessageCommand>, SaveAndSendMessageCommandValidator>();

            return services;
        }
    }
}
