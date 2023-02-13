using Audacia.Commands;

namespace Blab.Services.Services.Messages.UpdateReadDateTimeOfMessages;
public class UpdateReadDateTimeOfMessagesCommand : ICommand
{
    /// <summary>
    /// Gets or sets the ChatId of the Chat that the messages are being read in.
    /// </summary>
    public int ChatId { get; set; }

    /// <summary>
    /// UserId of the logged in user who is reading the chat. T
    /// The messages that have a UserId that is not the same as the loggedInUserId will have their message ReadDateTime updated.
    /// </summary>
    public int LoggedInUserId { get; set; }

    public UpdateReadDateTimeOfMessagesCommand(int chatId, int loggedInUserId)
    {
        ChatId = chatId;
        LoggedInUserId = loggedInUserId;
    }
}
