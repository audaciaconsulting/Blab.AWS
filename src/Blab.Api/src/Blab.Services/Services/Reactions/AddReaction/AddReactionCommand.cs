using Audacia.Commands;
using Blab.Domain.Models;
using Blab.Domain.Models.Reactions;

namespace Blab.Services.Services.Reactions.AddReaction;
public class AddReactionCommand : ICommand
{
    public int PostId { get; }

    public int UserId { get; }

    public ReactionType ReactionType { get; }

    public AddReactionCommand(int postId, int userId, ReactionType reactionType)
    {
        PostId = postId;
        UserId = userId;
        ReactionType = reactionType;
    }
}
