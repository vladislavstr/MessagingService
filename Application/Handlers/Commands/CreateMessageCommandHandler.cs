using Application.Handlers.Commands.Base;
using Application.Providers.Interfaces;
using Domain.Entities;
using Domain.Mappers.Message;
using Domain.Models.Handlers.Commands.Message;
using Npgsql;
using Serilog;

namespace Application.Handlers.Commands
{
    public class CreateMessageCommandHandler
        (
            IMessageProvider messageProvider,
            IMessageMapper messageMapper,
            IDataBaseProvider dataBaseProvider
        ) : BaseCommandHandler<CreateMessageCommand, string>
    {
        private readonly ILogger _logger = Log.ForContext<CreateMessageCommandHandler>();

        public override async Task<string> Handle(CreateMessageCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Information("Initial of message processing: {Request}", request);

                MessageEntity message = await dataBaseProvider.ExecuteNonQueryAsync(
                   "INSERT INTO messages (content, sentat ) VALUES (@content  ,@sentat) RETURNING id, content, savedat",
                   new NpgsqlParameter("content", request.Content),
                   new NpgsqlParameter("sentat", request.SentAt));

                await messageProvider.AddMessage(messageMapper.ToDto(message));

                return await Task.FromResult($"The message has been sent with number: {message.Id}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return ("Something wrong.");
            }
        }
    }
}
