using Audacia.Commands;
using Blab.Domain.Entities.Security;

namespace Blab.Services.Services.UserChats.DeleteUserChat;

public class DeleteUserChatCommand : ICommand
{
    /// <summary>
    /// Gets the Id of the <see cref="UserChats"/> to be deleted.
    /// </summary>
    public int ChatId { get; }

    /// <summary>
    /// Gets the Id of the logged in <see cref="ApplicationUser"/>.
    /// </summary>
    public int LoggedInUserId { get; }

    public DeleteUserChatCommand(int chatId, int loggedInUserId)
    {
        ChatId = chatId;
        LoggedInUserId = loggedInUserId;
    }
}
