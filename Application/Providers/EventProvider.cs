using Application.Events;
using Application.Providers.Interfaces;
using Domain.Dtos;
using MediatR;
using Serilog;

namespace Application.Providers
{
    public class EventProvider(IMediator mediator) : IEventProvider
    {
        private readonly ILogger _logger = Log.ForContext<EventProvider>();
        public async Task SendMessage(MessageDto message)
        {
            try
            {
                await mediator.Publish(new MessageSavedEvent(message));
                _logger.Information("Sent message with id: {@Id}", message.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }
    }
}
