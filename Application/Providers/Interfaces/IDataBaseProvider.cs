using Domain.Entities;
using Npgsql;

namespace Application.Providers.Interfaces
{
    public interface IDataBaseProvider
    {
        Task<MessageEntity> ExecuteNonQueryAsync(string sql, params NpgsqlParameter[] parameters);

        Task<IEnumerable<MessageEntity>> GetMessagesAsync();
    }
}
