using Domain.Dtos;
using MediatR;

namespace Application.Events
{
    public class MessageSavedEvent : INotification
    {
        public MessageDto Message { get; }

        public MessageSavedEvent(MessageDto message)
        {
            Message = message;
        }
    }
}
