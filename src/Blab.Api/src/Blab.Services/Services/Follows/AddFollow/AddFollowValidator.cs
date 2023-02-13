using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Follows.AddFollow;
public class AddFollowValidator : AbstractValidator<AddFollowCommand>
{
    private readonly BlabDbContext _context;

    public AddFollowValidator(BlabDbContext context)
    {
        _context = context;

        RuleFor(addFollowCommand => addFollowCommand.FollowerId)
            .NotEmpty()
            .MustAsync(UserExistsAsync)
            .WithMessage("Follower does not exist.");
        RuleFor(addFollowCommand => addFollowCommand.FolloweeId)
            .NotEmpty()
            .MustAsync(UserExistsAsync)
            .WithMessage("Followee does not exist.");
        RuleFor(addFollowCommand => addFollowCommand)
            .Must(x => x.FollowerId == x.LoggedInUserId)
            .WithMessage("Id provided does not match the logged in user.")
            .Must(x => x.FolloweeId != x.FollowerId)
            .WithMessage("Cannot follow yourself.")
            .MustAsync(UserIsNotFollowingAsync)
            .WithMessage("The user is already following.");
    }

    private async Task<bool> UserExistsAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Id == userId, cancellationToken) != null;
    }

    private async Task<bool> UserIsNotFollowingAsync(AddFollowCommand addFollowCommand, CancellationToken cancellationToken)
    {
        return !await _context.Follows.AsNoTracking().AnyAsync(
            follows =>
            follows.FollowerId == addFollowCommand.FollowerId &&
            follows.FolloweeId == addFollowCommand.FolloweeId, cancellationToken);
    }
}
