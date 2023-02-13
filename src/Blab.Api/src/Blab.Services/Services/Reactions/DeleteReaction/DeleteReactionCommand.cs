using Audacia.Commands;

namespace Blab.Services.Services.Reactions.DeleteReaction;
public class DeleteReactionCommand : ICommand
{
    public int PostId { get; }

    public int UserId { get; }

    public DeleteReactionCommand(int postId, int userId)
    {
        PostId = postId;
        UserId = userId;
    }
}
