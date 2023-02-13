using Blab.Services.Services.Common;

namespace Blab.Services.Services.Reactions.DeleteReaction;
public interface IDeleteReactionService : ICommandHandler<DeleteReactionCommand, bool>
{
}
