using MediatR;

namespace Domain.Models.Handlers.Queries.Base
{
    public abstract record BaseQuery<T> : IRequest<T>;
}
