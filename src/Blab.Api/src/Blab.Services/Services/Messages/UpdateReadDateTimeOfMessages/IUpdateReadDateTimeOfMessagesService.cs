using Blab.Services.Services.Common;

namespace Blab.Services.Services.Messages.UpdateReadDateTimeOfMessages;
public interface IUpdateReadDateTimeOfMessagesService : ICommandHandler<UpdateReadDateTimeOfMessagesCommand, IEnumerable<ReadMessagesDto>>
{
}
