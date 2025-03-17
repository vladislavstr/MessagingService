using Application.Handlers.Queries.Message;
using Application.Providers.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Mappers.Message;
using Domain.Models.Handlers.Queries.Message;
using Moq;

namespace Application.Tests
{
    public class GetMessagesQueryHandlerTest
    {
        private readonly Mock<IMessageMapper> _messageMapperMock;
        private readonly Mock<IMessageProvider> _providerMock;
        private readonly GetMessagesQueryHandler _handler;

        public GetMessagesQueryHandlerTest()
        {
            _messageMapperMock = new Mock<IMessageMapper>();
            _providerMock = new Mock<IMessageProvider>();
            _handler = new GetMessagesQueryHandler(_messageMapperMock.Object, _providerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsMessagesResponse()
        {
            // Arrange
            var request = new GetMessageQuery();
            var messages = new List<MessageEntity>
            {
                new MessageEntity { Id = 1, Content = "Message 1", SavedAt = DateTime.UtcNow },
                new MessageEntity { Id = 2, Content = "Message 2", SavedAt = DateTime.UtcNow }
            };
            var messageDtos = new List<MessageDto>
            {
                new MessageDto { Id = messages[0].Id, Content = messages[0].Content, SavedAt = messages[0].SavedAt },
                new MessageDto { Id = messages[1].Id, Content = messages[1].Content, SavedAt = messages[1].SavedAt }
            };

            _providerMock
                .Setup(db => db.GetMessagesAsync())
                .ReturnsAsync(messages);

            _messageMapperMock
                .Setup(mapper => mapper.ToDto(It.IsAny<IEnumerable<MessageEntity>>()))
                .Returns(messageDtos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(messageDtos, result.Messages);
        }

        [Fact]
        public async Task Handle_DatabaseThrowsException_ThrowsExceptionWithCorrectMessage()
        {
            // Arrange
            var request = new GetMessageQuery();

            _providerMock
                .Setup(db => db.GetMessagesAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Something wrong.", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidRequest_CallsToDtoWithCorrectArguments()
        {
            // Arrange
            var request = new GetMessageQuery();
            var messages = new List<MessageEntity>
            {
                new MessageEntity { Id = 1, Content = "Message 1", SavedAt = DateTime.UtcNow },
                new MessageEntity { Id = 2, Content = "Message 2", SavedAt = DateTime.UtcNow }
            };
            var messageDtos = new List<MessageDto>
            {
                new MessageDto { Id = messages[0].Id, Content = messages[0].Content, SavedAt = messages[0].SavedAt },
                new MessageDto { Id = messages[1].Id, Content = messages[1].Content, SavedAt = messages[1].SavedAt }
            };

            _providerMock
                .Setup(db => db.GetMessagesAsync())
                .ReturnsAsync(messages);

            _messageMapperMock
                .Setup(mapper => mapper.ToDto(It.IsAny<IEnumerable<MessageEntity>>()))
                .Returns(messageDtos);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _messageMapperMock.Verify(
                mapper => mapper.ToDto(It.Is<IEnumerable<MessageEntity>>(x => x.SequenceEqual(messages))),
                Times.Once);
        }
    }
}