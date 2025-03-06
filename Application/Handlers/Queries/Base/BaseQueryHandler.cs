using Domain.Models.Handlers.Queries.Base;
using MediatR;

namespace Application.Handlers.Queries.Base
{
    public abstract class BaseQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : BaseQuery<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
    }
}
