using Blab.Domain.Entities.Security;
using Blab.Services.Services.Users.UpdateUser;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Users.UpdateUser;

/// <summary>
/// Validator for UpdateUserCommand class. 
/// </summary>
public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IQueryable<ApplicationUser> _users;

    private readonly int _userId;

    /// <summary>
    /// Validation rules for UpdateUserCommand.
    /// </summary>
    public UpdateUserValidator(IQueryable<ApplicationUser> users, int userId)
    {
        _users = users;
        _userId = userId;

        RuleFor(updateUserCommand => updateUserCommand.UserId)
            .NotEmpty();
        RuleFor(updateUserCommand => updateUserCommand)
            .Must(x => x.LoggedInUserId == x.UserId)
            .WithMessage("Id provided does not match the logged in user.");
        RuleFor(updateUserCommand => updateUserCommand.Handle)
            .NotEmpty()
            .Length(2, 64)
            .Matches("^[a-zA-Z0-9_~.-]*$")
            .WithMessage(
                "The handle should only consist of Letters, Numbers and the following special characters: -_.~")
            .MustAsync(HandleBeUniqueAsync)
            .WithMessage("The handle needs to be unique.");
        RuleFor(updateUserCommand => updateUserCommand.DisplayName)
            .NotEmpty()
            .Length(2, 128);
    }

    private async Task<bool> HandleBeUniqueAsync(UpdateUserCommand updateUserCommand, string newHandle,
        CancellationToken cancellationToken)
    {
        var isHandleUnique = await _users.AsNoTracking().AnyAsync(
            user => user.UserName == newHandle && user.Id != _userId,
            cancellationToken: cancellationToken);

        return !isHandleUnique;
    }
}
