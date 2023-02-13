using Blab.Api.Requests.Messages;
using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Api.Validators.Messages;

/// <summary>
/// Validator to check that a message is not empty. 
/// If further validation is needed, it can be added here easily, and code elsewhere will not need to be changed. 
/// </summary>
public class AddMessageValidator : AbstractValidator<AddMessageRequest>
{
    private readonly IBlabDbContext _context;
    private readonly int _chatId;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddMessageValidator"/> class.
    /// </summary>
    public AddMessageValidator(IBlabDbContext context, int chatId)
    {
        _context = context;
        _chatId = chatId;

        // This validates the UserId inputed in the request.
        RuleFor(addMessageRequest => addMessageRequest.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserId cannot be null or empty.")
            .MustAsync(UserExistsAsync)
            .WithMessage("User does not exist")
            .MustAsync(UserExistsInSpecifiedChatAsync)
            .WithMessage("User does not belong to that chat");

        // This validates the content put in the request. 
        RuleFor(addMessageRequest => addMessageRequest.Content)
            .NotEmpty()
            .NotNull()
            .WithMessage("Content can not be empty");

        RuleFor(addMessageRequest => addMessageRequest.ChatId)
            .MustAsync(ChatExistsAsync)
            .WithMessage("Chat does not exist");
    }

    private async Task<bool> UserExistsAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Id == userId, cancellationToken: cancellationToken) != null;
    }

    private async Task<bool> UserExistsInSpecifiedChatAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.UserChats.AsNoTracking().SingleOrDefaultAsync(userChat => userChat.ChatId == _chatId && userChat.UserId == userId) != null;
    }

    private async Task<bool> ChatExistsAsync(int chatId, CancellationToken cancellationToken)
    {
        return await _context.Chats.AsNoTracking().SingleOrDefaultAsync(theChat => theChat.Id == chatId) != null;
    }
}
