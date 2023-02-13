using Blab.Services.Services.Common;

namespace Blab.Services.Services.Reactions.AddReaction;
public interface IAddReactionService : ICommandHandler<AddReactionCommand, int>
{
}
