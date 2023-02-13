using Audacia.Commands;

namespace Blab.Services.Services.Follows.DeleteFollow;
public class DeleteFollowCommand : ICommand
{
    public int LoggedInUserId { get; set; }

    public int FolloweeId { get; set; }

    public DeleteFollowCommand(int loggedInUserId, int followeeId)
    {
        LoggedInUserId = loggedInUserId;
        FolloweeId = followeeId;
    }
}
