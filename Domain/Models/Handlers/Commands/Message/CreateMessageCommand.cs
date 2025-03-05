using Domain.Models.Handlers.Commands.Base;

namespace Domain.Models.Handlers.Commands.Message
{
    public sealed record CreateMessageCommand : BaseCommand<string>
    {
        public string Content { get; init; }
        public DateTimeOffset SentAt { get; init; } = DateTimeOffset.UtcNow;
    }
}