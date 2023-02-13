using Blab.Domain.Entities.Security;

namespace Blab.Domain.Entities.Models;

/// <summary>
/// This Message model has the reference to the User it is sent by and the Chat that the Message belongs to. 
/// </summary>
public class Message
{
    /// <summary>
    /// Gets the Id of the Message. This is a readonly property as the Id of a Message should not change from the one generated in the database.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets or sets the Chat that the message is sent into.
    /// </summary>
    public int ChatId { get; set; }

    /// <summary>
    /// Gets or set navigational property so that the ChatId comes from the specified Chat. 
    /// </summary>
    public Chat Chat { get; set; }

    /// <summary>
    /// Gets or sets the User that is sending the message.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the navigational property so that the UserId comes from the specified User. 
    /// </summary>
    public ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets the content of the message. 
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets time the message was sent in the chat
    /// </summary>
    public DateTime Sent { get; set; }

    /// <summary>
    /// By default the ReadDateTime is set to null, meaning that the message has been unread. Once viewed in the app by the other user in the chat, the value is changed to the DateTime that it was read.
    /// </summary>
    public DateTime? ReadDateTime { get; set; } = null;
}
