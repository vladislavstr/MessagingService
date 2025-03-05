using Domain.Dtos;
using MediatR;

namespace Application.Events
{
    public class MessageCreatedEvent : INotification
    {
        public MessageDto Message { get; }

        public MessageCreatedEvent(MessageDto message)
        {
            Message = message;
        }
    }
}
