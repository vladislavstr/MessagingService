using Domain.Models;

namespace Application.Providers.Interfaces
{
    public interface IMessageProvider
    {
        int AddMessage(MessageModel message, bool isContinuation = false);
    }
}
