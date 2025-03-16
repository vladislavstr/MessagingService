using Domain.Dtos;

namespace Application.Providers.Interfaces
{
    public interface IMessageProvider
    {
        Task AddMessage(MessageDto message);
    }
}
