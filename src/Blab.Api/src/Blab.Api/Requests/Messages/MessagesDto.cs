using System.Linq.Expressions;
using Blab.Domain.Entities.Models;

namespace Blab.Api.Requests.Messages;

/// <summary>
/// DTO for returning a message or messages in a chat.
/// </summary>
public class MessagesDto
{
    /// <summary>
    /// Gets or sets the Id of the Message.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Content of the Message.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time sent of the message.
    /// </summary>
    public DateTime Sent { get; set; }

    /// <summary>
    /// Gets or sets the UserId of the user that send their message.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the ChatId of the chat the message is sent in. 
    /// </summary>
    public int ChatId { get; set; }

    /// <summary>
    /// Gets the MessageDto of a Message.
    /// </summary>
    public static Expression<Func<Message, MessagesDto>> FromMessage { get; } = message => new MessagesDto()
    {
        Id = message.Id,
        Content = message.Content,
        Sent = message.Sent,
        UserId = message.UserId,
        ChatId = message.ChatId
    };
}
