using Application.Providers.Interfaces;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Services;
using Npgsql;

namespace Application.Providers
{
    public class MessageProvider(IDatabaseService databaseService) : IMessageProvider
    {
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
                                new NpgsqlParameter("SentAt", sentAt)
                           };

            return await databaseService.ExecuteWithReturnAsync(Queries.SaveMessage, reader => new MessageEntity
            {
                Id = reader.GetInt32(0),
                Content = reader.GetString(1),
                SavedAt = new DateTimeOffset(reader.GetDateTime(2))
            }, parameters);
        }

        /// <summary>
        /// The list of messages from the last 10 minutes
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MessageEntity>> GetMessagesAsync()
        {
            return await databaseService.GetData(Queries.GetMessages, reader => new MessageEntity
            {
                Id = reader.GetInt32(0),
                Content = reader.GetString(1),
                SavedAt = new DateTimeOffset(reader.GetDateTime(2))
            });
        }
    }
}
