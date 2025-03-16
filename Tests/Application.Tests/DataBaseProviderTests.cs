using Application.Providers;
using Domain.Entities;
using Infrastructure.Services;
using Moq;
using Npgsql;

namespace Application.Tests.Providers
{
    public class DataBaseProviderTests
    {
        private readonly Mock<IDatabaseService> _mockDatabaseService;
        private readonly DataBaseProvider _provider;

        public DataBaseProviderTests()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            _provider = new DataBaseProvider(_mockDatabaseService.Object);
        }

        [Fact]
        public async Task SaveMessageAsync_ValidData_ReturnsMessageEntity()
        {
            // Arrange
            var content = "Test message";
            var sentAt = DateTimeOffset.UtcNow;

            var expectedMessage = new MessageEntity
            {
                Id = 1,
                Content = content,
                SavedAt = sentAt.DateTime
            };

            _mockDatabaseService
                .Setup(service => service.ExecuteWithReturnAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<NpgsqlDataReader, MessageEntity>>(),
                    It.IsAny<NpgsqlParameter[]>()))
                .ReturnsAsync(expectedMessage);

            // Act
            var result = await _provider.SaveMessageAsync(content, sentAt);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMessage.Id, result.Id);
            Assert.Equal(expectedMessage.Content, result.Content);
            Assert.Equal(expectedMessage.SavedAt, result.SavedAt);
        }

        [Fact]
        public async Task SaveMessageAsync_InvalidData_ThrowsException()
        {
            // Arrange
            var content = "Test message";
            var sentAt = DateTimeOffset.UtcNow;

            _mockDatabaseService
                .Setup(service => service.ExecuteWithReturnAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<NpgsqlDataReader, MessageEntity>>(),
                    It.IsAny<NpgsqlParameter[]>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _provider.SaveMessageAsync(content, sentAt));
        }

        [Fact]
        public async Task GetMessagesAsync_ValidData_ReturnsMessages()
        {
            // Arrange
            var expectedMessages = new List<MessageEntity>
            {
                new MessageEntity { Id = 1, Content = "Message 1", SavedAt = DateTime.UtcNow },
                new MessageEntity { Id = 2, Content = "Message 2", SavedAt = DateTime.UtcNow }
            };

            _mockDatabaseService
                .Setup(service => service.GetData(
                    It.IsAny<string>(),
                    It.IsAny<Func<NpgsqlDataReader, MessageEntity>>(),
                    It.IsAny<NpgsqlParameter[]>()))
                .ReturnsAsync(expectedMessages);

            // Act
            var result = await _provider.GetMessagesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMessages.Count, result.Count());
            Assert.Equal(expectedMessages[0].Id, result.First().Id);
            Assert.Equal(expectedMessages[1].Content, result.Last().Content);
        }

        [Fact]
        public async Task GetMessagesAsync_InvalidData_ThrowsException()
        {
            // Arrange
            _mockDatabaseService
                .Setup(service => service.GetData(
                    It.IsAny<string>(),
                    It.IsAny<Func<NpgsqlDataReader, MessageEntity>>(),
                    It.IsAny<NpgsqlParameter[]>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _provider.GetMessagesAsync());
        }
    }
}