using Domain.Dtos;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers.Message
{
    public interface IMessageMapper
    {
        MessageDto ToDto(MessageEntity entity);
    }

    [Mapper]
    public partial class MessageMapper : IMessageMapper
    {
        [MapProperty(nameof(MessageEntity.Id), nameof(MessageDto.Id))]
        public partial MessageDto ToDto(MessageEntity entity);
    }
}