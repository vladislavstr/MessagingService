using Domain.Models.Handlers.Commands.Base;
using MediatR;

namespace Application.Handlers.Commands.Base
{
    public abstract class BaseCommandHandler<T> : IRequestHandler<T> where T : BaseCommand
    {
        public abstract Task Handle(T request, CancellationToken cancellationToken = default);
    }

    public abstract class BaseCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : BaseCommand<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
    }
}
