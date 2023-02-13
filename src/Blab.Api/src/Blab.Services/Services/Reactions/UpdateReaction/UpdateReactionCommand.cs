using Audacia.Commands;
using Blab.Domain.Models;
using Blab.Domain.Models.Reactions;

namespace Blab.Services.Services.Reactions.UpdateReaction;
public class UpdateReactionCommand : ICommand
{
    public int PostId { get; }

    public int UserId { get; }

    public ReactionType ReactionType { get; }

    public UpdateReactionCommand(int postId, int userId, ReactionType reactionType)
    {
        PostId = postId;
        UserId = userId;
        ReactionType = reactionType;
    }
}
