using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace Infrastructure.Contexts
{
    public class MessageContext
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public MessageContext(IConfiguration configuration, ILogger logger)
        {
            _logger = Log.ForContext<MessageContext>();
            _connectionString = configuration.GetSection("ConnectionStrings:PG").Value;
        }

        public void InitializeDatabase()
        {
            using var connection = new NpgsqlConnection(connectionString: _connectionString);
            connection.Open();

            var command = new NpgsqlCommand(CmdText.CreateTable, connection);

            command.ExecuteNonQuery();
        }
    }
}
