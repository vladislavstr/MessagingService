using Application.Providers.Interfaces;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Services;
using Npgsql;
using Serilog;

namespace Application.Providers
{
    public class DataBaseProvider(IDatabaseService databaseService) : IDataBaseProvider
    {
        private readonly ILogger _logger = Log.ForContext<DataBaseProvider>();

        /// <summary>
        /// Save message 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sentAt"></param>
        /// <returns></returns>
        public async Task<MessageEntity> SaveMessageAsync(string content, DateTimeOffset sentAt)
        {
            var parameters = new NpgsqlParameter[]
                           {
                                new NpgsqlParameter("Content", content),
                                new NpgsqlParameter("SentAtt", sentAt)
                           };

            return await databaseService.ExecuteWithReturnAsync(CmdText.SaveMessage, reader => new MessageEntity
            {
                Id = reader.GetInt32(0),
                Content = reader.GetString(1),
                SavedAt = reader.GetDateTime(2)
            }, parameters);

        }

        /// <summary>
        /// The list of messages from the last 10 minutes
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MessageEntity>> GetMessagesAsync()
        {

            return await databaseService.GetData(CmdText.GetMessages, reader => new MessageEntity
            {
                Id = reader.GetInt32(0),
                Content = reader.GetString(1),
                SavedAt = reader.GetDateTime(2)
            });

        }
    }
}
