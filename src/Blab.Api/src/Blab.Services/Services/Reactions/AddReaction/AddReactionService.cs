using Blab.DataAccess;
using Blab.Domain.Models;
using Blab.Domain.Models.Reactions;
using Blab.Services.Extensions.FluentValidationExtensions;
using Blab.Services.Models.Validation;
using FluentValidation.Results;

namespace Blab.Services.Services.Reactions.AddReaction;
public class AddReactionService : IAddReactionService
{
    private readonly BlabDbContext _context;

    public AddReactionService(BlabDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResult<int>> HandleAsync(AddReactionCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);
        if (validationResult.IsValid)
        {
            var newReaction = new Reaction
            {
                PostId = command.PostId,
                UserId = command.UserId,
                Created = DateTime.Now,
                ReactionType = command.ReactionType
            };

            _context.Reactions.Add(newReaction);
            await _context.SaveChangesAsync(cancellationToken);

            return new CommandResult<int>(newReaction.Id);
        }

        return CommandResult.Failure<int>(validationResult.Errors.ErrorMessages().ToList());
    }

    private async Task<ValidationResult> ValidateCommandAsync(AddReactionCommand command, CancellationToken cancellationToken)
    {
        var validator = new AddReactionValidator(_context);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        return validationResult;
    }
}
