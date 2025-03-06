using Domain.Models.Handlers.Queries.Base;
using Domain.Models.Responses.Models;

namespace Domain.Models.Handlers.Queries.Message
{
    public sealed record GetMessageQuery : BaseQuery<MessagesResponse>;
}
