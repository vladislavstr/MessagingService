using Domain.Models.Handlers.Commands.Base;

namespace Domain.Models.Handlers.Commands.Message
{
    public sealed record CreateMessageCommand
    (
        int Id,
        string Content,
        DateTime SavedAt,
        DateTimeOffset SentAt
    ) : BaseCommand<string>;
}
