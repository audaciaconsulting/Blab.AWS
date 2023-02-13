using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Blab.DataAccess;
using Blab.Api.Requests.Chats;

namespace Blab.Api.Validators.Messages;

/// <summary>
/// Validator to check that a message is not empty. 
/// If further validation is needed, it can be added here easily, and code elsewhere will not need to be changed. 
/// </summary>
public class AddChatValidator : AbstractValidator<CreateNewChat>
{
    private readonly BlabDbContext _context;
    private readonly int _actualUserId;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddChatValidator"/> class.
    /// </summary>
    public AddChatValidator(BlabDbContext context, int actualUserId)
    {
        _context = context;
        _actualUserId = actualUserId;

        // This validates the UserId inputed in the request.
        RuleFor(chat => chat.LoggedInUserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("LoggedInUserId cannot be empty or null")
            .MustAsync(UserExistsAsync)
            .WithMessage("LoggedInUserId does not exist")
            .Must(DoesLoggedInUserIdMatch)
            .WithMessage("LoggedInUserId inputed does not match the actual UserId of the logged in User");

        RuleFor(chat => chat.OtherUserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("OtherUserId cannot be empty or null")
            .MustAsync(UserExistsAsync)
            .WithMessage("OtherUserId does not exist")
            .Must(UserIdsCannotBeTheSame)
            .WithMessage("UserIds can not be the same");

        RuleFor(chat => chat)
            .MustAsync(ChatDoesNotAlreadyExistBetweenTheTwoUsersAsync)
            .WithMessage("Chat already exists between the two users");
    }

    private async Task<bool> UserExistsAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Id == userId) != null;
    }

    private bool DoesLoggedInUserIdMatch(int userId)
    {
        return userId == _actualUserId;
    }

    private bool UserIdsCannotBeTheSame(int userId)
    {
        return userId != _actualUserId;
    }

    private async Task<bool> ChatDoesNotAlreadyExistBetweenTheTwoUsersAsync(CreateNewChat newChat, CancellationToken cancellationToken)
    {
        return await _context.Chats.AsNoTracking().SingleOrDefaultAsync(chat => chat.Users.Count(user => user.Id == newChat.OtherUserId || user.Id == newChat.LoggedInUserId) == 2) == null;
    }
}
