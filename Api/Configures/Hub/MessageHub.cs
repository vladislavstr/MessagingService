using Domain.Dtos;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace Api.Configures.Hub
{
    public class MessageHub : Hub<IMessageClient>
    {
        private readonly Serilog.ILogger _logger = Log.ForContext<MessageHub>();

        /// <summary>
        /// Accepting the message and forwarding to all other clients
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendMessageToChat(MessageDto message)
        {
            _logger.Information("Sent message with id: {@Id}", message.Id);
            return Clients.Others.Send(message);
        }
    }
}
