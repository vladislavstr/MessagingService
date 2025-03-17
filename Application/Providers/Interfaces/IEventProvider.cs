using Domain.Dtos;

namespace Application.Providers.Interfaces
{
    public interface IEventProvider
    {
        Task SendMessage(MessageDto message);
    }
}
