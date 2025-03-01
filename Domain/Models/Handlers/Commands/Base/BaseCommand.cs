using MediatR;

namespace Domain.Models.Handlers.Commands.Base
{
    public abstract record BaseCommand : IRequest;
    public abstract record BaseCommand<TResponse> : IRequest<TResponse>;
}
