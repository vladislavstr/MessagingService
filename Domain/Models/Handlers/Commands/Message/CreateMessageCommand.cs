using Domain.Models.Handlers.Commands.Base;

namespace Domain.Models.Handlers.Commands.Message
{
    public sealed record CreateMessageCommand
    (
        string Content,
        DateTimeOffset SentAt
    ) : BaseCommand<string>;
}
