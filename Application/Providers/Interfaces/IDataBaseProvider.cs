using Npgsql;

namespace Application.Providers.Interfaces
{
    public interface IDataBaseProvider
    {
        Task<int> ExecuteNonQueryAsync(string sql, params NpgsqlParameter[] parameters);
    }
}
