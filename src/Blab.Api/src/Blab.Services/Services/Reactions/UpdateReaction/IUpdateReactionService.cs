using Blab.Services.Services.Common;

namespace Blab.Services.Services.Reactions.UpdateReaction;
public interface IUpdateReactionService : ICommandHandler<UpdateReactionCommand, bool>
{
}
