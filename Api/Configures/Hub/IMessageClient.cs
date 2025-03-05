using Domain.Dtos;

namespace Api.Configures.Hub
{
    public interface IMessageClient
    {
        Task Send(MessageDto message);
    }
}
