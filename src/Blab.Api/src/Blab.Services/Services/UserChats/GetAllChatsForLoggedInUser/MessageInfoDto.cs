using System.Linq.Expressions;
using Blab.Domain.Entities.Models;

namespace Blab.Services.Services.UserChats.GetAllChatsForLoggedInUser;

/// <summary>
/// This Message info dto is the information returned about the latest message in the chat.
/// These properties can be null becasue a new chat will have no message in it.
/// </summary>
public class MessageInfoDto
{
    /// <summary>
    /// Gets or sets the message id of the latest message sent in the chat.
    /// </summary>
    public int? MessageId { get; set; }

    /// <summary>
    /// Gets or sets the content of the latest message in the chat so that it can be displayed in the chat preview panel.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets the userId of the sender of the latest message.
    /// This cna help change the styling of the css of the latest message in the chat preview to give a better indication of who sent the last message.
    /// </summary>
    public int? UserIdOfMessageSender { get; set; }

    /// <summary>
    /// Gets or sets the time stamp of the latest message sent in the chat so that it can be displayed.
    /// </summary>
    public DateTime? TimeStampOfLatestMessage { get; set; }

    /// <summary>
    /// Gets or sets a bool value to whether the latest message has been read. 
    /// This css differs to indicate to the logged in user whether they have an unread message in that chat or not.
    /// </summary>
    public bool? HasMessageBeenRead { get; set; }

    /// <summary>
    /// This expression converts a Message into this MessageInfoDto so that the needed properties are returned.
    /// </summary>
    public static Expression<Func<Message, MessageInfoDto>> FromMessage { get; } = message => new MessageInfoDto()
    {
        Content = message.Content,
        MessageId = message.Id,
        UserIdOfMessageSender = message.UserId,
        TimeStampOfLatestMessage = message.Sent,
        HasMessageBeenRead = message.ReadDateTime != null
    };

}
