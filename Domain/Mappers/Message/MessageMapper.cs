using Domain.Entities;
using Domain.Models;
using Domain.Models.Handlers.Commands.Message;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers.Message
{
    public interface IMessageMapper
    {
        //MessageModel ToModel(CreateMessageCommand command);
        //MessageModel ToModel(MessageEntity entity);
        //MessageEntity ToEntity(MessageModel model);
    }

    [Mapper]
    public partial class MessageMapper : IMessageMapper
    {
        //[MapProperty(nameof(CreateMessageCommand.Id), nameof(MessageModel.Id))]
        //public partial MessageModel ToModel(CreateMessageCommand command);

        //[MapProperty(nameof(MessageEntity.Id), nameof(MessageModel.Id))]
        //public partial MessageModel ToModel(MessageEntity entity);

        //[MapperIgnoreTarget(nameof(MessageEntity.Id))]
        //[MapProperty(nameof(MessageModel.Id), nameof(MessageEntity.Id))]
        //public partial MessageEntity ToEntity(MessageModel model);
    }
}
