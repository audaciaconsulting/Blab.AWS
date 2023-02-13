using Blab.Services.Services.Common;

namespace Blab.Services.Services.UserChats.DeleteUserChat;

public interface IDeleteUserChatService : ICommandHandler<DeleteUserChatCommand, bool>
{
}
