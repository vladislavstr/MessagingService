using Application.Events;
using Application.Providers.Interfaces;
using Domain.Dtos;
using MediatR;
using Serilog;

namespace Application.Providers
{
    public class MessageProvider(IMediator mediator) : IMessageProvider
    {
        private readonly ILogger _logger = Log.ForContext<MessageProvider>();
        public async Task AddMessage(MessageDto message, bool isContinuation = false)
        {
            try
            {
                await mediator.Publish(new MessageCreatedEvent(message));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }
    }
}
