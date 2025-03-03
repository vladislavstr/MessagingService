using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Contexts
{
    public class MessageContext
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public MessageContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("ConnectionStrings:PG").Value;
        }

        public static void InitializeDatabase(string connectionString)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var command = new NpgsqlCommand(@"
                                                CREATE TABLE IF NOT EXISTS messages (
                                                Id SERIAL PRIMARY KEY,
                                                Content VARCHAR(128) NOT NULL,
                                                SavedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                                SentAt TIMESTAMP WITH TIME ZONE NOT NULL);
                                             ", connection);

            command.ExecuteNonQuery();
        }
    }
}
