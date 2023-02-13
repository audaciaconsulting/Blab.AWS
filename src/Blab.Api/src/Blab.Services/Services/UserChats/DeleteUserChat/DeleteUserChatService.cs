using System.Globalization;
using Blab.DataAccess;
using Blab.Domain.Entities.Security;
using Blab.Services.Extensions.FluentValidationExtensions;
using Blab.Services.Models.Validation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blab.Services.Services.UserChats.DeleteUserChat;

public class DeleteUserChatService : IDeleteUserChatService
{
    private readonly BlabDbContext _context;

    public DeleteUserChatService(BlabDbContext context)
    {
        _context = context;
    }

    public async Task<CommandResult<bool>> HandleAsync(
        DeleteUserChatCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await ValidateCommandAsync(command, cancellationToken);

        if (validationResult.IsValid)
        {
            var entity = await _context.Chats.SingleOrDefaultAsync(chat => chat.Id == command.ChatId,
                cancellationToken: cancellationToken);

            if (entity is null)
            {
                return CommandResult.Failure<bool>(new PropertyErrorMessage("chatId",
                    string.Format(CultureInfo.InvariantCulture,
                        "Unable to find Chat with Id: {0}", command.ChatId)));
            }

            _context.Chats.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var commandResultWithEntity = new CommandResult<bool>(true);

            return commandResultWithEntity;
        }

        return CommandResult.Failure<bool>(validationResult.Errors.ErrorMessages().ToList());
    }

    private async Task<ValidationResult> ValidateCommandAsync(
        DeleteUserChatCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new DeleteUserChatValidator(_context);
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        return validationResult;
    }
}
