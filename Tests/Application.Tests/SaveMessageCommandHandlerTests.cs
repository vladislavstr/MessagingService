using Application.Handlers.Commands;
using Application.Providers.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Mappers.Message;
using Domain.Models.Handlers.Commands.Message;
using Moq;

namespace Application.Tests
{
    public class SaveMessageCommandHandlerTests
    {
        private readonly Mock<IMessageProvider> _messageProviderMock;
        private readonly Mock<IMessageMapper> _messageMapperMock;
        private readonly Mock<IDataBaseProvider> _dataBaseProviderMock;
        private readonly SaveAndSendMessageCommandHandler _handler;

        public SaveMessageCommandHandlerTests()
        {
            _messageProviderMock = new Mock<IMessageProvider>();
            _messageMapperMock = new Mock<IMessageMapper>();
            _dataBaseProviderMock = new Mock<IDataBaseProvider>();
            _handler = new SaveAndSendMessageCommandHandler(_messageProviderMock.Object, _messageMapperMock.Object, _dataBaseProviderMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessMessage()
        {
            // Arrange
            var request = new SaveAndSendMessageCommand { Content = "Test Content", SentAt = DateTime.UtcNow };
            var messageEntity = new MessageEntity { Id = 1, Content = request.Content, SavedAt = DateTime.UtcNow };
            var messageDto = new MessageDto { Id = messageEntity.Id, Content = messageEntity.Content, SavedAt = messageEntity.SavedAt };

            _dataBaseProviderMock
                .Setup(db => db.SaveMessageAsync(request.Content, request.SentAt))
                .ReturnsAsync(messageEntity);

            _messageMapperMock
                .Setup(mapper => mapper.ToDto(messageEntity))
                .Returns(messageDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal($"The message has been sent with number: {messageEntity.Id}", result);
            _messageProviderMock.Verify(provider => provider.AddMessage(messageDto), Times.Once);
        }

        [Fact]
        public async Task Handle_DatabaseThrowsException_ReturnsErrorMessage()
        {
            // Arrange
            var request = new SaveAndSendMessageCommand { Content = "Test Content", SentAt = DateTime.UtcNow };

            _dataBaseProviderMock
                .Setup(db => db.SaveMessageAsync(request.Content, request.SentAt))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.Equal("Something wrong.", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidRequest_CallsToDtoWithCorrectArguments()
        {
            // Arrange
            var request = new SaveAndSendMessageCommand { Content = "Test Content", SentAt = DateTime.UtcNow };
            var messageEntity = new MessageEntity { Id = 1, Content = request.Content, SavedAt = DateTime.UtcNow };
            var messageDto = new MessageDto { Id = messageEntity.Id, Content = messageEntity.Content, SavedAt = messageEntity.SavedAt };

            _dataBaseProviderMock
                .Setup(db => db.SaveMessageAsync(request.Content, request.SentAt))
                .ReturnsAsync(messageEntity);

            _messageMapperMock
                .Setup(mapper => mapper.ToDto(messageEntity))
                .Returns(messageDto);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _messageMapperMock.Verify(
                mapper => mapper.ToDto(It.Is<MessageEntity>(x => x.Id == messageEntity.Id && x.Content == messageEntity.Content && x.SavedAt == messageEntity.SavedAt)),
                Times.Once);
        }
    }
}