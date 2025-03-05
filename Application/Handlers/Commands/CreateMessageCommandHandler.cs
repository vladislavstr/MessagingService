using Application.Handlers.Commands.Base;
using Application.Providers.Interfaces;
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
            _logger.Information("The message has been created: {Request}", request);

            int messageNumber = await dataBaseProvider.ExecuteNonQueryAsync(
               "INSERT INTO messages (content, sentat ) VALUES (@content  ,@sentat) RETURNING id",
               new NpgsqlParameter("content", request.Content),
               new NpgsqlParameter("sentat", request.SentAt));

            return await Task.FromResult($"The message is recorded: {messageNumber}");
        }
    }
}
