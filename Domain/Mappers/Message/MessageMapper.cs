using Domain.Dtos;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers.Message
{
    public interface IMessageMapper
    {
        MessageDto ToDto(MessageEntity entity);

        IEnumerable<MessageDto> ToDto(IEnumerable<MessageEntity> entity);
    }

    [Mapper]
    public partial class MessageMapper : IMessageMapper
    {
        public partial MessageDto ToDto(MessageEntity entity);

        public partial IEnumerable<MessageDto> ToDto(IEnumerable<MessageEntity> entity);
    }
}