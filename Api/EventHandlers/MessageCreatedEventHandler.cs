using Api.Configures.Hub;
using Application.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Api.EventHandlers
{
    public class MessageCreatedEventHandler : INotificationHandler<MessageCreatedEvent>
    {
        #region DI
        private readonly IHubContext<MessageHub, IMessageClient> _hubContext;

        public MessageCreatedEventHandler(IHubContext<MessageHub, IMessageClient> hubContext)
        {
            _hubContext = hubContext;
        }
        #endregion

        public async Task Handle(MessageCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.Send(notification.Message);
        }
    }
}