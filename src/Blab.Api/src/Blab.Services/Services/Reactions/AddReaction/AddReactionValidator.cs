using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Reactions.AddReaction;

/// <summary>
/// Validator for AddReactionCommand.
/// </summary>
public class AddReactionValidator : AbstractValidator<AddReactionCommand>
{
    private readonly BlabDbContext _context;

    /// <summary>
    /// Validation rules for AddReactionCommand.
    /// </summary>
    /// <param name="context"></param>
    public AddReactionValidator(BlabDbContext context)
    {
        _context = context;

        RuleFor(addReactionCommand => addReactionCommand)
            .MustAsync(UserHasNotAlreadyReactedAsync)
            .WithMessage("The user has already reacted to this post.");
        RuleFor(addReactionCommand => addReactionCommand.PostId)
            .NotEmpty()
            .Must(postId => _context.Posts.AsNoTracking().Any(post => post.Id == postId))
            .WithMessage("A blab with that Id does not exist.");
        RuleFor(addReactionCommand => addReactionCommand.UserId)
            .NotEmpty()
            .Must(userId => _context.Users.AsNoTracking().Any(user => user.Id == userId))
            .WithMessage("A user with that Id does not exist.");
        RuleFor(addReactionCommand => addReactionCommand.ReactionType)
            .IsInEnum()
            .WithMessage("A reaction type with that value does not exist.");
    }

    private async Task<bool> UserHasNotAlreadyReactedAsync(AddReactionCommand addReactionCommand, CancellationToken cancellationToken)
    {
        return !await _context.Reactions.AsNoTracking().AnyAsync(reaction => reaction.UserId == addReactionCommand.UserId && reaction.PostId == addReactionCommand.PostId);
    }
}
