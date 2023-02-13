using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Reactions.UpdateReaction;

/// <summary>
/// Validator for UpdateReactionCommand.
/// </summary>
public class UpdateReactionValidator : AbstractValidator<UpdateReactionCommand>
{
    private readonly BlabDbContext _context;

    /// <summary>
    /// Validation rules for UpdateReactionCommand.
    /// </summary>
    /// <param name="context"></param>
    public UpdateReactionValidator(BlabDbContext context)
    {
        _context = context;

        RuleFor(updateReactionCommand => updateReactionCommand.PostId)
            .NotEmpty()
            .Must(postId => _context.Posts.AsNoTracking().Any(post => post.Id == postId))
            .WithMessage("A blab with that Id does not exist.");
        RuleFor(updateReactionCommand => updateReactionCommand.UserId)
            .NotEmpty()
            .Must(userId => _context.Users.AsNoTracking().Any(user => user.Id == userId))
            .WithMessage("A user with that Id does not exist.");
        RuleFor(x => new { x.UserId, x.PostId })
            .Must(x => _context.Reactions.AsNoTracking().Any(reaction => reaction.UserId == x.UserId && reaction.PostId == x.PostId))
            .WithMessage("The user has not reacted to this blab.");
        RuleFor(addReactionCommand => addReactionCommand.ReactionType)
            .NotEmpty()
            .IsInEnum();
    }
}
