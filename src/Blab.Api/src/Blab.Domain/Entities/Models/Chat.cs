using Blab.Domain.Entities.Security;

namespace Blab.Domain.Entities.Models;

/// <summary>
/// This Chat Class defines the properties on the class.
/// A chat has many users (at the moment this is confined to 2), and a user can have many chats. 
/// </summary>
public class Chat
{
    /// <summary>
    /// Gets or sets the Id of the Chat.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///  Gets or sets a collection of Users in the chat.
    /// </summary>
    public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();

    /// <summary>
    /// The UserChats is a the navigational property for the join table as the User and Chats relationship is many to many. 
    /// </summary>
    public ICollection<UserChats> UserChats { get; set; } = new HashSet<UserChats>();

    /// <summary>
    /// Gets or sets a collections of messages in a chat.
    /// </summary>
    public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    
    /// <summary>
    /// This empty constructor is for tests.
    /// </summary>
    public Chat()
    {

    }

    /// <summary>
    /// This constructor defines the relation of a chat between two users.
    /// </summary>
    /// <param name="userOneId"></param>
    /// <param name="userTwoId"></param>
    public Chat(int userOneId, int userTwoId)
    {
        var userChatOne = new UserChats() { UserId = userOneId };
        var userChatTwo = new UserChats { UserId = userTwoId };
        UserChats.Add(userChatOne);
        UserChats.Add(userChatTwo);
    }
}
