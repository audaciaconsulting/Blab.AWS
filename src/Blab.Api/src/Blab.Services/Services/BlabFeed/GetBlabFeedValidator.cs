using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.BlabFeed;

public class GetBlabFeedValidator : AbstractValidator<GetBlabFeedCommand>
{
    private readonly BlabDbContext _context;

    private readonly bool _forUserProfileFeed;

    /// <summary>
    /// Validator for Getting your own Blab Feed. 
    /// </summary>
    /// <param name="context"></param>
    public GetBlabFeedValidator(BlabDbContext context, bool forUserProfileFeed)
    {
        _context = context;
        _forUserProfileFeed = forUserProfileFeed;

        RuleFor(request => request)
            .MustAsync(IsPageNumberValidAsync)
            .WithMessage(
                "Page number requested does not exist. Not enough posts to generate a page number of that size");

        RuleFor(request => request.UserId)
            .MustAsync(IsAValidUserIdAsync)
            .WithMessage("That UserId does not exist");

        RuleFor(request => request.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be a postive integer");

        RuleFor(request => request.PageNumber)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page Number cant be negative");
    }

    public async Task<bool> IsAValidUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(user => user.Id == userId, cancellationToken: cancellationToken);
    }

    public async Task<bool> IsPageNumberValidAsync(GetBlabFeedCommand command, CancellationToken cancellationToken)
    {
        int totalUserBlabs;
        if (_forUserProfileFeed)
        {
            totalUserBlabs = await _context.Posts.AsNoTracking()
                .Where(post => post.UserId == command.UserId)
                .CountAsync(cancellationToken: cancellationToken);
        }
        else
        {
            totalUserBlabs = await _context.Posts.AsNoTracking()
                .Where(post =>
                    post.UserId == command.UserId ||
                    post.User.Followee.Any(followee => followee.FollowerId == command.UserId))
                .CountAsync(cancellationToken: cancellationToken);
        }

        if (command.PageNumber == 0)
        {
            return true;
        }

        if (command.PageSize != 0)
        {
            var totalPages =
                (int) Math.Floor((decimal) (totalUserBlabs / command.PageSize));
            return command.PageNumber <= totalPages;
        }

        return false;
    }
}
