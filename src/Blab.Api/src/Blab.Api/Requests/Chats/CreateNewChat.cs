namespace Blab.Api.Requests.Chats;

/// <summary>
/// Request body for creating a new chat.
/// </summary>
public class CreateNewChat
{
   /// <summary>
   /// Gets or sets the UserId of the User that is logged in and is the User that is Creating the chat.
   /// </summary>
    public int LoggedInUserId { get; set; }

    /// <summary>
    /// Gets or sets the UserId of the user that the logged in user wants to create a chat with.
    /// This could be changed to an ICollection to adapt for group chat functionality.
    /// </summary>
    public int OtherUserId { get; set; }
}
