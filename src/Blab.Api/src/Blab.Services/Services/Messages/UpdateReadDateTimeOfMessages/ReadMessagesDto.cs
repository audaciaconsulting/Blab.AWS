using System.Linq.Expressions;
using Blab.Domain.Entities.Models;

namespace Blab.Services.Services.Messages.UpdateReadDateTimeOfMessages;

/// <summary>
/// This ReadMessagesDto is so that the endpoint returns the messages that had a null ReadDateTime property to the ReadDateTime Now property.
/// </summary>
public class ReadMessagesDto
{
    /// <summary>
    /// Gets or sets the message id of the message that is being changed from "unread" to "read".
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    /// Gets or sets the ReadDateTimeProperty which should be changed from null, having been considered "unread", 
    /// to the time that the chat was opened and the message now considered "read".
    /// </summary>
    public DateTime ReadDateTime { get; set; }

    /// <summary>
    /// This expression is to ensure that only these two properties of the messages are returned.
    /// </summary>
    public static Expression<Func<Message, ReadMessagesDto>> FromMessage { get; } = message => new ReadMessagesDto()
    {
        MessageId = message.Id,
        ReadDateTime = DateTime.Now
    };
}
