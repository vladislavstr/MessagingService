using Application.Handlers.Queries.Base;
using Application.Providers.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Mappers.Message;
using Domain.Models.Handlers.Queries.Message;
using Domain.Models.Responses.Models;
using Serilog;

namespace Application.Handlers.Queries.Message
{
    public class GetMessagesQueryHandler
        (
            IMessageMapper messageMapper,
            IDataBaseProvider dataBaseProvider
        ) : BaseQueryHandler<GetMessageQuery, MessagesResponse>
    {
        private readonly ILogger _logger = Log.ForContext<GetMessagesQueryHandler>();

        public override async Task<MessagesResponse> Handle(GetMessageQuery request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Information("Initial of get messages processing: {@request}", request);

                IEnumerable<MessageEntity> messages = await dataBaseProvider.GetMessagesAsync();
                IEnumerable<MessageDto> result = messageMapper.ToDto(messages);

                return new MessagesResponse(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Something wrong.");
            }
        }
    }
}