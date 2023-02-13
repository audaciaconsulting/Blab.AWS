using System.Globalization;
using Blab.DataAccess;
using Blab.Services.Extensions.FluentValidationExtensions;
using Blab.Services.Models.Validation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Reactions.UpdateReaction;
public class UpdateReactionService : IUpdateReactionService
{
    private readonly BlabDbContext _context;

    public UpdateReactionService(BlabDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResult<bool>> HandleAsync(UpdateReactionCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);

        if (validationResult.IsValid)
        {
            var postReaction = await _context.Reactions.SingleOrDefaultAsync(x => x.UserId == command.UserId && x.PostId == command.PostId, cancellationToken: cancellationToken);
            if (postReaction == null)
            {
                return CommandResult.Failure<bool>(new PropertyErrorMessage(
                    "userId",
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "User with Id: {0} has not reacted to Post with Id: {1}", command.UserId, command.PostId)));
            }

            postReaction.ReactionType = command.ReactionType;
            postReaction.Created = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken: cancellationToken);
            return new CommandResult<bool>(true);
        }

        return CommandResult.Failure<bool>(validationResult.Errors.ErrorMessages().ToList());
    }

    private async Task<ValidationResult> ValidateCommandAsync(UpdateReactionCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateReactionValidator(_context);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        return validationResult;
    }
}
