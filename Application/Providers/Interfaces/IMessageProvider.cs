using Domain.Models;

namespace Application.Providers.Interfaces
{
    public interface IMessageProvider
    {
        int AddMessage(MessageModel patient, bool isContinuation = false);
    }
}
