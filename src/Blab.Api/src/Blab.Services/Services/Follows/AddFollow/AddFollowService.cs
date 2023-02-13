using Audacia.Core.Extensions;
using Blab.Services.Models.Validation;
using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Blab.Domain.Models;
using Blab.Services.Extensions.FluentValidationExtensions;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;

namespace Blab.Services.Services.Follows.AddFollow;
public class AddFollowService : IAddFollowService
{
    private readonly BlabDbContext _context;

    public AddFollowService(BlabDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResult<bool>> HandleAsync(AddFollowCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return CommandResult.Failure<bool>(validationResult.Errors.ErrorMessages().ToList());
        }

        var newFollow = new Follow()
        {
            FollowerId = command.FollowerId,
            FolloweeId = command.FolloweeId
        };

        _context.Follows.Add(newFollow);
        await _context.SaveChangesAsync(cancellationToken);

        return new CommandResult<bool>(true);
    }

    private async Task<ValidationResult> ValidateCommandAsync(AddFollowCommand command, CancellationToken cancellationToken)
    {
        var validator = new AddFollowValidator(_context);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        return validationResult;
    }
}
