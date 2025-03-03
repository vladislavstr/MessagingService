using Application.Providers.Interfaces;
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
        public async Task<int> ExecuteNonQueryAsync(string sql, params NpgsqlParameter[] parameters)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                await using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddRange(parameters);

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return 0;
            }
        }
    }
}
