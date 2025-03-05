using Domain.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace Api.Configures.Hub
{
    public class MessageHub : Hub<IMessageClient>
    {
        /// <summary>
        /// Accepting the message and forwarding to all other clients
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendMessageToChat(MessageDto message)
        {
            return Clients.Others.Send(message);
        }
    }
}
