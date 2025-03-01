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
    }
}
