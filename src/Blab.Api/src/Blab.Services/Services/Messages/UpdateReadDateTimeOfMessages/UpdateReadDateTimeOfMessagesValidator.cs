using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.Messages.UpdateReadDateTimeOfMessages;

/// <summary>
/// This validator ensures that the request is valid before it starts to execute more complex logic or more commands to the database.
/// </summary>
public class UpdateReadDateTimeOfMessagesValidator : AbstractValidator<UpdateReadDateTimeOfMessagesCommand>
{
    private readonly BlabDbContext _context;

    public UpdateReadDateTimeOfMessagesValidator(BlabDbContext context)
    {
        _context = context;

        RuleFor(command => command.LoggedInUserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("LoggedInUserId cannot be null or empty")
            .MustAsync(LoggedInUserExistsAsync)
            .WithMessage("No user matches the requested UserId");

        RuleFor(command => command.ChatId)
            .NotEmpty()
            .NotNull()
            .WithMessage("ChatId can not be null or empty.")
            .MustAsync(ChatIdExistsAsync)
            .WithMessage("No Chat exists with the requested ChatId");

        RuleFor(command => command)
            .MustAsync(UserBelongsToChatAsync)
            .WithMessage("User does not belong to that chat");
    }

    public async Task<bool> LoggedInUserExistsAsync(int userId, CancellationToken cancellation)
    {
        return await _context.Users.SingleOrDefaultAsync(user => user.Id == userId) != null;
    }

    public async Task<bool> ChatIdExistsAsync(int chatId, CancellationToken cancellation)
    {
        return await _context.Chats.SingleOrDefaultAsync(chat => chat.Id == chatId) != null;
    }

    public async Task<bool> UserBelongsToChatAsync(UpdateReadDateTimeOfMessagesCommand command, CancellationToken cancellation)
    {
        return await _context.UserChats.SingleOrDefaultAsync(userChat => userChat.ChatId == command.ChatId && userChat.UserId == command.LoggedInUserId) != null;
    }
}
