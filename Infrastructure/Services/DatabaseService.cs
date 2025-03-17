using Infrastructure.Contexts;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace Infrastructure.Services
{
    public class DatabaseService(IConfiguration configuration, MessageContext context) : IDatabaseService
    {
        private readonly ILogger _logger = Log.ForContext<DatabaseService>();

        /// <summary>
        /// Save data to database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="mapper"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<T> ExecuteWithReturnAsync<T>(string sql, Func<NpgsqlDataReader, T> mapper, params NpgsqlParameter[] parameters)
        {
            _logger.Information("Start {@Method} with query: {@Sql}", nameof(ExecuteWithReturnAsync), sql);
            await using var connection = await context.GetConnectionAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                return mapper(reader);
            }

            _logger.Warning("Can not return data for query: {@Sql}", sql);
            throw new InvalidOperationException("Invalid operation");
        }

        /// <summary>
        /// Get data from database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="mapper"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetData<T>(string sql, Func<NpgsqlDataReader, T> mapper, params NpgsqlParameter[] parameters)
        {
            _logger.Information("Start {@Method} with query: {@Sql}", nameof(GetData), sql);
            await using var connection = await context.GetConnectionAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);

            var results = new List<T>();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(mapper(reader));
            }

            if (results.Count == 0)
            {
                _logger.Warning("No data found for query: {@Sql}", sql);
            }

            return results;
        }
    }
}
