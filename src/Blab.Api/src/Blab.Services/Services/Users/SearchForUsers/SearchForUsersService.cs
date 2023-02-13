using Blab.DataAccess;
using Blab.Services.Extensions.FluentValidationExtensions;
using Blab.Services.Models.Validation;
using FluentValidation.Results;
using Audacia.Core;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Users.SearchForUsers;
public class SearchForUsersService : ISearchForUsersService
{
    private readonly BlabDbContext _context;

    public SearchForUsersService(BlabDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResult<Page<FoundUserDto>>> HandleAsync(SearchForUsersCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);

        if (validationResult.IsValid)
        {
            // Lists of valid users, handle priority over display name.
            // Each user found matching the search term is transformed to only return the relevant properties in the FoundUserDto.

            var pagingRequest = new PagingRequest(command.PageSize, command.PageNumber);

            var results = (from user in _context.Users.AsNoTracking()
                           where user.DisplayName.Contains(command.SearchTerm) || user.UserName.Contains(command.SearchTerm)
                           select new
                           {
                               user = FoundUserDto.FromUser.Compile().Invoke(user),
                               usernameContainsTerm = user.UserName.Contains(command.SearchTerm),
                               displayNameContainsTerm = user.DisplayName.Contains(command.SearchTerm)
                           })
                          .OrderByDescending(x => x.usernameContainsTerm)
                          .ThenByDescending(x => x.displayNameContainsTerm)
                          .AsQueryable()
                          .Select(x => x.user);

            var page = new Page<FoundUserDto>(results, pagingRequest);

            return new CommandResult<Page<FoundUserDto>>(page);
        }

        return CommandResult.Failure<Page<FoundUserDto>>(validationResult.Errors.ErrorMessages().ToList());
    }

    private async Task<ValidationResult> ValidateCommandAsync(SearchForUsersCommand command, CancellationToken cancellationToken)
    {
        var validator = new SearchForUsersValidator(_context);
        return await validator.ValidateAsync(command, cancellationToken);
    }
}
