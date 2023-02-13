using Blab.Domain.Entities.Models;
using System.Linq.Expressions;

namespace Blab.Services.Services.UserChats.GetAllChatsForLoggedInUser;

/// <summary>
/// Return DTO for each chat found for the logged in user. This information is all neded for the chat navigation component in the front end.
/// This is only for chats with two people. Group chat functionality is not accounted for.
/// The Messages properties can be null if it is a new chat and no messages have been sent yet.
/// </summary>
public class GetAllChatsDto
{
    /// <summary>
    /// Gets or sets the ChatId of the chat the Logged in user is in.
    /// </summary>
    public int ChatId { get; set; }

    /// <summary>
    ///  Gets or sets the UserId of the other user in the chat.
    /// </summary>
    public int OtherUserId { get; set; }

    /// <summary>
    /// Gets or sets the Display Name of the other user that is in the Chat with the logged in user.
    /// </summary>
    public string OtherUserDisplayName { get; set; }

    /// <summary>
    /// Gets or sets the user id of the person who sent the last message.
    /// So that the css on the front end can be differnet, depending on who sent the last message
    /// </summary>
    public int? UserIdOfLastMessageSender { get; set; }

    /// <summary>
    /// Gets or sets the time of the latest message sent so the order in which chats are displayed can be determined based on the which chat had the most recent last message.
    /// </summary>
    public DateTime? TimeOfLatestMessage { get; set; }

    /// <summary>
    /// The content of the latest message is also provided so that it can be easily dsiplayed in the chat preview.
    /// This means that not all the messages of a chat need to be requested before a chat is viewed.
    /// </summary>
    public string? ContentOfLastMessage { get; set; }

    /// <summary>
    /// If the last message has been read, this property will be true. 
    /// </summary>
    public bool? HasLastMessageBeenRead { get; set; }

    /// <summary>
    /// Gets or sets the blobName of the profile photo.
    /// </summary>
    public string ProfilePhotoBlob { get; set; } = string.Empty;

    public static Expression<Func<Chat, int, string, GetAllChatsDto>> FromChat { get; } = (chat, loggedInUserId, profileContainerUrl) => new GetAllChatsDto
    {
        ChatId = chat.Id,
        OtherUserId = chat.UserChats.Single(userChat => userChat.UserId != loggedInUserId).UserId,
        OtherUserDisplayName = chat.UserChats.Single(userChat => userChat.UserId != loggedInUserId).User.DisplayName,
        UserIdOfLastMessageSender = chat.Messages.OrderByDescending(message => message.Sent).Any() ? chat.Messages.OrderByDescending(message => message.Sent).First().UserId : null,
        TimeOfLatestMessage = chat.Messages.OrderByDescending(message => message.Sent).Any() ? chat.Messages.OrderByDescending(message => message.Sent).First().Sent : null,
        ContentOfLastMessage = chat.Messages.OrderByDescending(message => message.Sent).Any() ? chat.Messages.OrderByDescending(message => message.Sent).First().Content : null,
        HasLastMessageBeenRead = chat.Messages.Any()
                                                ? chat.Messages.OrderByDescending(message => message.Sent).Any()
                                                    && chat.Messages.OrderByDescending(message => message.Sent).First().ReadDateTime.HasValue
                                                : null,
        ProfilePhotoBlob = profileContainerUrl + chat.UserChats.Single(userChat => userChat.UserId != loggedInUserId).User.ProfilePhoto.BlobName
    };
}