using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Users.SearchForUsers;
public class SearchForUsersValidator : AbstractValidator<SearchForUsersCommand>
{
    private readonly IBlabDbContext _context;

    public SearchForUsersValidator(IBlabDbContext context)
    {
        _context = context;

        RuleFor(search => search.PageNumber)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page Number can not be negative.");

        RuleFor(search => search.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Must be a Page Size greater than 0.");

        RuleFor(search => search.SearchTerm)
            .NotEmpty()
            .NotNull()
            .WithMessage("Search term cannot be empty or null")
            .MinimumLength(2)
            .WithMessage("Search term must be at least 2 characters long.");

        RuleFor(search => search)
            .MustAsync(IsPageNumberValidAsync)
            .WithMessage("That page number doesnt exist, not enough users to generate a page number of that size");

    }

    public async Task<bool> IsPageNumberValidAsync(SearchForUsersCommand command, CancellationToken cancellation)

        // If the page number is 0 (the first page), but the page size is larger than the number of blabs found, all the blabs found should still be returned.
        // However, in all other cases, the total number of pages is determined by the number of blabs found divided by the page size.
        // If the PageNumber is greater than the total number of pages, than an invalid PageNumber error should be returned
    {
        //Lists of valid users, handle priority over display name
         var totalUsersMatchingTheSearchTerm = await _context.Users.AsNoTracking().Where(users => users.UserName.Contains(command.SearchTerm) || users.DisplayName.Contains(command.SearchTerm)).ToListAsync();

        if (command.PageNumber == 0)
        {
            return true;
        }
        
        // This must be included as division by 0 is not possible.
        if (command.PageSize != 0)
        {
            var totalPages = (int)Math.Ceiling((decimal)totalUsersMatchingTheSearchTerm.Count / (decimal)command.PageSize) - 1;
            return command.PageNumber <= totalPages;
        }

        return false;
    }
}