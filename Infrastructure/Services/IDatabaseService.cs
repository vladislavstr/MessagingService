using Npgsql;

namespace Infrastructure.Services
{
    public interface IDatabaseService
    {
        Task<T> ExecuteWithReturnAsync<T>(string sql, Func<NpgsqlDataReader, T> mapper, params NpgsqlParameter[] parameters);

        Task<IEnumerable<T>> GetData<T>(string sql, Func<NpgsqlDataReader, T> mapper, params NpgsqlParameter[] parameters);
    }
}
