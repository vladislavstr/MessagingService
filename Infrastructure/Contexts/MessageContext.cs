﻿using Npgsql;
using Serilog;

namespace Infrastructure.Contexts
{
    public class MessageContext(string connectionString) : IDisposable
    {
        private readonly ILogger _logger = Log.ForContext<MessageContext>();
        private NpgsqlConnection _connection = new NpgsqlConnection();
        
        /// <summary>
        /// Get connection to database
        /// </summary>
        /// <returns></returns>
        public async Task<NpgsqlConnection> GetConnectionAsync()
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                _connection = new NpgsqlConnection(connectionString);
                await _connection.OpenAsync();
                _logger.Debug("Database connection opened.");
            }
            else
                _logger.Warning("Using existing database connection.");
            
            return _connection;
        }

        /// <summary>
        /// Check connection with database and accessibility table of messages 
        /// </summary>
        /// <returns></returns>
        public async Task InitializeDatabaseAsync()
        {
            _logger.Information("Initializing database with connection string: {@ConnectionString}", connectionString);

            await CheckConnectionAsync();

            if (await TableExistsAsync("messages"))
                _logger.Information("Table 'messages' already exists.");

            else
                await CreateMessagesTableAsync();

            _logger.Information("Database initialized successfully.");
        }

        private async Task CheckConnectionAsync()
        {
            try
            {
                await using var connection = await GetConnectionAsync();
                _logger.Information("Database connection {@Connection} successful.", connection);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to connect to the database.");
                throw;
            }
        }

        private async Task<bool> TableExistsAsync(string tableName)
        {
            await using var connection = await GetConnectionAsync();
            await using var command = new NpgsqlCommand(Queries.CheckTableExist, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            var result = await command.ExecuteScalarAsync();
            return (bool)result!;
        }

        private async Task CreateMessagesTableAsync()
        {
            await using var connection = await GetConnectionAsync();
            await using var command = new NpgsqlCommand(Queries.CreateTable, connection);
            await command.ExecuteNonQueryAsync();

            _logger.Information("Table 'messages' created successfully.");
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
            _logger.Information("Database connection closed.");
        }
    }
}
