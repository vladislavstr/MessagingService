using Domain.Entities;
using Npgsql;

namespace Application.Providers.Interfaces
{
    public interface IDataBaseProvider
    {
        Task<MessageEntity> SaveMessageAsync(string content, DateTimeOffset sentAt);

        Task<IEnumerable<MessageEntity>> GetMessagesAsync();
    }
}
