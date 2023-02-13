using Blab.Services.Models.Validation;
using Blab.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Reactions.DeleteReaction;
public class DeleteReactionService : IDeleteReactionService
{
    private readonly BlabDbContext _context;

    public DeleteReactionService(BlabDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResult<bool>> HandleAsync(DeleteReactionCommand command, CancellationToken cancellationToken = default)
    {
        var postReaction =
            await _context.Reactions.SingleOrDefaultAsync(
                x =>
                x.UserId == command.UserId && x.PostId == command.PostId, 
                cancellationToken: cancellationToken);
        if (postReaction is not null)
        {
            _context.Reactions.Remove(postReaction);
            await _context.SaveChangesAsync(cancellationToken: cancellationToken);
            return new CommandResult<bool>(true);
        }

        return CommandResult.Failure<bool>(new PropertyErrorMessage(
            "Reaction",
            "Logged in user has no reaction for this post."));
    }
}
