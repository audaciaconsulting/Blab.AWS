using Audacia.Commands;

namespace Blab.Services.Services.UserChats.GetAllChatsForLoggedInUser;
public class GetAllChatsForLoggedInUserCommand : ICommand
{
    /// <summary>
    /// Gets or sets the logged in user's Id.
    /// </summary>
    public int UserId { get; set; }

    public GetAllChatsForLoggedInUserCommand(int userId)
    {
        UserId = userId;
    }
}
