using Application.Providers.Interfaces;
using Domain.Mappers.Message;
using Domain.Models;
using Serilog;

namespace Application.Providers
{
    public class MessageProvider(IMessageMapper messageMapper) : IMessageProvider
    {
        private readonly ILogger _logger = Log.ForContext<MessageProvider>();

        public int AddMessage(MessageModel patient, bool isContinuation = false)
        {
            throw new NotImplementedException();
        }
    }
}
