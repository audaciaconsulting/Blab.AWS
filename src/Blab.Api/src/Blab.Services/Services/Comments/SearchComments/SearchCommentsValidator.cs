using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Comments.SearchComments;
public class SearchCommentsValidator : AbstractValidator<SearchCommentsCommand>
{
    public readonly IBlabDbContext _context;

    public SearchCommentsValidator(IBlabDbContext context, CancellationToken cancellationToken)
    {
        _context = context;

        RuleFor(searchCommentsCommand => searchCommentsCommand.PageNumber)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page number cannot be negative.");
        RuleFor(searchCommentsCommand => searchCommentsCommand.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");
        RuleFor(searchCommentsCommand => searchCommentsCommand.PostId)
            .NotEmpty()
            .MustAsync(isValidPostAsync)
            .WithMessage("A blab with that Id doesn't exist.");
        RuleFor(searchCommentsCommmand => searchCommentsCommmand)
            .MustAsync(isPageNumberValidAsync)
            .WithMessage("Page number is out of range.");
    }

    private async Task<bool> isPageNumberValidAsync(SearchCommentsCommand command, CancellationToken cancellationToken)
    {
        var comments = await _context.Posts.AsNoTracking().Where(post => post.Id == command.PostId).SelectMany(post => post.Comments).ToListAsync();

        if (command.PageNumber == 0)
        {
            return true;

        }
        else if (command.PageSize > 0)
        {
            var totalPages = ((int)Math.Ceiling((decimal)comments.Count / (decimal)command.PageSize)) - 1;
            return command.PageNumber <= totalPages;
        }

        return false;
    }

    private async Task<bool> isValidPostAsync(int postId, CancellationToken cancellationToken)
    {
        return await _context.Posts.AsNoTracking().AnyAsync(post => post.Id == postId, cancellationToken);
    }
}
