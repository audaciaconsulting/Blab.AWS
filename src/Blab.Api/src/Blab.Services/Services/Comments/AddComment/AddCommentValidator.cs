using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Comments.AddComment;

/// <summary>
/// Validator for AddCommentCommand.
/// </summary>
public class AddCommentValidator : AbstractValidator<AddCommentCommand>
{
    private readonly IBlabDbContext _context;

    /// <summary>
    /// Validation rules for AddCommentCommand.
    /// </summary>
    /// <param name="context"></param>
    public AddCommentValidator(IBlabDbContext context, CancellationToken cancellationToken)
    {
        _context = context;

        RuleFor(addCommentCommand => addCommentCommand.PostId)
            .NotEmpty()
            .MustAsync(DoesPostExistAsync)
            .WithMessage("A blab with that Id does not exist.");
        RuleFor(addCommentCommand => addCommentCommand.UserId)
            .NotEmpty()
            .MustAsync(DoesUserExistAsync)
            .WithMessage("A user with that Id does not exist.");
        RuleFor(addCommentCommand => addCommentCommand.Content)
            .NotEmpty()
            .MaximumLength(256)
            .WithMessage("Comment can be a maximum of 256 characters.");
    }

    private async Task<bool> DoesPostExistAsync(int postId, CancellationToken cancellationToken)
    {
        return await _context.Posts.AsNoTracking().AnyAsync(post => post.Id == postId, cancellationToken);
    }

    private async Task<bool> DoesUserExistAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Users.AsNoTracking().AnyAsync(user => user.Id == userId, cancellationToken);
    }
}
