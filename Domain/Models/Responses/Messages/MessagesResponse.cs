using Domain.Dtos;

namespace Domain.Models.Responses.Models
{
    public sealed record MessagesResponse
    (
        IEnumerable<MessageDto> Messages
    );
}
