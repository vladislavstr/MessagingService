using Application.Handlers.Commands;
using Application.Providers.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Mappers.Message;
using Domain.Models.Handlers.Commands.Message;
using Moq;
using Npgsql;

namespace Application.Tests
{
    public class CreateMessageCommandHandlerTests
    {
        private readonly Mock<IMessageProvider> _messageProviderMock;
        private readonly Mock<IMessageMapper> _messageMapperMock;
        private readonly Mock<IDataBaseProvider> _dataBaseProviderMock;
        private readonly CreateMessageCommandHandler _handler;

        public CreateMessageCommandHandlerTests()
        {
            _messageProviderMock = new Mock<IMessageProvider>();
            _messageMapperMock = new Mock<IMessageMapper>();
            _dataBaseProviderMock = new Mock<IDataBaseProvider>();
            _handler = new CreateMessageCommandHandler(_messageProviderMock.Object, _messageMapperMock.Object, _dataBaseProviderMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessMessage()
        {
            // Arrange
            var request = new CreateMessageCommand { Content = "Test Content", SentAt = DateTime.UtcNow };
            var messageEntity = new MessageEntity { Id = 1, Content = request.Content, SavedAt = DateTime.UtcNow };
            var messageDto = new MessageDto { Id = messageEntity.Id, Content = messageEntity.Content, SavedAt = messageEntity.SavedAt };

            _dataBaseProviderMock
                .Setup(db => db.ExecuteNonQueryAsync(It.IsAny<string>(), It.IsAny<NpgsqlParameter[]>()))
                .ReturnsAsync(messageEntity);


            _messageMapperMock
                .Setup(mapper => mapper.ToDto(It.IsAny<MessageEntity>()))
                .Returns(messageDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal($"The message has been sent with number: {messageEntity.Id}", result);
            _messageProviderMock.Verify(provider => provider.AddMessage(messageDto), Times.Once); 
        }
    }
}
