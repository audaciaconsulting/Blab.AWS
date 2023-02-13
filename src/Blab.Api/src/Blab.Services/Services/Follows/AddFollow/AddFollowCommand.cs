using Audacia.Commands;

namespace Blab.Services.Services.Follows.AddFollow;
public class AddFollowCommand : ICommand
{
    public int FollowerId { get; set; }

    public int FolloweeId { get; set; }

    public int LoggedInUserId { get; set; }

    public AddFollowCommand(int followerId, int followeeId, int loggedInUserId)
    {
        FollowerId = followerId;
        FolloweeId = followeeId;
        LoggedInUserId = loggedInUserId;
    }
}
