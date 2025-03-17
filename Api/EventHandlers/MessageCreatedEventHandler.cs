using Api.Configures.Hub;
using Application.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Api.EventHandlers
{
    public class MessageCreatedEventHandler(IHubContext<MessageHub, IMessageClient> hubContext) : INotificationHandler<MessageCreatedEvent>
    {
        public async Task Handle(MessageCreatedEvent notification, CancellationToken cancellationToken)
        {
            await hubContext.Clients.All.Send(notification.Message);
        }
    }
}