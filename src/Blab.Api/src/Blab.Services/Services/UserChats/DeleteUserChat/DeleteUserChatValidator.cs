using Blab.DataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.UserChats.DeleteUserChat;

public class DeleteUserChatValidator : AbstractValidator<DeleteUserChatCommand>
{
    private readonly IBlabDbContext _dbContext;

    public DeleteUserChatValidator(IBlabDbContext context)
    {
        _dbContext = context;

        RuleFor(userChat => userChat.ChatId)
            .NotNull()
            .NotEqual(0)
            .WithMessage("Chat Id needs to be a valid Id.")
            .MustAsync(DoesChatExistAsync)
            .WithMessage(userChat => $"Unable to find Chat with Id: {userChat.ChatId}")
            .DependentRules(() =>
            {
                RuleFor(userChat => userChat.LoggedInUserId)
                    .MustAsync(UserApartOfChatAsync)
                    .WithMessage("You are not a part of this chat, therefore do not have permission to delete it.");
            });
    }

    private async Task<bool> DoesChatExistAsync(DeleteUserChatCommand command, int chatId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Chats.AnyAsync(chat => chat.Id == command.ChatId, cancellationToken: cancellationToken);
    }

    private async Task<bool> UserApartOfChatAsync(DeleteUserChatCommand command, int loggedInUserId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.UserChats.AnyAsync(
            userChat => userChat.ChatId == command.ChatId && userChat.UserId == loggedInUserId,
            cancellationToken: cancellationToken);
    }
}
