using Application.Providers.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace Application.Providers
{
    public class DataBaseProvider(IConfiguration configuration) : IDataBaseProvider
    {
        private readonly ILogger _logger = Log.ForContext<DataBaseProvider>();
        private readonly string _connectionString = configuration.GetSection("ConnectionStrings:PG").Value;

        /// <summary>
        /// Go to db
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>Message value</returns>
        //ToDo: go to generic 
        public async Task<MessageEntity> ExecuteNonQueryAsync(string sql, params NpgsqlParameter[] parameters)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddRange(parameters);

            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new MessageEntity
                {
                    Id = reader.GetInt32(0),
                    Content = reader.GetString(1),
                    SavedAt = reader.GetDateTime(2)
                };
            }

            _logger.Error("Error saving the message with sql: {Sql}\n parameters: {Parameters}", sql, parameters);
            throw new Exception("Try again later");
        }

        /// <summary>
        /// The list of messages from the last 10 minutes
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MessageEntity>> GetMessagesAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var messages = new List<MessageEntity>();

            const string sql = "SELECT id, content, savedat FROM messages WHERE savedat >= NOW() - INTERVAL '10 minutes'";
            await using var command = new NpgsqlCommand(sql, connection);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                messages.Add(new MessageEntity
                {
                    Id = reader.GetInt32(0),
                    Content = reader.GetString(1),
                    SavedAt = reader.GetDateTime(2)
                });
            }

            _logger.Error("Error saving the message with sql: {Sql}", sql);
            return messages;
        }
    }
}
