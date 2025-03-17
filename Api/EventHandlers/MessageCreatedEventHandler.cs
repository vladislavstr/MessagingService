using Api.Configures.Hub;
using Application.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Api.EventHandlers
{
    public class MessageCreatedEventHandler(IHubContext<MessageHub, IMessageClient> hubContext) : INotificationHandler<MessageSavedEvent>
    {
        public async Task Handle(MessageSavedEvent notification, CancellationToken cancellationToken)
        {
            await hubContext.Clients.All.Send(notification.Message);
        }
    }
}