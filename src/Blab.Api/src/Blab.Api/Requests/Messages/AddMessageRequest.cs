using System.Text.Json.Serialization;

namespace Blab.Api.Requests.Messages;

/// <summary>
/// The AddMessage request only requires the UserId of the User sending the message as the ChatId is obtained through the URL. 
///  The Content is the actual message itself that is sent in the chat. 
/// </summary>
public class AddMessageRequest
{
    /// <summary>
    /// Gets or Sets the UserId of the User who is sending the Message.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the content of the message that is being added to the chat.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets ChatId.
    ///JsonIgnore means this does not appear on the Swagger Documentation, but we can still use it as a property on a class for other matters, such as validating.
    /// </summary>
    [JsonIgnore]
    public int ChatId { get; set; } 
}
