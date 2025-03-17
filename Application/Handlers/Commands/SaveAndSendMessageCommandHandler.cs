using Application.Handlers.Commands.Base;
using Application.Providers.Interfaces;
using Domain.Entities;
using Domain.Mappers.Message;
using Domain.Models.Handlers.Commands.Message;
using Serilog;

namespace Application.Handlers.Commands
{
    public class SaveAndSendMessageCommandHandler
        (
            IEventProvider messageProvider,
            IMessageMapper messageMapper,
            IMessageProvider dataBaseProvider
        ) : BaseCommandHandler<SaveAndSendMessageCommand, string>
    {
        private readonly ILogger _logger = Log.ForContext<SaveAndSendMessageCommandHandler>();

        public override async Task<string> Handle(SaveAndSendMessageCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Information("Initial of save message processing: {@Request}", request);

                MessageEntity message = await dataBaseProvider.SaveMessageAsync(request.Content, request.SentAt);

                await messageProvider.SendMessage(messageMapper.ToDto(message));

                return $"The message has been sent with number: {message.Id}";
            }
            catch (Exception ex)
            {
                throw new Exception("Something wrong.");
            }
        }
    }
}
