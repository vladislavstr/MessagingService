using Domain.Models.Handlers.Commands.Base;
using Domain.Models.Responses.Models;

namespace Domain.Models.Handlers.Commands.Message
{
    public sealed record SaveAndSendMessageCommand : BaseCommand<string>
    {
        public string Content { get; init; }
        public DateTimeOffset SentAt { get; init; } = DateTimeOffset.UtcNow;
    }
}