using Application.Handlers.Commands.Base;
using Application.Providers.Interfaces;
using Domain.Mappers.Message;
using Domain.Models.Handlers.Commands.Message;
using Serilog;

namespace Application.Handlers.Commands
{
    public class CreateMessageCommandHandler
        (
            IMessageProvider messageProvider,
            IMessageMapper messageMapper
        ) : BaseCommandHandler<CreateMessageCommand, string>
    {
        private readonly ILogger _logger = Log.ForContext<CreateMessageCommandHandler>();

        public override Task<string> Handle(CreateMessageCommand request, CancellationToken cancellationToken = default)
        {
            _logger.Information("The message has been created: {Request}", request);
            var message = messageMapper.ToModel(request);
            messageProvider.AddMessage(message);

            return Task.FromResult($"The message is recorded: {message.Id}");
        }
    }
}
